using System;
using System.Collections.Generic;

/// <summary>
/// An set of values representing the min and max of an attribute
/// when finding what mod to add to an attribute.
/// Based on an attribute's value, this object can return the desired mod.
/// </summary>
[Serializable]
public struct AttributeMod
{
    [Serializable]
    public struct AttributeModRange
    {
        public int MinValue;
        public int MaxValue;
        public int Mod;
    }

    public List<AttributeModRange> ModRanges;

    public int GetMod(int value)
    {
        int outOfRangeResult = 0;
        if (value == 0)
            value++;

        foreach (AttributeModRange modRange in ModRanges)
        {
            if (value >= modRange.MinValue && value <= modRange.MaxValue)
                return modRange.Mod;
            outOfRangeResult = modRange.Mod;
        }

        return outOfRangeResult;
    }
}