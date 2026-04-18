using UnityEngine;
using System;

public class MetaCurrency : MonoBehaviour
{
    public static MetaCurrency instance;

    public int total = 0;
    public Action<int> OnMoneyChanged;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            total = PlayerPrefs.GetInt("META_MONEY", 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Add(int amount)
    {
        total += amount;
        Save();

        OnMoneyChanged?.Invoke(total);
    }

    public bool Spend(int amount)
    {
        if (total >= amount)
        {
            total -= amount;
            Save();

            OnMoneyChanged?.Invoke(total);
            return true;
        }

        Debug.Log("❌ Not enough money!");
        return false;
    }

    void Save()
    {
        PlayerPrefs.SetInt("META_MONEY", total);
        PlayerPrefs.Save();
    }
}