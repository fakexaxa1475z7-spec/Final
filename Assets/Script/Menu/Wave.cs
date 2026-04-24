using UnityEngine;
using TMPro;
public class Wave : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public int currentWave = 1;

    private void Awake()
    {
        waveText.text = "Wave " + currentWave;
    }
}
