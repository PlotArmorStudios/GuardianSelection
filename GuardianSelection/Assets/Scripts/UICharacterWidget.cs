using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterWidget : MonoBehaviour
{
    [SerializeField] private TMP_Text _characterName;
    [SerializeField] private Image _characterImage;

    public void SetCharacter(Character character)
    {
        _characterImage.sprite = character.Icon;
    }
}