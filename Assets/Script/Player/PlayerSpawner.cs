using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
        SpawnOrMove();
    }

    void SpawnOrMove()
    {
        // 🔥 หา player เดิมก่อน
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // ✅ มีอยู่แล้ว → แค่ย้ายตำแหน่ง
            player.transform.position = spawnPoint.position;

            Debug.Log("✔ Reuse Player");

            return;
        }

        // ❌ ไม่มี → ค่อย spawn ใหม่
        if (PlayerStats.selectedCharacter == null)
        {
            Debug.LogError("❌ No character selected");
            return;
        }

        var prefab = PlayerStats.selectedCharacter.prefab;

        if (prefab == null)
        {
            Debug.LogError("❌ Character prefab missing");
            return;
        }

        player = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        player.tag = "Player";

        Debug.Log("🔥 Spawn New Player");

        // 🔫 ผูกปืน
        var weapon = PlayerWeapon.instance;

        if (weapon != null)
        {
            weapon.transform.SetParent(player.transform);
            weapon.transform.localPosition = Vector3.zero;
        }
    }
}