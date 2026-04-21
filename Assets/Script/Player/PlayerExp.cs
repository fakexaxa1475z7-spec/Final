using UnityEngine;
using System;

public class PlayerExp : MonoBehaviour
{
    public static PlayerExp instance;

    public int currentExp = 0;

    public Action<int> OnExpChanged; // 🔥 เพิ่มบรรทัดนี้

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

    public void AddExp(int amount)
    {
        currentExp += amount;
        OnExpChanged?.Invoke(currentExp); // 🔥 ยิง event
    }

    public bool SpendExp(int amount)
    {
        if (currentExp >= amount)
        {
            currentExp -= amount;
            OnExpChanged?.Invoke(currentExp); // 🔥 ยิง event
            return true;
        }
        return false;
    }

    public void ResetExp()
    {
        currentExp = 0;
        OnExpChanged?.Invoke(currentExp); // 🔥 ยิง event
    }
}