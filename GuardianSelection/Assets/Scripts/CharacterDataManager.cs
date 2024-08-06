using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

[Serializable]
public struct DiceRoll
{
    public int Number;
    public int DiceSides;
}

[CreateAssetMenu(fileName = "Character Data/Character", menuName = "Character")]
public class Character : ScriptableObject
{
    [Header("Attributes")]
    public List<CharacterAttribute> Attributes;

    public DiceRoll AttributeDiceRoll;
    public DiceRoll HitPointsDiceRoll;
    public DiceRoll GoldDiceRoll;
    public Race Race;
    public Sprite Icon;
}

public class CharacterData
{
    public List<CharacterAttributeData> Attributes;
    public string Name { get; set; }
    public Sprite Icon { get; set; }
    public DiceRoll AttributeDiceRoll { get; set; }
    public DiceRoll HitPointsDiceRoll { get; set; }
    public DiceRoll GoldDiceRoll { get; set; }
    public Race Race;
    public ClassType Class;
    public int HitPoints { get; set; }
    public int Gold { get; set; }

    public CharacterData(Character characterSO)
    {
        Name = characterSO.name;
        Icon = characterSO.Icon;

        foreach (CharacterAttribute characterAttribute in characterSO.Attributes)
        {
            CharacterAttributeData newAttributeData = new CharacterAttributeData(characterAttribute);
            Attributes.Add(newAttributeData);
        }

        AttributeDiceRoll = characterSO.AttributeDiceRoll;
        HitPointsDiceRoll = characterSO.HitPointsDiceRoll;
        GoldDiceRoll = characterSO.GoldDiceRoll;
        Race = characterSO.Race;
    }
}

[CreateAssetMenu(fileName = "Character Data/Attributes", menuName = "Race")]
public class Race : ScriptableObject
{
    public struct StatModifier
    {
        public CharacterAttribute Stat;
        public int ModValue;
    }

    public List<StatModifier> MinimumStats;
    public List<StatModifier> StatModifiers;
    public RacialAbility RacialAbility;
    public List<ClassType> AllowedClasses;

    public int GetStatMinimumStat(CharacterAttributeData attribute)
    {
        StatModifier mod = MinimumStats.FirstOrDefault(x => x.Stat == attribute.AttributeType);
        return mod.ModValue;
    }
}

[CreateAssetMenu(fileName = "Character Data/Attributes", menuName = "Racial Ability")]
public class RacialAbility : ScriptableObject
{
}

[CreateAssetMenu(fileName = "Character Data/Attributes", menuName = "Class")]
public class ClassType : ScriptableObject
{
    public Sprite Icon;
    public DiceRoll HitDice;
    public CharacterAttribute PrimeStat;
}

public class CharacterAttribute : ScriptableObject
{
    public int Value;
    public AttributeMod Mods;
}

public class CharacterAttributeData
{
    public CharacterAttribute AttributeType;
    public string AttributeName;
    public int Value;
    public AttributeMod Mods;

    public CharacterAttributeData(CharacterAttribute characterAttribute)
    {
        AttributeName = characterAttribute.name;
        AttributeType = characterAttribute;
        Value = characterAttribute.Value;
        Mods = characterAttribute.Mods;
    }
}

/// <summary>
/// An set of values representing the min and max of an attribute
/// when finding what mod to add to an attribute.
/// Based on an attribute's value, this object can return the desired mod.
/// </summary>
public struct AttributeMod
{
    public struct AttributeModRange
    {
        public int MinValue;
        public int MaxValue;
        public int Mod;
    }

    public List<AttributeModRange> ModRanges;

    public int GetMod(int value)
    {
        int outOfRangeResult = 0;
        if (value == 0)
            value++;

        foreach (AttributeModRange modRange in ModRanges)
        {
            if (value >= modRange.MinValue && value <= modRange.MaxValue)
                return modRange.Mod;
            outOfRangeResult = modRange.Mod;
        }

        return outOfRangeResult;
    }
}

public class CharacterDataManager : MonoBehaviour
{
    [SerializeField] private List<Character> _characters;
    private Dictionary<Character, CharacterData> _characterDatas = new();
    private CharacterRandomizer _characterRandomizer;
    public int NumCharacters => _characters.Count;
    public Dictionary<Character, CharacterData> CharacterDatas => _characterDatas;
    public List<Character> Characters => _characters;

    private void Awake()
    {
        foreach (Character character in _characters)
        {
            CharacterData newCharacter = new CharacterData(character);
            _characterRandomizer.RandomizeCharacter(newCharacter);
            _characterDatas.Add(character, newCharacter);
        }
    }
}

public class CharacterRandomizer
{
    private CharacterAttributeCalculator _calc;

    public void RandomizeCharacter(CharacterData newCharacter)
    {
        _calc.CalculateAttributes(newCharacter);
        _calc.CalculateHitPoints(newCharacter);
        _calc.GenerateClass(newCharacter);
        _calc.CalculateGold(newCharacter);
    }
}

public class CharacterAttributeCalculator
{
    public void CalculateAttributes(CharacterData data)
    {
        foreach (CharacterAttributeData attribute in data.Attributes)
        {
            attribute.Value = GetDiceRollValue(data.AttributeDiceRoll.Number, data.AttributeDiceRoll.DiceSides);
            VerifyAttribute(attribute, data);
        }
    }

    private void VerifyAttribute(CharacterAttributeData attribute, CharacterData data)
    {
        if (attribute.Value < data.Race.GetStatMinimumStat(attribute))
        {
            attribute.Value = data.Race.GetStatMinimumStat(attribute);
        }
    }

    public void CalculateHitPoints(CharacterData data)
    {
        data.HitPoints = GetDiceRollValue(data.HitPointsDiceRoll.Number, data.HitPointsDiceRoll.DiceSides);

        int CON = data.Attributes[4].Value;
        data.HitPoints += data.Attributes[4].Mods.GetMod(CON);
    }

    public void GenerateClass(CharacterData data)
    {
        data.Class = GenerateApplicableClass(data.Race);
    }

    public void CalculateGold(CharacterData data)
    {
        data.Gold = GetDiceRollValue(data.GoldDiceRoll.Number, data.GoldDiceRoll.DiceSides);
    }

    private ClassType GenerateApplicableClass(Race dataRace)
    {
        int random = UnityEngine.Random.Range(0, dataRace.AllowedClasses.Count);
        return dataRace.AllowedClasses[random];
    }

    public int GetDiceRollValue(int number, int diceSides)
    {
        int value = 0;
        for (int i = 0; i < number; i++)
        {
            value += UnityEngine.Random.Range(1, diceSides + 1);
        }

        return value;
    }
}

public interface IUpdateOnCharacterChange
{
    public UICharacterSelectManager SelectManager { get; set; }
    public void HandleCharacterChange(CharacterData data);
}

public interface IUpdateCharacters
{
    public event Action<CharacterData> OnUpdateCharacters;
    public UICharacterSelectManager SelectManager { get; set; }
}

public class UICharacterSelectManager : MonoBehaviour
{
    public event Action<CharacterData> OnCharacterChange;
    public CharacterDataManager CharacterDataManager { get; set; }
    private CharacterData _currentSelectedCharacter;
    private List<IUpdateOnCharacterChange> _changeListeners;
    private List<IUpdateCharacters> _characterUpdaters;

    private void Awake()
    {
        CharacterDataManager = FindObjectOfType<CharacterDataManager>();
        _changeListeners = GetComponentsInChildren<IUpdateOnCharacterChange>().ToList();
        _characterUpdaters = GetComponentsInChildren<IUpdateCharacters>().ToList();
        foreach (IUpdateOnCharacterChange changeListener in _changeListeners)
        {
            OnCharacterChange += changeListener.HandleCharacterChange;
            changeListener.SelectManager = this;
        }

        foreach (IUpdateCharacters characterUpdater in _characterUpdaters)
        {
            characterUpdater.OnUpdateCharacters += UpdateSelectedCharacter;
            characterUpdater.SelectManager = this;
        }
    }

    public void UpdateSelectedCharacter(CharacterData data)
    {
        OnCharacterChange?.Invoke(data);
    }
}
public class UICharacterSelectPanel : MonoBehaviour
{
    public event Action<CharacterData> OnUpdateCharacters;
    public UICharacterSelectManager SelectManager { get; set; }

    [SerializeField] private Button _leftScrollButton;
    [SerializeField] private Button _rightScrollButton;
    [SerializeField] private TMP_Text _characterName;

    private List<UICharacterWidget> _characterWidgets = new();
    private int _viewableCharacterIndex;

    private void Awake()
    {
        //Set viewable index to middle of the characters on start
        _viewableCharacterIndex =
            Mathf.FloorToInt((SelectManager.CharacterDataManager.NumCharacters - 1) / 2);
        SetViewableCharacters(_viewableCharacterIndex);
    }

    private void OnEnable()
    {
        _leftScrollButton.onClick.AddListener(() => { IncrementCharacterSelect(-1); });

        _rightScrollButton.onClick.AddListener(() => { IncrementCharacterSelect(1); });
    }

    private void OnDisable()
    {
        _leftScrollButton.onClick.RemoveAllListeners();
        _rightScrollButton.onClick.RemoveAllListeners();
    }

    private void IncrementCharacterSelect(int increment)
    {
        _viewableCharacterIndex += increment;
        SetViewableCharacters(_viewableCharacterIndex);
    }

    private void SetViewableCharacters(int middleIndex)
    {
        int leftIndex = middleIndex - 1;
        int rightIndex = middleIndex + 1;

        for (int i = 0; i < 3; i++)
        {
            int positionIndex = middleIndex - (1 - i);
            if (positionIndex == -1 || positionIndex > SelectManager.CharacterDataManager.NumCharacters - 1)
                _characterWidgets[i].gameObject.SetActive(false);
            else
            {
                _characterWidgets[i].gameObject.SetActive(true);
                _characterWidgets[i].SetCharacter(SelectManager.CharacterDataManager.Characters[positionIndex]);
            }

            if (i == 1)
            {
                Character character = SelectManager.CharacterDataManager.Characters[positionIndex];
                _characterName.text = character.name;
                OnUpdateCharacters?.Invoke(SelectManager.CharacterDataManager.CharacterDatas[character]);
            }
        }
        
        //_characterWidgets[0].SetCharacter(SelectManager.CharacterDataManager.Characters[leftIndex]);
        //_characterWidgets[1].SetCharacter(SelectManager.CharacterDataManager.Characters[middleIndex]);
        //_characterWidgets[2].SetCharacter(SelectManager.CharacterDataManager.Characters[rightIndex]);
    }
}

public class UICharacterWidget : MonoBehaviour
{
    [SerializeField] private TMP_Text _characterName;
    [SerializeField] private Image _characterImage;

    public void SetCharacter(Character character)
    {
        _characterImage.sprite = character.Icon;
    }
}

public class UICharacterAttributePanel : MonoBehaviour, IUpdateOnCharacterChange
{
    [SerializeField] private TMP_Text _raceText;
    [SerializeField] private TMP_Text _classText;
    [SerializeField] private TMP_Text _racialAbilityText;

    [SerializeField] private Image _characterIcon;
    [SerializeField] private Image _raceImage;
    [SerializeField] private Image _classImage;
    [SerializeField] private List<Image> _displayedClassImages;

    [SerializeField] private Color _selectedClassColor = Color.blue;
    [SerializeField] private Color _defaultClassColor = Color.white;

    #region Character Attributes

    [Header("Attribute Text")]
    [SerializeField] private TMP_Text _strText;
    [SerializeField] private TMP_Text _intText;
    [SerializeField] private TMP_Text _wisText;
    [SerializeField] private TMP_Text _dexText;
    [SerializeField] private TMP_Text _conText;
    [SerializeField] private TMP_Text _chaText;

    [SerializeField] private TMP_Text _hpValueText;
    [SerializeField] private TMP_Text _strValueText;
    [SerializeField] private TMP_Text _intValueText;
    [SerializeField] private TMP_Text _wisValueText;
    [SerializeField] private TMP_Text _dexValueText;
    [SerializeField] private TMP_Text _conValueText;
    [SerializeField] private TMP_Text _chaValueText;

    [Header("Modifier Text")]
    [SerializeField] private TMP_Text _strModifierText;
    [SerializeField] private TMP_Text _intModifierText;
    [SerializeField] private TMP_Text _wisModifierText;
    [SerializeField] private TMP_Text _dexModifierText;
    [SerializeField] private TMP_Text _conModifierText;
    [SerializeField] private TMP_Text _chaModifierText;

    [Header("Totals")]
    [SerializeField] private TMP_Text _strTotalText;
    [SerializeField] private TMP_Text _intTotalText;
    [SerializeField] private TMP_Text _wisTotalText;
    [SerializeField] private TMP_Text _dexTotalText;
    [SerializeField] private TMP_Text _conTotalText;
    [SerializeField] private TMP_Text _chaTotalText;

    #endregion

    public UICharacterSelectManager SelectManager { get; set; }

    public void HandleCharacterChange(CharacterData data)
    {
        _raceText.text = data.Race.name;
        _classText.text = data.Class.name;
        _racialAbilityText.text = data.Race.RacialAbility.name;

        _strText.text = data.Attributes[0].AttributeName;
        _intText.text = data.Attributes[1].AttributeName;
        _wisText.text = data.Attributes[2].AttributeName;
        _dexText.text = data.Attributes[3].AttributeName;
        _conText.text = data.Attributes[4].AttributeName;
        _chaText.text = data.Attributes[5].AttributeName;

        _hpValueText.text = data.HitPoints.ToString();
        _strValueText.text = data.Attributes[0].Value.ToString();
        _intValueText.text = data.Attributes[1].Value.ToString();
        _wisValueText.text = data.Attributes[2].Value.ToString();
        _dexValueText.text = data.Attributes[3].Value.ToString();
        _conValueText.text = data.Attributes[4].Value.ToString();
        _chaValueText.text = data.Attributes[5].Value.ToString();

        _strModifierText.text = data.Race.StatModifiers[0].ModValue > 0 ? data.Race.StatModifiers[0].ModValue.ToString() : "";
        _intModifierText.text = data.Race.StatModifiers[1].ModValue > 0 ? data.Race.StatModifiers[1].ModValue.ToString() : "";
        _wisModifierText.text = data.Race.StatModifiers[2].ModValue > 0 ? data.Race.StatModifiers[2].ModValue.ToString() : "";
        _dexModifierText.text = data.Race.StatModifiers[3].ModValue > 0 ? data.Race.StatModifiers[3].ModValue.ToString() : "";
        _conModifierText.text = data.Race.StatModifiers[4].ModValue > 0 ? data.Race.StatModifiers[4].ModValue.ToString() : "";
        _chaModifierText.text = data.Race.StatModifiers[5].ModValue > 0 ? data.Race.StatModifiers[5].ModValue.ToString() : "";

        _strTotalText.text = (data.Attributes[0].Value + data.Race.StatModifiers[0].ModValue).ToString();
        _intTotalText.text = (data.Attributes[1].Value + data.Race.StatModifiers[1].ModValue).ToString();
        _wisTotalText.text = (data.Attributes[2].Value + data.Race.StatModifiers[2].ModValue).ToString();
        _dexTotalText.text = (data.Attributes[3].Value + data.Race.StatModifiers[3].ModValue).ToString();
        _conTotalText.text = (data.Attributes[4].Value + data.Race.StatModifiers[4].ModValue).ToString();
        _chaTotalText.text = (data.Attributes[5].Value + data.Race.StatModifiers[5].ModValue).ToString();
        SetClassDisplay(data);
    }

    private void SetClassDisplay(CharacterData data)
    {
        foreach (Image classImage in _displayedClassImages)
        {
            classImage.color = _defaultClassColor;
            classImage.gameObject.SetActive(false);
        }
        
        for (int i = 0; i < data.Race.AllowedClasses.Count; i++)
        {
            _displayedClassImages[i].gameObject.SetActive(true);
            if (data.Race.AllowedClasses[i] == data.Race)
                _displayedClassImages[i].color = _selectedClassColor;

            _displayedClassImages[i].sprite = data.Race.AllowedClasses[i].Icon;
        }
    }
}
