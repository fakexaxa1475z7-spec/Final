using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerWeapon : MonoBehaviour
{
    public static PlayerWeapon instance;

    public List<WeaponSlot> weapons = new List<WeaponSlot>();

    public int maxSlots = 2;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Level"))
        {
            // 🔥 หา player ใหม่
            var player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                transform.SetParent(player.transform);
                transform.localPosition = Vector3.zero;
            }

            InitWeapons();
        }
    }

    void InitWeapons()
    {
        if (weapons.Count > 0) return;

        var start = PlayerStats.selectedCharacter.startingWeapon;

        if (start != null && !HasWeapon(start))
        {
            Equip(start);
        }
    }

    bool HasWeapon(WeaponData w)
    {
        foreach (var s in weapons)
            if (s.weapon == w) return true;

        return false;
    }

    void Update()
    {
        if (PlayerStats.instance == null) return;

        for (int i = 0; i < weapons.Count; i++)
        {
            var slot = weapons[i];
            if (slot.weapon == null) continue;

            float fireRate = slot.weapon.fireRate * PlayerStats.instance.attackSpeed;
            fireRate = Mathf.Max(0.1f, fireRate);

            slot.timer += Time.deltaTime;

            while (slot.timer >= 1f / fireRate)
            {
                Shoot(slot.weapon, i);
                slot.timer -= 1f / fireRate;
            }
        }
    }

    void Shoot(WeaponData weapon, int index)
    {
        var enemies = PlayerWeapon.instance.enemies;
        if (enemies.Count == 0) return;

        // 🔥 ใช้ Transform แทน GameObject
        List<Transform> sorted = new List<Transform>(enemies);

        sorted.Sort((a, b) =>
            Vector2.Distance(transform.position, a.position)
            .CompareTo(Vector2.Distance(transform.position, b.position))
        );

        // 🔥 ยิงคนละเป้า
        Transform target = sorted[Mathf.Min(index, sorted.Count - 1)];

        Vector2 dir = (target.position - transform.position).normalized;

        // 🔥 ยิงคนละตำแหน่ง
        Vector3 offset = (index == 0) ? Vector3.left * 0.3f : Vector3.right * 0.3f;

        Bullet b = Instantiate(weapon.bulletPrefab, transform.position + offset, Quaternion.identity)
            .GetComponent<Bullet>();

        int dmg = weapon.damage + PlayerStats.instance.damage;

        b.Init(dir, dmg);
    }

    // 🔫 ใส่อาวุธ
    public void Equip(WeaponData weapon)
    {
        if (weapon == null) return;

        if (weapons.Count < maxSlots)
        {
            weapons.Add(new WeaponSlot { weapon = weapon, timer = 0f });
        }
        else
        {
            // 🔥 เต็ม → replace ช่อง 1 (หรือทำ UI ให้เลือก)
            weapons[1].weapon = weapon;
            weapons[1].timer = 0f;

            Debug.Log("🔁 Replaced slot 1 with: " + weapon.weaponName);
        }

        Debug.Log("Equipped: " + weapon.weaponName);
    }
    public List<Transform> enemies = new List<Transform>();

    public void AddEnemy(Transform e)
    {
        if (!enemies.Contains(e))
            enemies.Add(e);
    }

    public void RemoveEnemy(Transform e)
    {
        if (enemies.Contains(e))
            enemies.Remove(e);
    }
}