using UnityEngine;

public class StunEffect : StatusEffect
{
    public StunEffect(int duration) : base("Stun", duration) { }

    public override void ApplyEffect(CharacterModel target)
    {
        target.IsStunned = true;
        Debug.Log($"{target.Name} está aturdido y pierde su turno.");
    }
}