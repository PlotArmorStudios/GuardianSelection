using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data transfer objects for Character
/// scriptable objects.
/// </summary>
public class CharacterData
{
    public List<CharacterAttributeData> Attributes = new();
    public string Name { get; set; }
    public Sprite Icon { get; set; }
    public DiceRoll AttributeDiceRoll { get; set; }
    public DiceRoll HitPointsDiceRoll { get; set; }
    public DiceRoll GoldDiceRoll { get; set; }
    public Race Race;
    public ClassType Class;
    public GameObject Model;
    public int HitPoints { get; set; }
    public int Gold { get; set; }

    public CharacterData(Character characterSO)
    {
        Name = characterSO.name;
        Icon = characterSO.Icon;

        foreach (CharacterAttribute characterAttribute in characterSO.Attributes)
        {
            CharacterAttributeData newAttributeData = new CharacterAttributeData(characterAttribute);
            Attributes.Add(newAttributeData);
        }

        AttributeDiceRoll = characterSO.AttributeDiceRoll;
        GoldDiceRoll = characterSO.GoldDiceRoll;
        Race = characterSO.Race;
        Model = characterSO.Model;
    }
}