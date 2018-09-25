using System.Collections.Generic;

public class CharacterStat
{
    public float BaseValue;

    private readonly List<StatModifer> statModifers;

    public CharacterStat(float baseValue)
    {
        BaseValue = baseValue;
        statModifers = new List<StatModifer>();
    }
}