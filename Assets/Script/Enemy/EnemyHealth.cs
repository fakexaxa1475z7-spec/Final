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
    public Animator anim;

    public bool isBoss = false;

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

        // 🔥 เล่น animation
        if (anim != null)
        {
            anim.SetTrigger("Die");
        }

        // 🔥 ปิด collider กันโดนซ้ำ
        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // 🔥 หยุดการเคลื่อนที่
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // 🔥 delay แล้วค่อยหาย
        StartCoroutine(DieDelay());
    }
    IEnumerator DieDelay()
    {
        yield return new WaitForSeconds(0.5f); // ⏳ ให้ animation เล่น

        if (isBoss)
        {
            int total = PlayerPrefs.GetInt("META_MONEY", 0);
            total += 100;

            PlayerPrefs.SetInt("META_MONEY", total);
            PlayerPrefs.Save();

            Destroy(GameObject.FindWithTag("Player"));

            EndGameData.instance.isWin = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndGame");
        }
        else
        {
            if (expPrefab != null)
            {
                Instantiate(expPrefab, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }
}