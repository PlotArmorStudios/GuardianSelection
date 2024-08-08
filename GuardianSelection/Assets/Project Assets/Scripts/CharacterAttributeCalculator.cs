public class CharacterAttributeCalculator
{
    /// <summary>
    /// Randomly set the values of each of a character's stats.
    /// </summary>
    /// <param name="data"></param>
    public void CalculateAttributes(CharacterData data)
    {
        foreach (CharacterAttributeData attribute in data.Attributes)
        {
            attribute.Value = GetDiceRollValue(data.AttributeDiceRoll.Number, data.AttributeDiceRoll.DiceSides);
            VerifyAttribute(attribute, data);
        }
    }

    /// <summary>
    /// Ensures stats meet their minimum stat requirements
    /// after stat randomization.
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="data"></param>
    private void VerifyAttribute(CharacterAttributeData attribute, CharacterData data)
    {
        if (attribute.Value < data.Race.GetStatMinimumStat(attribute))
        {
            attribute.Value = data.Race.GetStatMinimumStat(attribute);
        }
    }

    public void CalculateHitPoints(CharacterData data)
    {
        data.HitPoints = GetDiceRollValue(data.Class.HitDice.Number, data.Class.HitDice.DiceSides);

        int CON = data.Attributes[4].Value;
        data.HitPoints += data.Attributes[4].Mods.GetMod(CON);
        if (data.HitPoints < 1)
            data.HitPoints = 1;
    }

    /// <summary>
    /// Sets the class of a character randomly
    /// based on the classes available to that race.
    /// </summary>
    /// <param name="data"> The character of whom to generate the class. </param>
    public void GenerateClass(CharacterData data)
    {
        data.Class = GenerateApplicableClass(data.Race);
    }

    public void CalculateGold(CharacterData data)
    {
        data.Gold = GetDiceRollValue(data.GoldDiceRoll.Number, data.GoldDiceRoll.DiceSides);
    }

    private ClassType GenerateApplicableClass(Race dataRace)
    {
        int random = UnityEngine.Random.Range(0, dataRace.AllowedClasses.Count);
        return dataRace.AllowedClasses[random];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="number"> The number of times to roll the dice. </param>
    /// <param name="diceSides"> The number of sides of the dice. </param>
    /// <returns></returns>
    public int GetDiceRollValue(int number, int diceSides)
    {
        int value = 0;
        for (int i = 0; i < number; i++)
        {
            value += UnityEngine.Random.Range(1, diceSides + 1);
        }

        return value;
    }
}