using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    public static CharacterData selectedCharacter;

    public int damage = 1;
    public float attackSpeed = 1f;
    public int maxHP = 10;
    public float hpRegen = 0f;

    public float critRate = 0.1f;     // 10%
    public float critDamage = 2f;   // x2 ดาเมจ

    public float moveSpeed = 5f;

    [SerializeField] CharacterData defaultCharacter;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (selectedCharacter == null)
                selectedCharacter = defaultCharacter;

            ApplyCharacter();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplyCharacter()
    {
        if (selectedCharacter != null)
        {
            maxHP = selectedCharacter.maxHP;
            damage = selectedCharacter.damage;
            attackSpeed = selectedCharacter.attackSpeed;

            Debug.Log("Applied: " + selectedCharacter.characterName);
        }
        else
        {
            Debug.LogWarning("No character selected!");
        }
    }

    public int GetDamage(out bool isCrit)
    {
        isCrit = Random.value < critRate;

        if (isCrit)
            return Mathf.RoundToInt(damage * critDamage);
        else
            return damage;
    }

    public void ApplyUpgrade(UpgradeData up)
    {
        damage += up.damage;
        attackSpeed += up.attackSpeed;
        maxHP += up.hp;
        hpRegen += up.hpRegen;
        critRate += up.critRate;
        critDamage += up.critDamage;
        moveSpeed += up.moveSpeed;
    }
}