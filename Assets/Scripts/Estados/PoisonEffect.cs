using UnityEngine;

public class PoisonEffect : StatusEffect
{
    private int damagePerTurn;

    public PoisonEffect(int duration, int damage) : base("Veneno", duration)
    {
        damagePerTurn = damage;
    }

    public override void ApplyEffect(Character target)
    {
        target.TakeDamage(damagePerTurn);
        Debug.Log($"{target.Name} recibe {damagePerTurn} de daño por veneno.");
    }
}