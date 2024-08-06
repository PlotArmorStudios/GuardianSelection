using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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