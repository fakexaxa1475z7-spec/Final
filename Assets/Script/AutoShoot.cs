using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 1f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / fireRate)
        {
            ShootNearest();
            timer = 0f;
        }
    }

    void ShootNearest()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0) return;

        GameObject nearest = enemies[0];
        float minDist = Vector2.Distance(transform.position, nearest.transform.position);

        foreach (GameObject e in enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = e;
            }
        }

        Vector2 dir = (nearest.transform.position - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(dir);
    }
}