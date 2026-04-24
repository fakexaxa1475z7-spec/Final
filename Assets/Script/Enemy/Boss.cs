using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    enum State
    {
        Chase,
        Attack,
        Skill,
        Dead
    }

    State currentState;

    Transform player;

    [Header("Stats")]
    public float speed = 2f;
    public int damage = 3;

    [Header("Attack")]
    public GameObject bulletPrefab;
    public float attackCooldown = 2f;
    float attackTimer;

    [Header("Skill")]
    public float dashForce = 10f;
    public float skillCooldown = 5f;
    float skillTimer;

    [Header("Phase")]
    bool phase2 = false;

    Animator anim;
    SpriteRenderer sr;
    public Transform pivot;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (PlayerWeapon.instance != null)
            PlayerWeapon.instance.AddEnemy(transform);

        currentState = State.Chase;

        FindPlayer();
    }
    void OnDestroy()
    {
        if (PlayerWeapon.instance != null)
            PlayerWeapon.instance.RemoveEnemy(transform);
    }

    void Update()
    {
        if (player == null) FindPlayer();
        if (player == null) return;
        Flip();

        switch (currentState)
        {
            case State.Chase:
                Chase();
                break;

            case State.Attack:
                Attack();
                break;

            case State.Skill:
                break;
        }

        float moveSpeed = (player != null)
    ? Vector2.Distance(transform.position, player.position)
    : 0;

        anim.SetFloat("Speed", moveSpeed);

        attackTimer += Time.deltaTime;
        skillTimer += Time.deltaTime;

    }

    void FindPlayer()
    {
        if (PlayerStats.instance != null)
            player = PlayerStats.instance.transform;
    }

    void Chase()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);

        anim.SetFloat("Speed", speed);

        if (dist < 6f && attackTimer >= attackCooldown)
        {
            currentState = State.Attack;
        }
        else if (skillTimer >= skillCooldown)
        {
            StartCoroutine(Dash());
        }
    }

    void Attack()
    {
        attackTimer = 0f;

        anim.SetTrigger("Attack"); // 🔥 เล่น animation

        StartCoroutine(ShootBurst());

        currentState = State.Chase;
    }

    public Transform firePoint;

    IEnumerator ShootBurst()
    {
        int bulletCount = phase2 ? 5 : 3;

        for (int i = 0; i < bulletCount; i++)
        {
            if (player == null) yield break;

            Vector2 dir = (player.position - firePoint.position).normalized;

            EnemyBullet b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity)
                .GetComponent<EnemyBullet>();

            b.Init(dir, damage);

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Dash()
    {
        currentState = State.Skill;

        anim.SetTrigger("Skill"); // 🔥 เล่น skill

        skillTimer = 0f;

        Vector2 dir = (player.position - transform.position).normalized;

        float t = 0.3f;
        while (t > 0)
        {
            transform.position += (Vector3)(dir * dashForce * Time.deltaTime);
            t -= Time.deltaTime;
            yield return null;
        }

        currentState = State.Chase;
    }
    void Flip()
    {
        if (player == null) return;

        Vector3 scale = pivot.localScale;

        if (player.position.x < transform.position.x)
            scale.x = Mathf.Abs(scale.x);   // หันขวา
        else
            scale.x = -Mathf.Abs(scale.x);  // หันซ้าย

        pivot.localScale = scale;
    }
}