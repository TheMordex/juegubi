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

    public abstract void ApplyEffect(CharacterModel target);

    public void Tick()
    {
        Duration--;
    }

    public bool IsExpired() => Duration <= 0;
}