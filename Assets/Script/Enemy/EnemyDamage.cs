using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}