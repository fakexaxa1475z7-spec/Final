using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    public static CharacterData selectedCharacter;

    public int damage = 1;
    public float attackSpeed = 1f;
    public int maxHP = 10;

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

    public void ApplyUpgrade(UpgradeData up)
    {
        damage += up.damage;
        attackSpeed += up.attackSpeed;
        maxHP += up.hp;
    }
}