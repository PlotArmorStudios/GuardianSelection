using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public interface ISendCharacterData
{
    public event Action<CharacterData> OnSendData; 
    public CharacterData AssignedCharacter { get; set; }
    public void SetCharacter(CharacterData data);
}
public class UICharacterWidget : MonoBehaviour, IPointerClickHandler, ISendCharacterData
{
    [SerializeField] private Image _characterImage;

    public event Action<CharacterData> OnSendData;
    public CharacterData AssignedCharacter { get; set; }
    
    public void SetCharacter(CharacterData character)
    {
        AssignedCharacter = character;
        _characterImage.sprite = character.Icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSendData?.Invoke(AssignedCharacter);
    }
}