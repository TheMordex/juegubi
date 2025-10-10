using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : ICommand
{
    private CharacterController attacker;
    private CharacterController target;
    private int damage;

    public AttackCommand(CharacterController attacker, CharacterController target, int damage)
    {
        this.attacker = attacker;
        this.target = target;
        this.damage = damage;
    }

    public void Execute()
    {
        attacker.Attack(target);
        Debug.Log($"{attacker.GetName()} ataca a {target.GetName()} causando {damage} de daño.");
    }

    public void Undo()
    {
        target.model.Heal(damage);
        target.UpdateView();
        Debug.Log($"Se deshizo el ataque: {target.GetName()} recupera {damage} de vida.");
    }
}

public class DefendCommand : ICommand
{
    private CharacterController defender;

    public DefendCommand(CharacterController defender)
    {
        this.defender = defender;
    }

    public void Execute()
    {
        defender.model.DefenseBonus = 20; 
        Debug.Log($"{defender.GetName()} se pone en guardia (+20 defensa temporal).");
    }

    public void Undo()
    {
        defender.model.DefenseBonus = 0;
        Debug.Log($"{defender.GetName()} deja de defenderse.");
    }
}

public class HealCommand : ICommand
{
    private CharacterController healer;
    private int healAmount;

    public HealCommand(CharacterController healer, int healAmount)
    {
        this.healer = healer;
        this.healAmount = healAmount;
    }

    public void Execute()
    {
        healer.Heal(healAmount);
        Debug.Log($"{healer.GetName()} se cura {healAmount} de vida.");
    }

    public void Undo()
    {
        healer.model.TakeDamage(healAmount);
        healer.UpdateView();
        Debug.Log($"Se deshizo la curación de {healer.GetName()}.");
    }
}
