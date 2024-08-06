using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public interface IUpdateOnCharacterChange
{
    public UICharacterSelectManager SelectManager { get; set; }
    public void HandleCharacterChange(CharacterData data);
}

public interface IUpdateCharacters
{
    public event Action<CharacterData> OnUpdateCharacters;
    public UICharacterSelectManager SelectManager { get; set; }
    public void Initialize();
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
        CharacterDataManager.OnDataLoaded += Initialize;
    }
    
    private void Initialize()
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
            characterUpdater.Initialize();
        }
    }

    public void UpdateSelectedCharacter(CharacterData data)
    {
        OnCharacterChange?.Invoke(data);
    }
}