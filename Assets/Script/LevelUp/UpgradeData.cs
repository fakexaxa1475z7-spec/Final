using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;

    public Sprite icon;

    public int damage;
    public float attackSpeed;
    public int hp;
    public float critRate;
    public float critDamage;
    public float moveSpeed;
    public float hpRegen;

    public int cost;
}