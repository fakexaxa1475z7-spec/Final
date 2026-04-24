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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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