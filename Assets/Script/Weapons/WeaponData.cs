using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Game/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponType weaponType;
    public GameObject bulletPrefab;
    public GameObject weaponModel;
    public float fireRate = 1f;
    public int damage = 1;
    public int cost = 100;

    public int pelletCount;   // shotgun
    public float spread;      // shotgun
}

public enum WeaponType
{
    Gun,
    Shotgun,
    Sword
}