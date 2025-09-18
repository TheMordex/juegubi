using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : ICommand
{
    private Character attacker;
    private Character target;
    private int damage;

    public AttackCommand(Character attacker, Character target, int damage)
    {
        this.attacker = attacker;
        this.target = target;
        this.damage = damage;
    }

    public void Execute()
    {
        target.TakeDamage(damage);
        Debug.Log($"{attacker.Name} ataca a {target.Name} causando {damage} de daño.");
    }

    public void Undo()
    {
        target.Heal(damage);
        Debug.Log($"Se deshizo el ataque: {target.Name} recupera {damage} de vida.");
    }
}

public class DefendCommand : ICommand
{
    private Character defender;

    public DefendCommand(Character defender)
    {
        this.defender = defender;
    }

    public void Execute()
    {
        defender.IsDefending = true;
        Debug.Log($"{defender.Name} se pone en guardia.");
    }

    public void Undo()
    {
        defender.IsDefending = false;
        Debug.Log($"{defender.Name} deja de defenderse.");
    }
}

public class HealCommand : ICommand
{
    private Character healer;
    private int healAmount;

    public HealCommand(Character healer, int healAmount)
    {
        this.healer = healer;
        this.healAmount = healAmount;
    }

    public void Execute()
    {
        healer.Heal(healAmount);
        Debug.Log($"{healer.Name} se cura {healAmount} de vida.");
    }

    public void Undo()
    {
        healer.TakeDamage(healAmount);
        Debug.Log($"Se deshizo la curación de {healer.Name}.");
    }
}
