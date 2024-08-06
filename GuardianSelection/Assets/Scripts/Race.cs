using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data/Attributes", menuName = "Race")]
public class Race : ScriptableObject
{
    public struct StatModifier
    {
        public CharacterAttribute Stat;
        public int ModValue;
    }

    public List<StatModifier> MinimumStats;
    public List<StatModifier> StatModifiers;
    public RacialAbility RacialAbility;
    public List<ClassType> AllowedClasses;

    public int GetStatMinimumStat(CharacterAttributeData attribute)
    {
        StatModifier mod = MinimumStats.FirstOrDefault(x => x.Stat == attribute.AttributeType);
        return mod.ModValue;
    }
}