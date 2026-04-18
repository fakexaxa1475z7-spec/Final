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
    public Image bg;

    CharacterSelectManager manager;

    void Start()
    {
        manager = CharacterSelectManager.instance;

        manager.RegisterButton(this);

        // 🔥 กัน lock icon บังคลิก
        if (lockIcon.TryGetComponent<Image>(out var img))
            img.raycastTarget = false;

        UpdateUI();
        UpdateSelection();

        MetaCurrency.instance.OnMoneyChanged += OnMoneyChanged;
        CharacterUnlockSystem.OnUnlock += OnUnlock;
    }

    void OnDestroy()
    {
        MetaCurrency.instance.OnMoneyChanged -= OnMoneyChanged;
        CharacterUnlockSystem.OnUnlock -= OnUnlock;
    }

    void OnMoneyChanged(int _)
    {
        UpdateUI();
    }

    void OnUnlock()
    {
        UpdateUI();
    }

    public void UpdateSelection()
    {
        if (manager.currentSelected == data)
            bg.color = Color.green;
        else
            bg.color = Color.white;
    }

    public void UpdateUI()
    {
        nameText.text = data.characterName;

        bool unlocked = CharacterUnlockSystem.IsUnlocked(data);

        if (unlocked)
        {
            lockIcon.SetActive(false);
            costText.text = "SELECT";
            button.interactable = true;
        }
        else
        {
            lockIcon.SetActive(true);
            costText.text = "Unlock: " + data.unlockCost;
            button.interactable = MetaCurrency.instance.total >= data.unlockCost;
        }
    }

    public void OnClick()
    {
        bool unlocked = CharacterUnlockSystem.IsUnlocked(data);

        if (unlocked)
        {
            manager.SelectCharacter(data);
        }
        else
        {
            if (MetaCurrency.instance.Spend(data.unlockCost))
            {
                CharacterUnlockSystem.Unlock(data.characterID);

                // 🔥 unlock แล้วเลือกทันที
                manager.SelectCharacter(data);
            }
        }
    }
}