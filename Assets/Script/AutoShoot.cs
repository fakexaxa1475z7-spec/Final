using UnityEngine;
using System.Collections.Generic;

public class AutoShoot : MonoBehaviour
{
    public GameObject bulletPrefab;

    float timer;
    List<Transform> enemies = new List<Transform>();

    void Update()
    {
        if (PlayerStats.instance == null) return;

        float fireRate = PlayerStats.instance.attackSpeed; // 🔥 ใช้ stat

        timer += Time.deltaTime;

        if (timer >= 1f / fireRate)
        {
            ShootNearest();
            timer = 0f;
        }
    }

    void ShootNearest()
    {
        if (enemies.Count == 0) return;

        Transform nearest = null;
        float minDist = Mathf.Infinity;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                continue;
            }

            float dist = Vector2.Distance(transform.position, enemies[i].position);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemies[i];
            }
        }

        if (nearest == null) return;

        Vector2 dir = (nearest.position - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(dir);
    }

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