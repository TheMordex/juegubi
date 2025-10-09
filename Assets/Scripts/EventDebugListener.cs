using UnityEngine;

public class EventDebugListener : MonoBehaviour
{
    void OnEnable()
    {
        EventManager.OnTurnStarted += OnTurnStarted;
        EventManager.OnTurnEnded += OnTurnEnded;
        EventManager.OnHealthChanged += OnHealthChanged;
        EventManager.OnBattleEnded += OnBattleEnded;
    }

    void OnDisable()
    {
        EventManager.OnTurnStarted -= OnTurnStarted;
        EventManager.OnTurnEnded -= OnTurnEnded;
        EventManager.OnHealthChanged -= OnHealthChanged;
        EventManager.OnBattleEnded -= OnBattleEnded;
    }

    void OnTurnStarted(Character c)
    {
        Debug.Log($"🟢 Comienza el turno de {c.Name}");
    }

    void OnTurnEnded(Character c)
    {
        Debug.Log($"🔴 Termina el turno de {c.Name}");
    }

    void OnHealthChanged(Character c, int newHealth)
    {
        Debug.Log($"❤️ {c.Name} tiene ahora {newHealth} de vida");
    }

    void OnBattleEnded(string result)
    {
        Debug.Log($"🏁 Fin del combate: {result}");
    }
}