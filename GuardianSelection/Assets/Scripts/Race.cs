using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data/Attributes", menuName = "Race")]
public class Race : ScriptableObject
{
    [Serializable]
    public struct StatModifier
    {
        public CharacterAttribute Stat;
        public int ModValue;
    }

    public Sprite Icon;
    public List<StatModifier> MinimumStats;
    public List<StatModifier> StatModifiers;
    public List<ClassType> AllowedClasses;
    public RacialAbility RacialAbility;

    public int GetStatMinimumStat(CharacterAttributeData attribute)
    {
        StatModifier mod = MinimumStats.FirstOrDefault(x => x.Stat == attribute.AttributeType);
        return mod.ModValue;
    }
}