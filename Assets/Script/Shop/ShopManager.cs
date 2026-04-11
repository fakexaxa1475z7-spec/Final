using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPanel;
    PlayerStats player;
    PlayerExp exp;
    public TextMeshProUGUI rerollText;
    public int rerollCost = 5;
    bool isRerolling = false;

    public ShopButton[] buttons; // 🔥 ปุ่มในร้าน
    public UpgradeData[] allUpgrades;

    public int level = 1;

    void Start()
    {
        shopPanel.SetActive(false);

        player = PlayerStats.instance; // 🔥 ดึงจาก instance
        exp = PlayerExp.instance;
    }
    void Update()
    {
        if (player == null) player = PlayerStats.instance;
        if (exp == null) exp = PlayerExp.instance;
    }

    public void OpenShop()
    {
        Time.timeScale = 0f;
        shopPanel.SetActive(true);

        ShopData.Init(buttons.Length);

        // 🔥 โหลดสถานะ lock
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].isLocked = ShopData.lockedSlots[i];
            buttons[i].lockIcon.SetActive(buttons[i].isLocked);
        }

        GenerateShop();
        UpdateRerollUI();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // 🎲 สุ่มของในร้าน
    void GenerateShop()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].isLocked) continue;

            UpgradeData up = allUpgrades[Random.Range(0, allUpgrades.Length)];

            buttons[i].Setup(up, up.cost, this);
        }
    }

    // 💰 ซื้อของ
    public void BuyUpgrade(UpgradeData up, int cost, ShopButton btn)
    {
        if (exp.SpendExp(cost))
        {
            player.ApplyUpgrade(up);
            btn.Disable(); // 🔥 ซื้อแล้วกดไม่ได้อีก
        }
        else
        {
            Debug.Log("Not enough EXP!");
        }
    }
    public void Reroll()
    {
        if (isRerolling) return;

        if (exp.SpendExp(rerollCost))
        {
            isRerolling = true;

            GenerateShop();
            rerollCost += 2;

            isRerolling = false;
            UpdateRerollUI();
        }
        else
        {
            Debug.Log("Not enough EXP for reroll!");
        }
    }
    void UpdateRerollUI()
    {
        rerollText.text = "Reroll (" + rerollCost + ")";
    }

    // ▶️ ไปด่านต่อ
    public void NextLevel()
    {
        Time.timeScale = 1f;

        level++; // 🔥 เพิ่มเลเวลก่อนโหลด

        SceneManager.LoadScene("Level" + level);
    }
}