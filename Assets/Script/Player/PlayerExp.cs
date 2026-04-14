using UnityEngine;

public class PlayerExp : MonoBehaviour
{
    public static PlayerExp instance;

    public int currentExp = 0;

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
        ResetExp();
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
    }

    public bool SpendExp(int amount)
    {
        if (currentExp >= amount)
        {
            currentExp -= amount;
            return true;
        }
        return false;
    }

    public void ResetExp()
    {
        currentExp = 0;
    }
}