using UnityEngine;

public class FortifyEffect : StatusEffect
{
    private int defenseBoost;

    public FortifyEffect(int duration, int boost) : base("Fortify", duration)
    {
        defenseBoost = boost;
    }

    public override void ApplyEffect(Character target)
    {
        target.DefenseBonus = defenseBoost;
        Debug.Log($"{target.Name} aumenta su defensa en {defenseBoost} por {Duration} turnos.");
    }
}