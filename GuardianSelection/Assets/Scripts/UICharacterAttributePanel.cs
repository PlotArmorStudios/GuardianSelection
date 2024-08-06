using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterAttributePanel : MonoBehaviour, IUpdateOnCharacterChange
{
    [SerializeField] private TMP_Text _charNameText;
    [SerializeField] private TMP_Text _raceText;
    [SerializeField] private TMP_Text _classText;
    [SerializeField] private TMP_Text _racialAbilityText;
    [SerializeField] private TMP_Text _goldValueText;

    [SerializeField] private Image _characterIcon;
    [SerializeField] private Image _raceImage;
    [SerializeField] private Image _classImage;
    [SerializeField] private List<Image> _displayedClassImages;

    [SerializeField] private Color _selectedClassColor = Color.blue;
    [SerializeField] private Color _defaultClassColor = Color.white;

    #region Character Attributes

    [Header("Attribute Text")]
    [SerializeField] private TMP_Text _hpValueText;
    [SerializeField] private TMP_Text _strValueText;
    [SerializeField] private TMP_Text _intValueText;
    [SerializeField] private TMP_Text _wisValueText;
    [SerializeField] private TMP_Text _dexValueText;
    [SerializeField] private TMP_Text _conValueText;
    [SerializeField] private TMP_Text _chaValueText;

    [Header("Modifier Text")]
    [SerializeField] private TMP_Text _hpModifierText;
    [SerializeField] private TMP_Text _strModifierText;
    [SerializeField] private TMP_Text _intModifierText;
    [SerializeField] private TMP_Text _wisModifierText;
    [SerializeField] private TMP_Text _dexModifierText;
    [SerializeField] private TMP_Text _conModifierText;
    [SerializeField] private TMP_Text _chaModifierText;

    [Header("Totals")]
    [SerializeField] private TMP_Text _hpTotalText;
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
        _charNameText.text = data.Name;
        _goldValueText.text = data.Gold.ToString();
        _raceImage.sprite = data.Race.Icon;
        _classImage.sprite = data.Class.Icon;
        _characterIcon.sprite = data.Icon;
        
        int CON = data.Attributes[4].Value;

        _racialAbilityText.text = data.Race.RacialAbility != null ? data.Race.RacialAbility.name : "None";

        _hpValueText.text = (data.HitPoints - data.Attributes[4].Mods.GetMod(CON)).ToString();
        _strValueText.text = data.Attributes[0].Value.ToString();
        _intValueText.text = data.Attributes[1].Value.ToString();
        _wisValueText.text = data.Attributes[2].Value.ToString();
        _dexValueText.text = data.Attributes[3].Value.ToString();
        _conValueText.text = data.Attributes[4].Value.ToString();
        _chaValueText.text = data.Attributes[5].Value.ToString();

        _hpModifierText.text = data.Attributes[4].Mods.GetMod(CON) != 0
            ? data.Attributes[4].Mods.GetMod(CON).ToString() : "";
        _strModifierText.text = GetModifierString(data, 0);
        _intModifierText.text = GetModifierString(data, 1);
        _wisModifierText.text = GetModifierString(data, 2);
        _dexModifierText.text = GetModifierString(data, 3);
        _conModifierText.text = GetModifierString(data, 4);
        _chaModifierText.text = GetModifierString(data, 5);
        
        _hpTotalText.text = (data.HitPoints).ToString();
        _strTotalText.text = (data.Attributes[0].Value + GetModifierInt(data, 0)).ToString();
        _intTotalText.text = (data.Attributes[1].Value + GetModifierInt(data, 1)).ToString();
        _wisTotalText.text = (data.Attributes[2].Value + GetModifierInt(data, 2)).ToString();
        _dexTotalText.text = (data.Attributes[3].Value + GetModifierInt(data, 3)).ToString();
        _conTotalText.text = (data.Attributes[4].Value + GetModifierInt(data, 4)).ToString();
        _chaTotalText.text = (data.Attributes[5].Value + GetModifierInt(data, 5)).ToString();
    }

    private string GetModifierString(CharacterData data, int modifierIndex)
    {
        string result = string.Empty;
        for (int i = 0; i < data.Race.StatModifiers.Count; i++)
        {
            if (data.Race.StatModifiers[i].Stat == data.Attributes[modifierIndex].AttributeType)
            {
                result = data.Race.StatModifiers[i].ModValue.ToString();
                return result;
            }
            else
            {
                result = "";
            }
        }

        return result;
    }

    private int GetModifierInt(CharacterData data, int modifierIndex)
    {
        int result = 0;
        for (int i = 0; i < data.Race.StatModifiers.Count; i++)
        {
            if (data.Race.StatModifiers[i].Stat == data.Attributes[modifierIndex].AttributeType)
            {
                result = data.Race.StatModifiers[i].ModValue;
                return result;
            }
            else
            {
                result = 0;
            }
        }

        return result;
    }
}