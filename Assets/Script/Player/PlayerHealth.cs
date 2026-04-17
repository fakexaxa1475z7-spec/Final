using System.Collections;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 10;
    int currentHP;
    float regenTimer = 0f;

    public float invincibleTime = 1f;
    bool isInvincible = false;
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        maxHP = PlayerStats.instance.maxHP; // 🔥 ดึงจาก stats
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
            {
                TakeHeal(healAmount);
            }

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

    System.Collections.IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    public void TakeHeal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;

        Debug.Log("Heal: " + currentHP);
    }

    void Die()
    {
        Debug.Log("Player Dead");
        gameObject.SetActive(false);
    }

    public void SetMaxHP(int newMaxHP, bool healFull = true)
    {
        int diff = newMaxHP - maxHP;

        maxHP = newMaxHP;

        if (healFull)
        {
            currentHP = maxHP; // เติมเต็ม
        }
        else
        {
            currentHP += diff; // เพิ่มตามส่วน
            if (currentHP > maxHP) currentHP = maxHP;
        }

        Debug.Log("HP Updated: " + currentHP + "/" + maxHP);
    }
}