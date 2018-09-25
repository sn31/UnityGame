

public class StatModifer
{
    public readonly float HealthValue;
    public readonly float MovementRadiusValue;
    public readonly float ArmorValue;
    public readonly float ActionPointValue;

    public StatModifer(float healthValue, float movementRadiusValue, float armorValue, float actionPointValue)
    {
        HealthValue = healthValue;
        MovementRadiusValue = movementRadiusValue;
        ArmorValue = armorValue;
        ActionPointValue = actionPointValue;
    }
}


//StatModifier MoveModifier = new StatModifier(0, 0, 0, -1);
//StatModifier AttackedModifier = new StatModifier(-5.0f, 0, 0, 0);

//Knight moves 2 steps and gets attacked.

CharacterStat Knight = new CharacterStat(100.00f, 30.00f, 25.00f, 2);

Knight.AddModifier(MoveModifer);
Knight.AddModifier(MoveModifer);
Knight.AddModifier(AttackedModifer);

statModifiers = {MoveModifier, MoveModfier, AttackedModifier}
//i = 0 - Moved once
float finalHealth = 100;
float finalMovementRadius = 30;
float finalArmor = 25;
float finalActionPoint = 1;

//i = 1 - Moved twice
float finalHealth = 100;
float finalMovementRadius = 30;
float finalArmor = 25;
float finalActionPoint = 0;

//i = 2 - Gets attacked
float finalHealth = 95;
float finalMovementRadius = 30;
float finalArmor = 25;
float finalActionPoint = 0;

List<float> finalTurnStats = Knight.CalculateFinalValue();
finalTurnStats = {95;30;25;0}