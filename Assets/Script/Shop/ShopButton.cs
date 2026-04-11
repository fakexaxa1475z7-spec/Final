using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Button button;

    UpgradeData upgrade;
    int cost;
    ShopManager shop;

    public int index; // ช่องที่เท่าไหร่
    public bool isLocked = false;

    public GameObject lockIcon; // รูป 🔒

    public void Setup(UpgradeData up, int price, ShopManager m)
    {
        // ❗ ถ้าล็อกอยู่ → ไม่เปลี่ยนของ
        if (isLocked) return;

        upgrade = up;
        cost = price;
        shop = m;

        text.text = up.upgradeName + "\nCost: " + cost;
        button.interactable = true;
    }

    public void OnClick()
    {
        shop.BuyUpgrade(upgrade, cost, this);
    }

    public void Disable()
    {
        button.interactable = false;
        text.text += "\nBOUGHT";
    }

    // 🔒 กดล็อก
    public void ToggleLock()
    {
        isLocked = !isLocked;

        // 🔒 เปิด/ปิดไอคอน
        lockIcon.SetActive(isLocked);

        // 💾 เซฟข้าม scene
        ShopData.lockedSlots[index] = isLocked;

        Debug.Log(isLocked ? "LOCKED" : "UNLOCKED");
    }
}