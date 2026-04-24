using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerWeapon : MonoBehaviour
{
    public static PlayerWeapon instance;

    public List<WeaponSlot> weapons = new List<WeaponSlot>();

    public int maxSlots = 2;

    List<GameObject> weaponObjects = new List<GameObject>();

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

            SyncWeaponModels();
        }
    }

    void InitWeapons()
    {
        if (weapons.Count > 0) return;

        if (PlayerStats.selectedCharacter == null) return;

        var start = PlayerStats.selectedCharacter.startingWeapon;

        if (start != null)
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

        UpdateWeaponRotation();

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
        switch (weapon.weaponType)
        {
            case WeaponType.Gun:
                ShootGun(weapon, index);
                break;

            case WeaponType.Shotgun:
                ShootShotgun(weapon, index);
                break;

            case WeaponType.Sword:
                ShootSword(weapon, index);
                break;
        }
    }
    void ShootGun(WeaponData weapon, int index)
    {
        var target = GetTarget(index);
        if (target == null) return;

        Vector2 dir = (target.position - transform.position).normalized;

        var b = Instantiate(weapon.bulletPrefab, transform.position, Quaternion.identity)
            .GetComponent<Bullet>();

        int dmg = weapon.damage + PlayerStats.instance.damage;
        b.Init(dir, dmg);
    }
    void ShootShotgun(WeaponData weapon, int index)
    {
        var target = GetTarget(index);
        if (target == null) return;

        Vector2 baseDir = (target.position - transform.position).normalized;

        for (int i = 0; i < weapon.pelletCount; i++)
        {
            float angle = Random.Range(-weapon.spread, weapon.spread);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * baseDir;

            var b = Instantiate(weapon.bulletPrefab, transform.position, Quaternion.identity)
                .GetComponent<Bullet>();

            int dmg = weapon.damage + PlayerStats.instance.damage;
            b.Init(dir, dmg);
        }
    }
    void ShootSword(WeaponData weapon, int index)
    {
        var target = GetTarget(index);
        if (target == null) return;

        Vector2 dir = (target.position - transform.position).normalized;

        var s = Instantiate(weapon.bulletPrefab, transform.position, Quaternion.identity)
            .GetComponent<Bullet>();

        int dmg = weapon.damage + PlayerStats.instance.damage;
        s.Init(dir, dmg);
    }
    Transform GetTarget(int index)
    {
        if (enemies.Count == 0) return null;

        List<Transform> sorted = new List<Transform>(enemies);

        sorted.Sort((a, b) =>
        {
            if (a == null || b == null) return 0;

            // 🔥 ให้ Boss priority สูงสุด
            bool aBoss = a.CompareTag("Boss");
            bool bBoss = b.CompareTag("Boss");

            if (aBoss && !bBoss) return -1; // a มาก่อน
            if (!aBoss && bBoss) return 1;  // b มาก่อน

            // 🔥 ถ้าเป็นประเภทเดียวกัน → วัดระยะ
            float distA = Vector2.Distance(transform.position, a.position);
            float distB = Vector2.Distance(transform.position, b.position);

            return distA.CompareTo(distB);
        });

        return sorted[Mathf.Min(index, sorted.Count - 1)];
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

        SyncWeaponModels();

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

    void SyncWeaponModels()
    {
        foreach (var obj in weaponObjects)
        {
            if (obj != null)
                Destroy(obj);
        }

        weaponObjects.Clear();

        for (int i = 0; i < weapons.Count; i++)
        {
            SpawnWeaponModel(weapons[i].weapon, i);
        }
    }
    void SpawnWeaponModel(WeaponData weapon, int index)
    {
        if (weapon.weaponModel == null) return;

        GameObject obj = Instantiate(weapon.weaponModel, transform);

        // 🔥 ซ้าย / ขวา
        Vector3 offset = Vector3.zero;

        if (index == 0) offset = new Vector3(-0.5f, 0, 0); // ซ้าย
        else if (index == 1) offset = new Vector3(0.5f, 0, 0); // ขวา

        obj.transform.localPosition = offset;

        // 🔥 scale กันกลับด้าน
        obj.transform.localScale = Vector3.one;

        // เก็บไว้
        if (weaponObjects.Count <= index)
            weaponObjects.Add(obj);
        else
            weaponObjects[index] = obj;
    }

    void UpdateWeaponRotation()
    {
        if (enemies.Count == 0) return;

        Transform target = enemies[0];

        foreach (var obj in weaponObjects)
        {
            if (obj == null) continue;

            Vector2 dir = (target.position - obj.transform.position).normalized;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            angle += 180f;
            obj.transform.rotation = Quaternion.Euler(0, 0, angle);

            // 🔥 flip ซ้ายขวา
            Vector3 scale = obj.transform.localScale;

            if (dir.x < 0)
                scale.y = Mathf.Abs(scale.y); // กลับด้าน
            else
                scale.y = -Mathf.Abs(scale.y);  // ปกติ

            obj.transform.localScale = scale;
        }
    }
}