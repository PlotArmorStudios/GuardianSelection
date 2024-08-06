using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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