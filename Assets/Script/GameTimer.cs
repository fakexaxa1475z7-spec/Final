using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public bool isBossLevel = false; // 🔥 กำหนดใน Inspector

    public float gameTime = 60f;

    public TextMeshProUGUI timerText; // 🔥 ใช้ TMP

    float currentTime;
    bool isGameOver = false;

    int total = 0;

    void Start()
    {
        isGameOver = false;
        currentTime = gameTime;
        total = PlayerPrefs.GetInt("META_MONEY", 0);
    }

    void Update()
    {
        if (isGameOver) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            EndGame();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";

        // 🔴 ใกล้หมดเวลา
        if (currentTime <= 10f)
        {
            timerText.color = Color.red;
        }
    }

    void EndGame()
    {
        if (isBossLevel)
        {
            isGameOver = true;

            total += 100;
            PlayerPrefs.SetInt("META_MONEY", total);
            PlayerPrefs.Save();
            Destroy(GameObject.FindWithTag("Player"));
            EndGameData.instance.isWin = true;
            SceneManager.LoadScene("EndGame");
        }
        else
        {
             isGameOver = true;

            FindObjectOfType<LevelUpManager>().OpenLevelUp();
        }       
    }
}