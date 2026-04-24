using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    public int maxHP = 10;
    int currentHP;
    float regenTimer = 0f;

    public float invincibleTime = 1f;
    bool isInvincible = false;

    SpriteRenderer sr;
    AudioSource audioSource;

    [Header("SFX")]
    public AudioClip hurtSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        maxHP = PlayerStats.instance.maxHP;
        currentHP = maxHP;
    }

    void Update()
    {
        RegenHP();
    }

    void RegenHP()
    {
        float regen = PlayerStats.instance.hpRegen;

        if (regen <= 0) return;
        if (currentHP >= maxHP) return;

        regenTimer += Time.deltaTime;

        if (regenTimer >= 1f)
        {
            int healAmount = Mathf.FloorToInt(regen);

            if (healAmount > 0)
                TakeHeal(healAmount);

            regenTimer = 0f;
        }
    }

    IEnumerator Flash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    public void TakeDamage(int dmg)
    {
        if (isInvincible) return;

        currentHP -= dmg;

        PlayHurtSound(); // 🔥 เล่นเสียงเจ็บ

        Debug.Log("HP: " + currentHP);

        StartCoroutine(Flash());

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Invincible());
        }
    }

    IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    public void TakeHeal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
    }

    void Die()
    {
        Debug.Log("Player Dead");
        EndGameData.instance.isWin = false;

        Destroy(gameObject);
        SceneManager.LoadScene("EndGame");
    }

    void PlayHurtSound()
    {
        if (audioSource != null && hurtSound != null)
            audioSource.PlayOneShot(hurtSound);
    }


    public void SetMaxHP(int newMaxHP, bool healFull = true)
    {
        int diff = newMaxHP - maxHP;
        maxHP = newMaxHP;

        if (healFull)
        {
            currentHP = maxHP;
        }
        else
        {
            currentHP += diff;
            if (currentHP > maxHP) currentHP = maxHP;
        }
    }

    public float GetHPPercent()
    {
        return (float)currentHP / maxHP;
    }
}