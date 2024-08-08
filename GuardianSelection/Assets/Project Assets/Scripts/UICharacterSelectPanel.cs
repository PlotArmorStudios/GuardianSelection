using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterSelectPanel : MonoBehaviour, IUpdateCharacters
{
    public event Action<CharacterData> OnUpdateCharacters;
    public CharacterSelectManager SelectManager { get; set; }

    [SerializeField] private TMP_Text _characterName;
    [SerializeField] private Button _randomizeButton;

    private List<ISendCharacterData> _characterWidgets = new();

    private void OnEnable()
    {
        _randomizeButton.onClick.AddListener(() =>
        {
            foreach (KeyValuePair<Character,CharacterData> pair in SelectManager.CharacterDataManager.CharacterDatas)
            {
                SelectManager.CharacterDataManager.RandomizeCharacter(pair.Value);
                HandleCharacterUpdated(SelectManager.CharacterDataManager.CharacterDataList[0]);
            }
        });
    }

    private void OnDisable()
    {
        _randomizeButton.onClick.RemoveAllListeners();
    }
    
    public void Initialize()
    {
        _characterWidgets = GetComponentsInChildren<ISendCharacterData>().ToList();

        for (int i = 0; i < _characterWidgets.Count; i++)
        {
            _characterWidgets[i].OnSendData += HandleCharacterUpdated;
            _characterWidgets[i].SetCharacter(SelectManager.CharacterDataManager.CharacterDataList[i]);
        }
        
        HandleCharacterUpdated(SelectManager.CharacterDataManager.CharacterDataList[0]);
    }

    private void HandleCharacterUpdated(CharacterData data)
    {
        OnUpdateCharacters?.Invoke(data);
        _characterName.text = data.Name;
    }
}