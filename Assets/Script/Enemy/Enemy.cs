using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public float scale = 1f;
    public int face = 1;

    Transform player;

    int facing = 1; // 1 = ขวา, -1 = ซ้าย

    void Start()
    {
        if (PlayerWeapon.instance != null)
            PlayerWeapon.instance.AddEnemy(transform);
    }

    void OnDestroy()
    {
        if (PlayerWeapon.instance != null)
            PlayerWeapon.instance.RemoveEnemy(transform);
    }

    void Update()
    {
        FindPlayer();
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;

        Move(dir);
    }
    void LateUpdate()
    {
        FindPlayer();
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;

        if (Mathf.Abs(dir.x) > 0.1f)
        {
            facing = dir.x > 0 ? face : -face;
        }

        transform.localScale = new Vector3(-facing * scale, scale, 1);
    }


    void Move(Vector2 dir)
    {
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    void FindPlayer()
    {
        if (player == null && PlayerStats.instance != null)
            player = PlayerStats.instance.transform;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Vector2 push = (transform.position - col.transform.position).normalized;
            transform.position += (Vector3)(push * 0.5f * Time.deltaTime);
        }
    }
}