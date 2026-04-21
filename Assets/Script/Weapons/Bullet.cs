using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;

    int damage;
    Vector2 direction;
    Rigidbody2D rb;

    // 🔥 รับค่าจากอาวุธ
    public void Init(Vector2 dir, int dmg)
    {
        direction = dir.normalized;
        damage = dmg;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            bool isCrit;

            // 🔥 ใช้ crit จาก PlayerStats แต่ base damage จาก weapon
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

            Destroy(gameObject);
        }
    }
}