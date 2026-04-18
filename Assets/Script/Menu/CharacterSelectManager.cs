using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager instance;

    public string gameSceneName = "Level1";
    public CharacterData currentSelected;

    public List<CharacterButton> buttons = new List<CharacterButton>();

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LoadSelected();
        RefreshAll();
    }

    public void RegisterButton(CharacterButton btn)
    {
        if (!buttons.Contains(btn))
            buttons.Add(btn);
    }

    public void SelectCharacter(CharacterData data)
    {
        currentSelected = data;

        PlayerPrefs.SetString("Character", data.characterID);
        PlayerPrefs.Save();

        RefreshAll();

        Debug.Log("Selected: " + data.characterName);
    }

    void LoadSelected()
    {
        string id = PlayerPrefs.GetString("Character", "");

        if (!string.IsNullOrEmpty(id))
        {
            CharacterData[] all = Resources.LoadAll<CharacterData>("");

            foreach (var c in all)
            {
                if (c.characterID == id)
                {
                    currentSelected = c;
                    return;
                }
            }
        }
    }

    public void RefreshAll()
    {
        foreach (var btn in buttons)
        {
            btn.UpdateUI();
            btn.UpdateSelection();
        }
    }

    public void StartGame()
    {
        if (currentSelected == null)
        {
            Debug.Log("❌ No character selected!");
            return;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
}