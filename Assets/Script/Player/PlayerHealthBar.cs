using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image fill;

    void Update()
    {
        if (PlayerHealth.instance == null) return;

        float hp = PlayerHealth.instance.GetHPPercent();
        fill.fillAmount = hp;
    }
}