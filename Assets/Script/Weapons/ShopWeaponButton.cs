using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopWeaponButton : MonoBehaviour
{
    public WeaponData weapon;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public Button button;

    public int index;
    public bool isLocked = false;
    public GameObject lockIcon;

    WeaponShopManager manager;

    bool isBought = false;

    void Start()
    {
        if (PlayerExp.instance != null)
            PlayerExp.instance.OnExpChanged += OnExpChanged;
    }

    void OnDestroy()
    {
        if (PlayerExp.instance != null)
            PlayerExp.instance.OnExpChanged -= OnExpChanged;
    }

    void OnExpChanged(int _)
    {
        Refresh();
    }

    public void Setup(WeaponData w, WeaponShopManager m)
    {
        if (isLocked) return;

        weapon = w;
        manager = m;
        isBought = false;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (weapon == null) return;

        nameText.text = weapon.weaponName;

        if (isBought)
        {
            costText.text = "BOUGHT";
            button.interactable = false;
            return;
        }

        costText.text = weapon.cost.ToString();

        button.interactable =
            PlayerExp.instance != null &&
            PlayerExp.instance.currentExp >= weapon.cost;

        lockIcon.SetActive(isLocked);
    }

    public void OnClick()
    {
        if (weapon == null || isBought) return;

        if (PlayerExp.instance != null &&
            PlayerExp.instance.SpendExp(weapon.cost))
        {
            if (PlayerWeapon.instance != null)
            {
                PlayerWeapon.instance.Equip(weapon);
            }
            else
            {
                Debug.LogError("❌ PlayerWeapon not found!");
                return;
            }

            isBought = true;

            manager.Refresh();
        }
        else
        {
            Debug.Log("❌ Not enough EXP");
        }
    }

    public void ToggleLock()
    {
        isLocked = !isLocked;

        lockIcon.SetActive(isLocked);

        ShopData.weaponLocked[index] = isLocked;
    }

    public void Refresh()
    {
        UpdateUI();
    }
}