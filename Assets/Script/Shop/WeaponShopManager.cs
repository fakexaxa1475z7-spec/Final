using UnityEngine;
using System.Collections.Generic;

public class WeaponShopManager : MonoBehaviour
{
    public ShopWeaponButton[] buttons;
    public WeaponData[] allWeapons;

    public int rerollCost = 5;

    void Start()
    {
        Open();
        InitLockData();
    }
    void InitLockData()
    {
        if (ShopData.weaponLocked == null ||
            ShopData.weaponLocked.Length != buttons.Length)
        {
            ShopData.weaponLocked = new bool[buttons.Length];
        }
    }

    public void Open()
    {
        Generate();
        Refresh();
    }

    public void Generate()
    {
        // 🔥 กัน null + ขนาดไม่ตรง
        if (ShopData.weaponLocked == null ||
            ShopData.weaponLocked.Length != buttons.Length)
        {
            ShopData.weaponLocked = new bool[buttons.Length];
        }

        var pool = new List<WeaponData>(allWeapons);

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == null) continue; // 🔥 กัน inspector ลืมใส่

            buttons[i].isLocked = ShopData.weaponLocked[i];

            if (buttons[i].lockIcon != null)
                buttons[i].lockIcon.SetActive(buttons[i].isLocked);

            if (buttons[i].isLocked) continue;

            if (pool.Count == 0) break;

            int rand = Random.Range(0, pool.Count);
            var w = pool[rand];

            pool.RemoveAt(rand);

            buttons[i].Setup(w, this);
        }
    }

    public void Reroll()
    {
        if (PlayerExp.instance == null) return;

        if (PlayerExp.instance.SpendExp(rerollCost))
        {
            Generate();
            rerollCost += 2;

            Refresh();
        }
        else
        {
            Debug.Log("❌ Not enough EXP for reroll");
        }
    }

    public void Refresh()
    {
        foreach (var b in buttons)
            b.Refresh();
    }
}