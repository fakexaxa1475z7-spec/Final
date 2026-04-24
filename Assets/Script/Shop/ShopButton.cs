using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Button button;

    public Image iconImage; // ⭐ เพิ่มตรงนี้

    public int index;
    public bool isLocked = false;
    public GameObject lockIcon;

    UpgradeData upgrade;
    int cost;
    UpgradeShopManager shop;

    public void Setup(UpgradeData up, int price, UpgradeShopManager m)
    {
        if (isLocked) return;

        upgrade = up;
        cost = price;
        shop = m;

        text.text = up.upgradeName + "\nCost: " + cost;

        // ⭐ ใส่รูปตรงนี้
        if (iconImage != null && up.icon != null)
        {
            iconImage.sprite = up.icon;
            iconImage.enabled = true;
        }

        Refresh();
    }

    public void OnClick()
    {
        shop.Buy(upgrade, cost, this);
    }

    public void Disable()
    {
        button.interactable = false;
        text.text += "\nBOUGHT";
    }

    public void ToggleLock()
    {
        isLocked = !isLocked;
        lockIcon.SetActive(isLocked);

        ShopData.upgradeLocked[index] = isLocked;
    }

    public void Refresh()
    {
        if (upgrade == null) return;

        button.interactable = PlayerExp.instance.currentExp >= cost;
    }
}