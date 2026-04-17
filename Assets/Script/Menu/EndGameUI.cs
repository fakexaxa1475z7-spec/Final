using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI expText;

    void Start()
    {
        var data = EndGameData.instance;

        titleText.text = data.isWin ? "VICTORY!" : "GAME OVER";
        expText.text = "EXP: " + data.exp;
    }

    public void GoShop()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public void GoMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}