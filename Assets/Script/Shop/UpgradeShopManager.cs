using UnityEngine;

public class UpgradeShopManager : MonoBehaviour
{
    public ShopButton[] buttons;
    public UpgradeData[] allUpgrades;

    public int rerollCost = 5;

    public void Open()
    {
        Generate();
    }

    public void Generate()
    {
        var pool = new System.Collections.Generic.List<UpgradeData>(allUpgrades);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].isLocked = ShopData.upgradeLocked[i];
            buttons[i].lockIcon.SetActive(buttons[i].isLocked);

            if (buttons[i].isLocked) continue;

            int rand = Random.Range(0, pool.Count);
            var up = pool[rand];

            pool.RemoveAt(rand);

            buttons[i].Setup(up, up.cost, this);
        }
    }

    public void Buy(UpgradeData up, int cost, ShopButton btn)
    {
        if (PlayerExp.instance.SpendExp(cost))
        {
            PlayerStats.instance.ApplyUpgrade(up);
            btn.Disable();
            Refresh();
        }
    }

    public void Reroll()
    {
        if (PlayerExp.instance.SpendExp(rerollCost))
        {
            Generate();
            rerollCost += 2;
        }
    }

    public void Refresh()
    {
        foreach (var b in buttons)
            b.Refresh();
    }
}