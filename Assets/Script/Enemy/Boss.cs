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
    public int maxHP = 200;
    int currentHP;
    public int damage = 3;
    int total = 0;

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

    void Start()
    {
        total = PlayerPrefs.GetInt("META_MONEY", 0);

        currentHP = maxHP;
        currentState = State.Chase;

        FindPlayer();
    }

    void Update()
    {
        if (player == null) FindPlayer();
        if (player == null) return;

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

        attackTimer += Time.deltaTime;
        skillTimer += Time.deltaTime;

        CheckPhase();
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

        StartCoroutine(ShootBurst());

        currentState = State.Chase;
    }

    IEnumerator ShootBurst()
    {
        int bulletCount = phase2 ? 5 : 3;

        for (int i = 0; i < bulletCount; i++)
        {
            Vector2 dir = (player.position - transform.position).normalized;

            Bullet b = Instantiate(bulletPrefab, transform.position, Quaternion.identity)
    .GetComponent<Bullet>();

            b.Init(dir, damage); // ใส่ดาเมจ boss

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Dash()
    {
        currentState = State.Skill;

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

    void CheckPhase()
    {
        if (!phase2 && currentHP <= maxHP / 2)
        {
            phase2 = true;

            attackCooldown *= 0.7f; // ยิงเร็วขึ้น
            speed *= 1.5f;

            Debug.Log("🔥 Boss Phase 2!");
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        currentState = State.Dead;

        Debug.Log("💀 Boss Dead");

        Destroy(gameObject);

        total += 100; // 🔥 สมมติได้ 100 META_MONEY จากการชนะบอส
        PlayerPrefs.SetInt("META_MONEY", total);
        PlayerPrefs.Save();
        Destroy(GameObject.FindWithTag("Player")); // 🔥 ทำลายตัวละครผู้เล่น
        EndGameData.instance.isWin = true;
        SceneManager.LoadScene("EndGame");
    }
}