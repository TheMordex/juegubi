using UnityEngine;

public class StunEffect : StatusEffect
{
    public StunEffect(int duration) : base("Stun", duration) { }

    public override void ApplyEffect(Character target)
    {
        // No hace daño ni cura, solo evita que el personaje actúe
        target.IsStunned = true;
        Debug.Log($"{target.Name} está aturdido y pierde su turno.");
    }
}