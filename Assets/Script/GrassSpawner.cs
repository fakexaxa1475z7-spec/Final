using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    public GameObject[] grassPrefabs; // 🌿 หลายแบบ
    public int amount = 200;

    public Vector2 mapSize = new Vector2(20, 20);

    public Transform parent; // optional

    void Start()
    {
        SpawnGrass();
    }

    void SpawnGrass()
    {
        for (int i = 0; i < amount; i++)
        {
            // 🔥 สุ่มตำแหน่ง
            float x = Random.Range(-mapSize.x / 2f, mapSize.x / 2f);
            float y = Random.Range(-mapSize.y / 2f, mapSize.y / 2f);

            Vector3 pos = new Vector3(x, y, 0);

            // 🔥 สุ่ม prefab
            GameObject prefab = grassPrefabs[Random.Range(0, grassPrefabs.Length)];

            GameObject g = Instantiate(prefab, pos, Quaternion.identity, parent);

            // 🔥 สุ่มขนาด
            float scale = Random.Range(0.8f, 1.3f);
            g.transform.localScale = Vector3.one * scale;

            // 🔥 สุ่มหมุน
            g.transform.rotation = Quaternion.identity;
        }
    }
}