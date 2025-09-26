using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [Header("Recursos del jugador")]
    public int gold = 0;
    public int gems = 0;

    [Header("Config de ganancia (modificable por Remote Config)")]
    public int goldPerWin = 50;
    public int gemsPerWin = 1;

    void Awake()
    {
        // Singleton → se mantiene único entre escenas
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Agregar recursos
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"Ganaste {amount} oro. Total = {gold}");
    }

    public void AddGems(int amount)
    {
        gems += amount;
        Debug.Log($"Ganaste {amount} gemas. Total = {gems}");
    }

    // Gastar recursos
    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            Debug.Log($"Gastaste {amount} oro. Total = {gold}");
            return true;
        }
        Debug.Log("No hay suficiente oro");
        return false;
    }

    public bool SpendGems(int amount)
    {
        if (gems >= amount)
        {
            gems -= amount;
            Debug.Log($"Gastaste {amount} gemas. Total = {gems}");
            return true;
        }
        Debug.Log("No hay suficientes gemas");
        return false;
    }
}