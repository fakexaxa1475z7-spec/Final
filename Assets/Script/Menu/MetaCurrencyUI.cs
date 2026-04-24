using TMPro;
using UnityEngine;

public class MetaCurrencyUI : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    void Start()
    {
        UpdateUI();

        if (MetaCurrency.instance != null)
            MetaCurrency.instance.OnMoneyChanged += OnMoneyChanged;
    }

    void OnDestroy()
    {
        if (MetaCurrency.instance != null)
            MetaCurrency.instance.OnMoneyChanged -= OnMoneyChanged;
    }

    void OnMoneyChanged(int amount)
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (MetaCurrency.instance == null)
        {
            moneyText.text = "Currrency 0";
            return;
        }

        moneyText.text = $"Currency {MetaCurrency.instance.total}";
    }
}