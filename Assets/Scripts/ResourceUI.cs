using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI gemsText;

    void Update()
    {
        if (ResourceManager.Instance != null)
        {
            goldText.text = $"Oro: {ResourceManager.Instance.gold}";
            gemsText.text = $"Gemas: {ResourceManager.Instance.gems}";
        }
    }
}