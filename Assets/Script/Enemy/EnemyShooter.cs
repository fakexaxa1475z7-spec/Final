using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float fireRate = 1f; // ยิงต่อวินาที
    public int damage = 1;

    float timer;
    Transform player;

    void Start()
    {
        FindPlayer();
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        timer += Time.deltaTime;

        if (timer >= 1f / fireRate)
        {
            Shoot();
            timer = 0f;
        }

        RotateToPlayer();
    }

    void FindPlayer()
    {
        if (PlayerStats.instance != null)
            player = PlayerStats.instance.transform;
        else
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        Vector2 dir = (player.position - firePoint.position).normalized;

        EnemyBullet b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity)
            .GetComponent<EnemyBullet>();

        b.Init(dir, damage);
    }

    void RotateToPlayer()
    {
        if (player == null) return;

        // 👉 เช็คว่า player อยู่ซ้ายหรือขวา
        float dirX = player.position.x - transform.position.x;

        Vector3 scale = transform.localScale;

        if (dirX > 0)
            scale.x = Mathf.Abs(scale.x);   // 👉 หันขวา
        else
            scale.x = -Mathf.Abs(scale.x);  // 👉 หันซ้าย

        transform.localScale = scale;
    }
}