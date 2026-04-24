using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameData : MonoBehaviour
{
    public static EndGameData instance;

    public int exp;
    public bool isWin;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }       
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isWin)
        {
            exp = 100;
        }
        else
        {
            exp = 1;
        }
    }
}