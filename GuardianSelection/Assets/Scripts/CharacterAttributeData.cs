public class CharacterAttributeData
{
    public CharacterAttribute AttributeType;
    public string AttributeName;
    public int Value;
    public AttributeMod Mods;

    public CharacterAttributeData(CharacterAttribute characterAttribute)
    {
        AttributeName = characterAttribute.name;
        AttributeType = characterAttribute;
        Value = characterAttribute.Value;
        Mods = characterAttribute.Mods;
    }
}