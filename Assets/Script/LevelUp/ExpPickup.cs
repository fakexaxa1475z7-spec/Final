using UnityEngine;

public class ExpPickup : MonoBehaviour
{
    public int expValue = 1;
    public float moveSpeed = 5f;
    public float attractRange = 2f;

    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // 🔵 วิ่งเข้าหาผู้เล่น (แบบดูด)
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist < attractRange) // ระยะดูด
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
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerExp>().AddExp(expValue);
            Destroy(gameObject);
        }
    }
}