using UnityEngine;

public static class StatusEffectFactory
{
    public static StatusEffect CreateEffect(string type)
    {
        switch (type)
        {
            case "Poised":
                // duración, daño por turno
                return new PoisonEffect(3, 5);

            case "Stun":
                // duración
                return new StunEffect(2);

            case "Fortify":
                // duración, bonificación de defensa
                return new FortifyEffect(2, 10);

            default:
                return null;
        }
    }
}
