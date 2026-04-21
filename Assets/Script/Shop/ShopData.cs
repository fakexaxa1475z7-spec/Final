using UnityEngine;

public static class ShopData
{
    public static bool[] upgradeLocked;
    public static bool[] weaponLocked;

    public static void Init(int upgradeSize, int weaponSize)
    {
        if (upgradeLocked == null || upgradeLocked.Length != upgradeSize)
            upgradeLocked = new bool[upgradeSize];

        if (weaponLocked == null || weaponLocked.Length != weaponSize)
            weaponLocked = new bool[weaponSize];
    }
}