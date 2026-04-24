using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 3;

    AudioSource audioSource;
    Collider2D col2D;

    bool isCollected = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        col2D = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (isCollected) return; // 🔥 กันซ้ำทันที

        if (col.CompareTag("Player"))
        {
            isCollected = true;

            PlayerHealth hp = col.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeHeal(healAmount);
            }

            // 🔥 ปิด collider กัน trigger ซ้ำ
            if (col2D != null)
                col2D.enabled = false;

            gameObject.GetComponent<SpriteRenderer>().enabled = false;

            // 🔥 เล่นเสียง + ค่อยลบ
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
                Destroy(gameObject, audioSource.clip.length);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}