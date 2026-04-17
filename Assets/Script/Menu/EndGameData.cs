using UnityEngine;

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
            DontDestroyOnLoad(gameObject); // 🔥 ข้าม scene
        }
        else
        {
            Destroy(gameObject);
        }
        exp = 100;
    }
}