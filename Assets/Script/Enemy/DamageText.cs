using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float lifeTime = 1f;

    TextMeshPro text;
    Color color;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        color = text.color;
    }

    public void Setup(int dmg, bool isCrit)
    {
        text.text = dmg.ToString();

        if (isCrit)
        {
            text.color = Color.yellow;
            text.fontSize = 8;
        }
        else
        {
            text.color = Color.white;
            text.fontSize = 5;
        }
    }

    void Update()
    {
        // ลอยขึ้น
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // fade
        color.a -= Time.deltaTime / lifeTime;
        text.color = color;

        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
    void LateUpdate()
    {
        if (Camera.main != null)
            transform.forward = Camera.main.transform.forward;
    }
}