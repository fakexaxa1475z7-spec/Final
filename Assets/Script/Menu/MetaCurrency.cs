using UnityEngine;

public class MetaCurrency : MonoBehaviour
{
    public static MetaCurrency instance;

    public int total = 0;

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
    }

    void Start()
    {
        total = PlayerPrefs.GetInt("META_MONEY", 0);
    }

    // ➕ เพิ่มเงินถาวร
    public void Add(int amount)
    {
        total += amount;
        Debug.Log("Total Money: " + total);
        PlayerPrefs.SetInt("META_MONEY", total);
        PlayerPrefs.Save();
    }

    // 💸 ใช้เงินถาวร
    public bool Spend(int amount)
    {
        if (total >= amount)
        {
            total -= amount;
            return true;
            PlayerPrefs.SetInt("META_MONEY", total);
            PlayerPrefs.Save();
        }
        return false;
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("META_MONEY", total);
        PlayerPrefs.Save();
    }
}