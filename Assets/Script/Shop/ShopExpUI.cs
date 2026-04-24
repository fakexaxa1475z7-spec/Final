using TMPro;
using UnityEngine;

public class ShopExpUI : MonoBehaviour
{
    public TextMeshProUGUI expText;

    void Start()
    {
        UpdateUI();

        // 🔥 subscribe event
        if (PlayerExp.instance != null)
            PlayerExp.instance.OnExpChanged += OnExpChanged;
    }

    void OnDestroy()
    {
        if (PlayerExp.instance != null)
            PlayerExp.instance.OnExpChanged -= OnExpChanged;
    }

    void OnExpChanged(int exp)
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (PlayerExp.instance == null) return;

        expText.text = "EXP: " + PlayerExp.instance.currentExp;
    }
}