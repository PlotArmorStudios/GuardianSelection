using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public struct DiceRoll
{
    public int Number;
    public int DiceSides;
}

/// <summary>
/// Initialize character data
/// to be accessed by other components.
/// </summary>
public class CharacterDataManager : MonoBehaviour
{
    public event Action OnDataLoaded;
    [SerializeField] private List<Character> _characters;
    private Dictionary<Character, CharacterData> _characterDatas = new();
    private CharacterRandomizer _characterRandomizer;
    public List<CharacterData> _characterDataList = new();

    public Dictionary<Character, CharacterData> CharacterDatas => _characterDatas;
    public List<CharacterData> CharacterDataList => _characterDataList;

    private void Awake()
    {
        _characterRandomizer = new CharacterRandomizer();
        foreach (Character character in _characters)
        {
            CharacterData newCharacter = new CharacterData(character);
            RandomizeCharacter(newCharacter);
            _characterDatas.Add(character, newCharacter);
            _characterDataList.Add(newCharacter);
        }
        
        OnDataLoaded?.Invoke();
    }
    
    public void RandomizeCharacter(CharacterData data)
    {
        _characterRandomizer.RandomizeCharacter(data);
    }
}