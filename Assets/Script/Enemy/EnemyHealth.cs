using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int hp = 3;
    public GameObject expPrefab;

    bool isDead = false;
    SpriteRenderer sr;

    public GameObject damageTextPrefab;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int dmg, bool isCrit = false)
    {
        if (isDead) return;

        hp -= dmg;

        // 🔥 spawn damage text
        if (damageTextPrefab != null)
        {
            Vector3 pos = transform.position + Vector3.up; // ลอยเหนือหัว

            GameObject txt = Instantiate(damageTextPrefab, pos, Quaternion.identity);

            DamageText dt = txt.GetComponent<DamageText>();
            if (dt != null)
            {
                dt.Setup(dmg, isCrit);
            }
        }

        StartCoroutine(Flash(isCrit));

        if (hp <= 0)
        {
            Die();
        }
    }

    IEnumerator Flash(bool isCrit)
    {
        if (sr == null) yield break;

        // 🔥 คริ = สีเหลือง / ปกติ = แดง
        sr.color = isCrit ? Color.yellow : Color.red;

        yield return new WaitForSeconds(0.1f);

        sr.color = Color.white;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // 💎 ดรอป EXP
        if (expPrefab != null)
        {
            Instantiate(expPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}