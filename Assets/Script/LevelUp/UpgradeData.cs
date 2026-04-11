using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;

    public int damage;
    public float attackSpeed;
    public int hp;

    public int cost;
}