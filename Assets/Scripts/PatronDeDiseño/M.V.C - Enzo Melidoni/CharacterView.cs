using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterView : MonoBehaviour
{
    private Slider healthBar;
    private TextMeshProUGUI healthText;

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
}