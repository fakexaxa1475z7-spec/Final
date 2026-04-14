using UnityEngine;

public static class CharacterUnlockSystem
{
    public static bool IsUnlocked(string id)
    {
        return PlayerPrefs.GetInt("CHAR_" + id, id == "C_Gunner" ? 1 : 0) == 1;
        // 🔥 Gunner เปิดตั้งแต่แรก
    }

    public static void Unlock(string id)
    {
        PlayerPrefs.SetInt("CHAR_" + id, 1);
        PlayerPrefs.Save();
    }
}