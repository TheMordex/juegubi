using System;
using UnityEngine;

public static class EventManager
{
    // Eventos principales
    public static event Action<CharacterModel, int> OnHealthChanged;
    public static event Action<CharacterModel> OnTurnStarted;
    public static event Action<CharacterModel> OnTurnEnded;
    public static event Action<string> OnBattleEnded;
    
    public static void HealthChanged(CharacterModel character, int newHealth)
    {
        OnHealthChanged?.Invoke(character, newHealth);
        Debug.Log($"Salud actualizada de {character.Name}: {newHealth}/{character.MaxHealth}");
    }

    public static void TurnStarted(CharacterModel character)
    {
        OnTurnStarted?.Invoke(character);
        Debug.Log($"Comienza el turno de {character.Name}");
    }

    public static void TurnEnded(CharacterModel character)
    {
        OnTurnEnded?.Invoke(character);
        Debug.Log($"Termina el turno de {character.Name}");
    }

    public static void BattleEnded(string result)
    {
        OnBattleEnded?.Invoke(result);
        Debug.Log($"[Event] Fin de batalla: {result}");
    }
}