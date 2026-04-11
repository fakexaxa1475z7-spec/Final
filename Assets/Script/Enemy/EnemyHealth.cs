using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int hp = 3;
    public GameObject expPrefab;

    bool isDead = false;

    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Instantiate(expPrefab, transform.position, Quaternion.identity); // 🔥 spawn ตรงนี้
        Destroy(gameObject);
    }
}