using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class CharacterData : ScriptableObject
{
    public bool isDefault;
    public string characterID;
    public string characterName;
    public GameObject prefab;

    public int maxHP;
    public int damage;
    public float attackSpeed;
    public float critRate;
    public float critDamage;
    public float moveSpeed;
    public float hpRegen;
    public WeaponData startingWeapon;

    public int unlockCost;
}

[System.Serializable]
public class WeaponSlot
{
    public WeaponData weapon;
    public float timer;
}