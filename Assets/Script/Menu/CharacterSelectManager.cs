using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    public CharacterData selectedCharacter;

    public void SelectCharacter(CharacterData data)
    {
        selectedCharacter = data;

        // 🔥 เก็บไว้ข้าม scene
        PlayerPrefs.SetString("Character", data.name);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
}