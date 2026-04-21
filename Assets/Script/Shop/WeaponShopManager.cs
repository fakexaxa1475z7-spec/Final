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
    }

    public void Open()
    {
        Generate();
        Refresh();
    }

    public void Generate()
    {
        var pool = new List<WeaponData>(allWeapons);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].isLocked = ShopData.weaponLocked[i];
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