using UnityEngine;
using System.Collections.Generic;

public class LevelUpManager : MonoBehaviour
{
    public GameObject panel;
    public UpgradeData[] allUpgrades;

    public UpgradeButton[] buttons; // UI 3 ปุ่ม

    PlayerStats player;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        panel.SetActive(false);
    }

    public void OpenLevelUp()
    {
        Time.timeScale = 0f;
        panel.SetActive(true);

        List<UpgradeData> selected = new List<UpgradeData>();

        while (selected.Count < 3)
        {
            UpgradeData rand = allUpgrades[Random.Range(0, allUpgrades.Length)];

            if (!selected.Contains(rand))
                selected.Add(rand);
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetUpgrade(selected[i], this);
        }
    }

    public void ChooseUpgrade(UpgradeData up)
    {
        player.ApplyUpgrade(up);

        panel.SetActive(false);
        Time.timeScale = 0f;

        // 👉 ไป Shop ต่อ
        FindObjectOfType<ShopManager>().OpenShop();
    }
}