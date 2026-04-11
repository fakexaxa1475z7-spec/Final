using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public int damage = 1;

    Vector2 direction;
    Rigidbody2D rb;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        damage = PlayerStats.instance.damage;

        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = direction * speed; // 🔥 ใช้ physics

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            // 🔥 ส่ง damage ไป enemy
            EnemyHealth enemy = col.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            else
            {
                Destroy(col.gameObject); // fallback
            }

            Destroy(gameObject);
        }
    }
}