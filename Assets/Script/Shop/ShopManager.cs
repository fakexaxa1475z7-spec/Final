using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPanel;

    public UpgradeShopManager upgradeShop;
    public WeaponShopManager weaponShop;

    public int upgradeSlotCount = 3;
    public int weaponSlotCount = 3;

    public int level = 1;

    public void OpenShop()
    {
        Time.timeScale = 0f;
        shopPanel.SetActive(true);

        ShopData.Init(upgradeSlotCount, weaponSlotCount);

        upgradeShop.Open();
        weaponShop.Open();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void NextLevel()
    {
        Time.timeScale = 1f;

        level++; // 🔥 เพิ่มเลเวลก่อนโหลด

        SceneManager.LoadScene("Level" + level);
    }
}