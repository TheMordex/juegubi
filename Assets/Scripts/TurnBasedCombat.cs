using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;  
using TMPro;           
using UnityEngine;

public class TurnBasedCombat : MonoBehaviour
{
    private Character hero;
    private Character enemy;
    private TurnManager turnManager;

    private bool heroTurn = true;

    [Header("UI")]
    public Button attackButton;
    public Button defendButton;
    public Button healButton;
    public TextMeshProUGUI statusText;  

    void Start()
    {
        hero = new Character("Héroe", 100);
        enemy = new Character("Enemigo", 80);
        turnManager = new TurnManager();

        statusText.text = "¡Comienza la batalla!\nElige una acción para el Héroe.";
        
        attackButton.onClick.AddListener(() => OnHeroAction("attack"));
        defendButton.onClick.AddListener(() => OnHeroAction("defend"));
        healButton.onClick.AddListener(() => OnHeroAction("heal"));
    }

    void OnHeroAction(string action)
    {
        if (!heroTurn) return; // solo el héroe puede elegir

        switch (action)
        {
            case "attack":
                turnManager.AddCommand(new AttackCommand(hero, enemy, 20));
                break;
            case "defend":
                turnManager.AddCommand(new DefendCommand(hero));
                break;
            case "heal":
                turnManager.AddCommand(new HealCommand(hero, 15));
                break;
        }

        turnManager.ExecuteTurn();
        CheckBattleState();

        if (enemy.Health > 0) // si el enemigo sigue vivo, toma su turno
        {
            EnemyTurn();
        }
    }

    void EnemyTurn()
    {
        heroTurn = false;
        statusText.text = "Turno del Enemigo...";

        // alterna entre ataque y defensa al azar
        if (Random.value > 0.5f)
        {
            turnManager.AddCommand(new AttackCommand(enemy, hero, 15));
        }
        else
        {
            turnManager.AddCommand(new DefendCommand(enemy));
        }

        turnManager.ExecuteTurn();
        CheckBattleState();

        heroTurn = true;
        if (hero.Health > 0)
        {
            statusText.text = "Elige una acción para el Héroe.";
        }
    }

    void CheckBattleState()
    {
        if (hero.Health <= 0)
        {
            statusText.text = "¡El Héroe ha caído! Fin del combate.";
            DisableButtons();
        }
        else if (enemy.Health <= 0)
        {
            statusText.text = "¡El Enemigo ha sido derrotado!";
            DisableButtons();
        }
    }

    void DisableButtons()
    {
        attackButton.interactable = false;
        defendButton.interactable = false;
        healButton.interactable = false;
    }
}