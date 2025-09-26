using UnityEngine;
using System.Collections.Generic;

public class CharacterBehaviour : MonoBehaviour
{
    [Header("Stats base")]
    public string characterName = "Personaje";
    public int maxHealth = 100;

    [Header("Visual 3D")]
    public Renderer meshRenderer; // arrastrá el Renderer del cubo

    public Character Data { get; private set; }

    private Color originalColor;

    // Tabla de colores por estado
    private Dictionary<string, Color> stateColors = new Dictionary<string, Color>()
    {
        { "Stun", Color.blue },       // aturdido
        { "Veneno", Color.magenta },    // envenenado
        { "Fortify", Color.cyan }   // defensa mejorada
    };

    // Lista de prioridades (el primero tiene más prioridad)
    private List<string> priority = new List<string> { "Stun", "Veneno", "Fortify" };

    void Awake()
    {
        // Crear el personaje lógico
        Data = new Character(characterName, maxHealth);

        // Guardar color original
        if (meshRenderer != null)
            originalColor = meshRenderer.material.color;

        // Suscribirse a los eventos
        Data.OnDamagedEvent += OnDamaged;
        Data.OnHealedEvent += OnHealed;
        Data.OnStatusChanged += CheckStatusColor;
    }

    void OnDamaged()
    {
        if (meshRenderer != null)
            StartCoroutine(FlashColor(Color.red));
    }

    void OnHealed()
    {
        if (meshRenderer != null)
            StartCoroutine(FlashColor(Color.green));
    }

    private System.Collections.IEnumerator FlashColor(Color flashColor)
    {
        if (meshRenderer == null) yield break;

        // Guardar color actual (el que tiene por estado)
        Color beforeFlash = meshRenderer.material.color;

        // Flash
        meshRenderer.material.color = flashColor;
        yield return new WaitForSeconds(0.2f);

        // Volver al color correspondiente por estado
        meshRenderer.material.color = beforeFlash;
    }

    /// <summary>
    /// Cambia el color según el estado actual con tabla de colores y prioridad
    /// </summary>
    void CheckStatusColor()
    {
        if (meshRenderer == null) return;

        Color targetColor = originalColor;

        // Revisar la lista de prioridades
        foreach (string state in priority)
        {
            foreach (var effect in Data.ActiveEffects)
            {
                if (effect.Name == state && stateColors.ContainsKey(state))
                {
                    targetColor = stateColors[state];
                    meshRenderer.material.color = targetColor;
                    return; // se encontró el estado de mayor prioridad
                }
            }
        }

        // Si no hay estados activos relevantes
        meshRenderer.material.color = targetColor;
    }
    
    public void InitializeCharacter(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        Data = new Character(characterName, maxHealth);

        // (re-suscribirse a eventos)
        Data.OnDamagedEvent += OnDamaged;
        Data.OnHealedEvent += OnHealed;
        Data.OnStatusChanged += CheckStatusColor;
        // actualizar UI, etc.
    }
}
