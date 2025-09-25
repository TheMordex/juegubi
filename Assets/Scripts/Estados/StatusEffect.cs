using UnityEngine;

public abstract class StatusEffect
{
    public string Name { get; private set; }
    public int Duration { get; private set; }

    public StatusEffect(string name, int duration)
    {
        Name = name;
        Duration = duration;
    }

    // Se ejecuta cada turno
    public abstract void ApplyEffect(Character target);

    // Reducir duración en 1 turno
    public void Tick()
    {
        Duration--;
    }

    public bool IsExpired()
    {
        return Duration <= 0;
    }
}