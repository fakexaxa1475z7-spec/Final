using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 3;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerHealth>().TakeHeal(healAmount);
            Destroy(gameObject);
        }
    }
}