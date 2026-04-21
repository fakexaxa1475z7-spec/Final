using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;

    Transform player;

    void Start()
    {
        if (PlayerWeapon.instance != null)
            PlayerWeapon.instance.AddEnemy(transform);
    }

    void OnDestroy()
    {
        if (PlayerWeapon.instance != null)
            PlayerWeapon.instance.RemoveEnemy(transform);
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
        // 🔥 ใช้ instance
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
}