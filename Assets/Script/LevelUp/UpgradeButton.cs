using UnityEngine;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public TextMeshProUGUI text;

    UpgradeData current;
    LevelUpManager manager;

    public void SetUpgrade(UpgradeData up, LevelUpManager m)
    {
        current = up;
        manager = m;

        text.text = up.upgradeName;
    }

    public void OnClick()
    {
        manager.ChooseUpgrade(current);
    }
}