using UnityEngine;

public class ExpPickup : MonoBehaviour
{
    public int expValue = 1;
    public float moveSpeed = 5f;
    public float attractRange = 2f;

    Transform player;
    AudioSource audioSource;

    bool isCollected = false; // 🔥 กันเก็บซ้ำ

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null)
            player = p.transform;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null || isCollected) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist < attractRange)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (isCollected) return; // 🔥 กันซ้ำ

        if (col.CompareTag("Player"))
        {
            isCollected = true; // 🔥 lock ทันที

            col.GetComponent<PlayerExp>()?.AddExp(expValue);

            if (audioSource != null)
            {
                audioSource.Play();
                Destroy(gameObject, audioSource.clip.length);
            }
            else
            {
                Destroy(gameObject);
            }

            // 🔥 ปิด collider กัน trigger ซ้ำ
            GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}