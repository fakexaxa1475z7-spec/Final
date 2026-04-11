using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float gameTime = 60f;

    public TextMeshProUGUI timerText; // 🔥 ใช้ TMP

    float currentTime;
    bool isGameOver = false;

    void Start()
    {
        currentTime = gameTime;
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
        isGameOver = true;

        FindObjectOfType<LevelUpManager>().OpenLevelUp();
    }
}