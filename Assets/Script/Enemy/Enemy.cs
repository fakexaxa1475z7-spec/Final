using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;

    Transform player;
    AutoShoot shooter;

    void Start()
    {
        // 🔥 cache shooter
        shooter = FindObjectOfType<AutoShoot>();

        if (shooter != null)
            shooter.AddEnemy(transform);
    }

    void Update()
    {
        FindPlayer();

        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;

        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    void FindPlayer()
    {
        // 🔥 ใช้ instance ถ้ามี
        if (player == null && PlayerStats.instance != null)
        {
            player = PlayerStats.instance.transform;
        }

        // fallback
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Vector2 push = (transform.position - col.transform.position).normalized;
            transform.position += (Vector3)(push * 0.5f * Time.deltaTime);
        }
    }

    void OnDestroy()
    {
        // 🔥 กัน null ตอนเปลี่ยน scene
        if (shooter != null)
        {
            shooter.RemoveEnemy(transform);
        }
    }
}