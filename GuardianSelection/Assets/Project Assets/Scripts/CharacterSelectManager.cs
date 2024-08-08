using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IUpdateOnCharacterChange
{
    public CharacterSelectManager SelectManager { get; set; }
    public void HandleCharacterChange(CharacterData data);
}

public interface IInitializeAfterLoad
{
    public void Initialize();
}

public interface IUpdateCharacters
{
    public event Action<CharacterData> OnUpdateCharacters;
    public CharacterSelectManager SelectManager { get; set; }
    public void Initialize();
}

public class CharacterSelectManager : MonoBehaviour
{
    public event Action<CharacterData> OnCharacterChange;
    public CharacterDataManager CharacterDataManager { get; set; }
    private CharacterData _currentSelectedCharacter;
    private List<IUpdateOnCharacterChange> _changeListeners;
    private List<IInitializeAfterLoad> _toBeInitialized;
    private List<IUpdateCharacters> _characterUpdaters;

    private void Awake()
    {
        CharacterDataManager = FindObjectOfType<CharacterDataManager>();
        CharacterDataManager.OnDataLoaded += Initialize;
    }
    
    private void Initialize()
    {
        _changeListeners = GetComponentsInChildren<IUpdateOnCharacterChange>().ToList();
        _characterUpdaters = GetComponentsInChildren<IUpdateCharacters>().ToList();
        _toBeInitialized = GetComponentsInChildren<IInitializeAfterLoad>().ToList();
        
        //Subscribe objects that need to update to OnCharacterChange
        //So they can respond after character updaters send off their OnUpdateCharacters event
        foreach (IUpdateOnCharacterChange changeListener in _changeListeners)
        {
            OnCharacterChange += changeListener.HandleCharacterChange;
            changeListener.SelectManager = this;
        }
        
        foreach (IInitializeAfterLoad initializeAfterLoad in _toBeInitialized)
        {
            initializeAfterLoad.Initialize();
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