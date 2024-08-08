/// <summary>
/// Data transfer object for attribute
/// scriptable objects.
/// </summary>
public class CharacterAttributeData
{
    public CharacterAttribute AttributeType;
    public int Value;
    public AttributeMod Mods;

    public CharacterAttributeData(CharacterAttribute characterAttribute)
    {
        AttributeType = characterAttribute;
        Value = characterAttribute.Value;
        Mods = characterAttribute.Mods;
    }
}