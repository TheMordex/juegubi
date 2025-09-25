using UnityEngine;
using System.Collections.Generic;

public class Character
{
    public string Name { get; private set; }
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }
    public bool IsDefending { get; set; }
    public int DefenseBonus { get; set; } = 0;
    public bool IsStunned { get; set; } = false;
    public List<StatusEffect> ActiveEffects { get; private set; }

    // Eventos para conectar con la parte visual
    public event System.Action OnDamagedEvent;
    public event System.Action OnHealedEvent;
    public event System.Action OnStatusChanged;   // nuevo evento para cambios de estado

    public Character(string name, int health)
    {
        Name = name;
        MaxHealth = health;
        Health = health;
        IsDefending = false;
        ActiveEffects = new List<StatusEffect>();
    }

    public void TakeDamage(int amount)
    {
        if (IsDefending)
        {
            amount /= 2;
            IsDefending = false;
        }

        if (DefenseBonus > 0)
        {
            amount -= DefenseBonus;
            if (amount < 0) amount = 0;
        }

        Health -= amount;
        if (Health < 0) Health = 0;

        Debug.Log($"{Name} recibe {amount} de daño. Vida restante: {Health}");

        // Avisar que recibió daño
        OnDamagedEvent?.Invoke();
    }

    public void Heal(int amount)
    {
        Health += amount;
        if (Health > MaxHealth) Health = MaxHealth;

        Debug.Log($"{Name} se cura {amount} de vida. Ahora tiene {Health}.");

        // Avisar que se curó
        OnHealedEvent?.Invoke();
    }

    public void ApplyStatus(StatusEffect effect)
    {
        ActiveEffects.Add(effect);
        Debug.Log($"{Name} gana estado: {effect.Name} por {effect.Duration} turnos.");

        // Avisar que cambió su lista de estados
        OnStatusChanged?.Invoke();
    }

    public void UpdateEffects()
    {
        for (int i = ActiveEffects.Count - 1; i >= 0; i--)
        {
            ActiveEffects[i].ApplyEffect(this);
            ActiveEffects[i].Tick();

            if (ActiveEffects[i].IsExpired())
            {
                if (ActiveEffects[i] is StunEffect)
                    IsStunned = false;

                Debug.Log($"{Name} ya no tiene el estado {ActiveEffects[i].Name}.");
                ActiveEffects.RemoveAt(i);

                // Avisar que se quitó un estado
                OnStatusChanged?.Invoke();
            }
        }
    }
}
