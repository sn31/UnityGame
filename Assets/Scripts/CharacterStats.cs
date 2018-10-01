using System.Collections.Generic;

public class CharacterStat
{
    public float BaseHealth; //Character's base health
    public float BaseMovementRadius; //Character's maximum movement radius
    public float BaseArmor; //Character's base armor
    public float BaseActionPoint = 2; //Character's maximum number of actions - Usually 2 points but Ryan said have this just in case...
    public bool TurnAvailability = true;
    public bool IsDead = false;

    private readonly List<StatModifer> statModifers; //List of different modifiers added to the characters, which will affect one or more base values.

    public CharacterStat(float baseHealth, float baseMovementRadius, float baseArmor, float baseActionPoint)
    {
        BaseHealth = baseHealth;
        BaseMovementRadius = baseMovementRadius;
        BaseArmor = baseArmor;
        BaseActionPoint = baseActionPoint;
        statModifers = new List<StatModifer>();
    }

    public void AddModifier(StatModifer mod)
    {
        statModifers.Add(mod);
    }
	public bool AddModifier(StatModifer mod)
	{
		statModifers.Remove(mod);
	}

    private List<float> CalculateFinalValue()
    {
        float finalHealth = BaseHealth;
        float finalMovementRadius = BaseMovementRadius;
        float finalArmor = BaseArmor;
        float finalActionPoint = BaseActionPoint;
        for (int i = 0; i < statModifers.Count; i++)
        {
            finalHealth += statModifers[i].HealthValue;
            if (finalHealth <=0)
            {
                IsDead = true;
            }
            finalMovementRadius += statModifers[i].MovementRadiusValue;
            finalArmor += statModifers[i].ArmorValue;
            finalActionPoint += statModifers[i].ActionPointValue;

            if (finalActionPoint == 0)
            {
                TurnAvailability = false;
                finalActionPoint = 2;
            }



            if (finalActionPoint = 1 && statModifers[i].ActionPointValue == -1 ) //If action point is going to be zero, just reset it to maximum action point of 2.
            {
                TurnAvailability = false;//Change to other player somehow in here...
				finalActionPoint = 2; 
			}
            else
            {
                finalActionPoint += statModifers[i].ActionPointValue;
			}
			
		}

    }
	public CharacterStat EndTurn()
	{
		BaseActionPointValue = 0;
	}

}



