using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 input;
    Animator anim;
    public float scaleX = 1.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // 🔥 เพิ่ม
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // 🔥 ส่งค่าไป Animator
        anim.SetFloat("Speed", input.sqrMagnitude);

        if (input.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = input.x > 0 ? scaleX : -scaleX;
            transform.localScale = scale;
        }
    }

    void FixedUpdate()
    {
        float speed = PlayerStats.instance.moveSpeed;
        rb.velocity = input.normalized * speed;
    }
}