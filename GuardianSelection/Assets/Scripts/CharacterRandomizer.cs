public class CharacterRandomizer
{
    private CharacterAttributeCalculator _calc;
    public CharacterRandomizer()
    {
        _calc = new CharacterAttributeCalculator();
    }
    public void RandomizeCharacter(CharacterData newCharacter)
    {
        _calc.CalculateAttributes(newCharacter);
        _calc.GenerateClass(newCharacter);
        _calc.CalculateHitPoints(newCharacter);
        _calc.CalculateGold(newCharacter);
    }
}