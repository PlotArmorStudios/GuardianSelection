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

public class CharacterDataManager : MonoBehaviour
{
    [SerializeField] private List<Character> _characters;
    private Dictionary<Character, CharacterData> _characterDatas = new();
    private CharacterRandomizer _characterRandomizer;
    public int NumCharacters => _characters.Count;
    public Dictionary<Character, CharacterData> CharacterDatas => _characterDatas;
    public List<Character> Characters => _characters;

    private void Awake()
    {
        foreach (Character character in _characters)
        {
            CharacterData newCharacter = new CharacterData(character);
            _characterRandomizer.RandomizeCharacter(newCharacter);
            _characterDatas.Add(character, newCharacter);
        }
    }
}