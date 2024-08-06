using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Character Data/Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public Race Race;
    public Sprite Icon;
}

public class CharacterData
{
    public string Name { get; set; }
    public Sprite Icon { get; set; }

    public Race Race;
    public ClassType Class;
    public int HitPoints { get; set; }
    public int Gold { get; set; }

    public CharacterData(Character characterSO)
    {
        Name = characterSO.name;
        Icon = characterSO.Icon;
        Race = characterSO.Race;
    }
}

[CreateAssetMenu(fileName = "Character Data/Attributes", menuName = "Race")]
public class Race : ScriptableObject
{
    public Sprite Icon;
}

[CreateAssetMenu(fileName = "Character Data/Attributes", menuName = "Class")]
public class ClassType : ScriptableObject
{
    public Sprite Icon;
}

public class CharacterDataManager : MonoBehaviour
{
    [SerializeField] private List<Character> _characters;
    private Dictionary<Character, CharacterData> _characterDatas = new();
    public int NumCharacters => _characters.Count;

    private void Awake()
    {
        foreach (Character character in _characters)
        {
            CharacterData newCharacter = new CharacterData(character);
            _characterDatas.Add(character, newCharacter);
        }
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

public class UICharacterAttributePanel : MonoBehaviour
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

    public void HandleCharacterChange(CharacterData data)
    {
        _raceText.text = data.Race.name;
        _classText.text = data.Class.name;

        SetClassDisplay(data);
    }

    private void SetClassDisplay(CharacterData data)
    {
        foreach (Image classImage in _displayedClassImages)
        {
            classImage.color = _defaultClassColor;
            classImage.gameObject.SetActive(false);
        }
    }
}
