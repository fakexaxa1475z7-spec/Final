using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSaveManager : MonoBehaviour
{
    public string menuScene = "Menu"; // 🔥 ชื่อ scene หน้าแรก

    public void ResetSave()
    {
        Debug.Log("⚠ Reset ALL Save!");

        // 🧹 ลบทุก PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // 💰 รีเซ็ตเงิน (กัน instance ค้าง)
        if (MetaCurrency.instance != null)
        {
            MetaCurrency.instance.total = 0;
        }

        // 🎮 รีเซ็ตตัวละคร
        PlayerStats.selectedCharacter = null;

        // 🔄 โหลด scene ใหม่
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }
}