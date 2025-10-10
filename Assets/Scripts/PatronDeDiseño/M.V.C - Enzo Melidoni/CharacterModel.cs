using System.Collections.Generic;
using UnityEngine;

public class CharacterModel
{
    private List<StatusEffect> activeEffects = new List<StatusEffect>();

    public string Name;
    public int MaxHealth;
    public int CurrentHealth;
    public int AttackPower;
    public int DefenseBonus = 0;
    public bool IsStunned = false;

    public CharacterModel(string name, int maxHealth, int attackPower)
    {
        Name = name;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        AttackPower = attackPower;
    }

    public void TakeDamage(int amount)
    {
        int totalDamage = Mathf.Max(amount - DefenseBonus, 0);
        CurrentHealth -= totalDamage;
        if (CurrentHealth < 0)
            CurrentHealth = 0;
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    public void AddEffect(StatusEffect effect)
    {
        if (effect == null) return;

        activeEffects.Add(effect);
        Debug.Log($"{Name} ha recibido el efecto: {effect.Name}");

        // Algunos efectos se aplican instantáneamente
        effect.ApplyEffect(this);
    }

    public void UpdateStatusEffects()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = activeEffects[i];
            effect.ApplyEffect(this);
            effect.Tick();

            if (effect.IsExpired())
            {
                Debug.Log($"{Name} ya no tiene el efecto {effect.Name}.");

                if (effect is StunEffect)
                    IsStunned = false;
                if (effect is FortifyEffect)
                    DefenseBonus = 0;

                activeEffects.RemoveAt(i);
            }
        }
    }

    public bool IsDead() => CurrentHealth <= 0;
}