using UnityEngine;

public static class CharacterUnlockSystem
{
    const string KEY_PREFIX = "CHAR_";
    public static System.Action OnUnlock;

    public static bool IsUnlocked(CharacterData data)
    {
        if (data.isDefault)
            return true;

        return PlayerPrefs.GetInt(KEY_PREFIX + data.characterID, 0) == 1;
    }

    public static void Unlock(string id)
    {
        PlayerPrefs.SetInt(KEY_PREFIX + id, 1);
        PlayerPrefs.Save();

        OnUnlock?.Invoke();
    }

    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("⚠ Reset All Unlocks");
    }
}