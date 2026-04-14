using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public CharacterData data;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public GameObject lockIcon;
    public Button button;

    public MetaCurrency exp; // ใช้ EXP ซื้อ

    void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        nameText.text = data.characterName;

        if (CharacterUnlockSystem.IsUnlocked(data.characterID))
        {
            lockIcon.SetActive(false);
            costText.text = "";
            button.interactable = true;
        }
        else
        {
            lockIcon.SetActive(true);
            costText.text = "Unlock: " + data.unlockCost;
        }
    }

    public void OnClick()
    {
        if (CharacterUnlockSystem.IsUnlocked(data.characterID))
        {
            // ✅ เลือกตัวละคร
            PlayerStats.selectedCharacter = data;

            // 🔥 เพิ่มบรรทัดนี้
            if (PlayerStats.instance != null)
                PlayerStats.instance.ApplyCharacter();

            Debug.Log("Selected: " + data.characterName);
        }
        else
        {
            if (exp.Spend(data.unlockCost))
            {
                CharacterUnlockSystem.Unlock(data.characterID);
                UpdateUI();
            }
            else
            {
                Debug.Log("Not enough EXP!");
            }
        }
    }
}