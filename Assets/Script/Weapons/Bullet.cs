using UnityEngine;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;

    public bool isPiercing = false; // 🔥 ทะลุได้ไหม
    public int maxHit = 3;          // 🔥 ทะลุได้กี่ตัว (กันโกง)

    int damage;
    int hitCount = 0;

    Vector2 direction;
    Rigidbody2D rb;

    // 🔥 กันตีซ้ำตัวเดิม
    HashSet<Collider2D> hitTargets = new HashSet<Collider2D>();

    public void Init(Vector2 dir, int dmg)
    {
        direction = dir.normalized;
        damage = dmg;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy")) return;

        // 🔥 กันตีซ้ำ
        if (hitTargets.Contains(col)) return;
        hitTargets.Add(col);

        bool isCrit;
        int finalDmg = damage;

        if (PlayerStats.instance != null)
        {
            isCrit = Random.value < PlayerStats.instance.critRate;

            if (isCrit)
                finalDmg = Mathf.RoundToInt(damage * PlayerStats.instance.critDamage);
        }
        else
        {
            isCrit = false;
        }

        EnemyHealth enemy = col.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(finalDmg, isCrit);
        }

        if (isCrit)
            Debug.Log("CRIT! " + finalDmg);

        hitCount++;

        // ❌ ไม่ทะลุ → หายทันที
        if (!isPiercing)
        {
            Destroy(gameObject);
            return;
        }

        // 🔥 ทะลุได้ แต่มี limit
        if (hitCount >= maxHit)
        {
            Destroy(gameObject);
        }
    }
}