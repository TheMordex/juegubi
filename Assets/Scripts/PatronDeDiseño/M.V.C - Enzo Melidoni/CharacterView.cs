using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CharacterView : MonoBehaviour
{
    private Slider healthBar;
    private TextMeshProUGUI healthText;

    [Header("Referencia visual del personaje")]
    public Transform characterVisual; 
    private Renderer meshRenderer;
    private Color originalColor;
    private MaterialPropertyBlock colorBlock;

    private Vector3 originalPos;

    [Header("Efectos visuales")]
    public float shakeDuration = 0.25f;
    public float shakeMagnitude = 0.15f;
    public Color damageFlashColor = Color.red;
    public float flashDuration = 0.15f;

    void Awake()
    {
        if (characterVisual == null)
            characterVisual = transform;
        
        meshRenderer = characterVisual.GetComponentInChildren<Renderer>();

        if (meshRenderer != null)
        {
            colorBlock = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(colorBlock);
            originalColor = meshRenderer.sharedMaterial.color;
        }

        originalPos = characterVisual.localPosition;
    }

    public void Setup(Slider bar, TextMeshProUGUI text)
    {
        healthBar = bar;
        healthText = text;
    }

    public void UpdateHealth(int current, int max)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = max;
            healthBar.value = current;
        }

        if (healthText != null)
            healthText.text = $"{current}/{max}";
    }
    
    public void PlayShake()
    {
        StopCoroutine(nameof(ShakeEffect));
        StartCoroutine(ShakeEffect());
    }

    private IEnumerator ShakeEffect()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            float z = Random.Range(-1f, 1f) * shakeMagnitude * 0.5f;

            characterVisual.localPosition = originalPos + new Vector3(x, y, z);
            
            float scale = 1f + Random.Range(-0.03f, 0.03f);
            characterVisual.localScale = Vector3.one * scale;

            elapsed += Time.deltaTime;
            yield return null;
        }

        characterVisual.localPosition = originalPos;
        characterVisual.localScale = Vector3.one;
    }
    
    public void PlayDamageFlash()
    {
        if (meshRenderer != null)
            StartCoroutine(Flash3D());
    }

    private IEnumerator Flash3D()
    {
        meshRenderer.GetPropertyBlock(colorBlock);
        colorBlock.SetColor("_Color", damageFlashColor);
        meshRenderer.SetPropertyBlock(colorBlock);

        yield return new WaitForSeconds(flashDuration);

        colorBlock.SetColor("_Color", originalColor);
        meshRenderer.SetPropertyBlock(colorBlock);
    }
}
