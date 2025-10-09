using System;
using UnityEngine;

public static class EventManager
{
    public static event Action<Character, int> OnHealthChanged;
    public static event Action<Character> OnTurnStarted;
    public static event Action<Character> OnTurnEnded;
    public static event Action<string> OnBattleEnded;

    public static void HealthChanged(Character character, int newHealth)
    {
        OnHealthChanged?.Invoke(character, newHealth);
    }

    public static void TurnStarted(Character character)
    {
        OnTurnStarted?.Invoke(character);
    }

    public static void TurnEnded(Character character)
    {
        OnTurnEnded?.Invoke(character);
    }

    public static void BattleEnded(string result)
    {
        OnBattleEnded?.Invoke(result);
    }
}

