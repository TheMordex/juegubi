using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string Name { get; private set; }
    public int Health { get; private set; }
    public bool IsDefending { get; set; }

    public Character(string name, int health)
    {
        Name = name;
        Health = health;
        IsDefending = false;
    }

    public void TakeDamage(int amount)
    {
        if (IsDefending)
        {
            amount /= 2;
            IsDefending = false; // solo defiende un turno
        }

        Health -= amount;
        if (Health < 0) Health = 0;

        Debug.Log($"{Name} recibe {amount} de daño. Vida restante: {Health}");
    }

    public void Heal(int amount)
    {
        Health += amount;
        Debug.Log($"{Name} se cura {amount} de vida. Ahora tiene {Health}.");
    }
}