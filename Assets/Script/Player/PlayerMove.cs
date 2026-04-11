using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody2D rb;
    Vector2 move;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = move * speed;
    }
}