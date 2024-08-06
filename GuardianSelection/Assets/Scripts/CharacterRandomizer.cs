public class CharacterRandomizer
{
    private CharacterAttributeCalculator _calc;

    public void RandomizeCharacter(CharacterData newCharacter)
    {
        _calc.CalculateAttributes(newCharacter);
        _calc.CalculateHitPoints(newCharacter);
        _calc.GenerateClass(newCharacter);
        _calc.CalculateGold(newCharacter);
    }
}