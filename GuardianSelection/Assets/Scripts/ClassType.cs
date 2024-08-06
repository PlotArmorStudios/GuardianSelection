using UnityEngine;

[CreateAssetMenu(fileName = "Character Data/Attributes", menuName = "Class")]
public class ClassType : ScriptableObject
{
    public Sprite Icon;
    public DiceRoll HitDice;
    public CharacterAttribute PrimeStat;
}