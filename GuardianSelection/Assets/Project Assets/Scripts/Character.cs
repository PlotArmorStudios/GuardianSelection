using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data/Character", menuName = "Character")]
public class Character : ScriptableObject
{
    [Header("Attributes")]
    public List<CharacterAttribute> Attributes;

    public DiceRoll AttributeDiceRoll;
    public DiceRoll HitPointsDiceRoll;
    public DiceRoll GoldDiceRoll;
    public Race Race;
    public Sprite Icon;
    public GameObject Model;
}