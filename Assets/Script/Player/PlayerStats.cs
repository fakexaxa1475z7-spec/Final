using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    public int damage = 1;
    public float attackSpeed = 1f;
    public int maxHP = 10;

    void Awake()
    {
        // 🔥 กันซ้ำ
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 🔥 ไม่โดนลบตอนเปลี่ยน scene
        }
        else
        {
            Destroy(gameObject); // ❌ กันตัวซ้ำ
        }
    }

    public void ApplyUpgrade(UpgradeData up)
    {
        damage += up.damage;
        attackSpeed += up.attackSpeed;
        maxHP += up.hp;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transform.position = Vector3.zero; // หรือ spawn point
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}