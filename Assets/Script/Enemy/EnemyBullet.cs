using UnityEngine;
using System.Collections.Generic;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 8f;
    public float lifeTime = 4f;

    public bool isPiercing = false;
    public int maxHit = 1;

    int damage;
    int hitCount = 0;

    Vector2 direction;
    Rigidbody2D rb;

    HashSet<Collider2D> hitTargets = new HashSet<Collider2D>();

    // 🔥 เรียกตอนยิง
    public void Init(Vector2 dir, int dmg)
    {
        direction = dir.normalized;
        damage = dmg;

        // 🔄 หมุนกระสุน
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
        // 🎯 ยิงโดน Player เท่านั้น
        if (!col.CompareTag("Player")) return;

        // 🔥 กันโดนซ้ำ
        if (hitTargets.Contains(col)) return;
        hitTargets.Add(col);

        PlayerHealth player = col.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }

        hitCount++;

        // ❌ ไม่ทะลุ
        if (!isPiercing)
        {
            Destroy(gameObject);
            return;
        }

        // 🔥 ทะลุแต่มี limit
        if (hitCount >= maxHit)
        {
            Destroy(gameObject);
        }
    }
}