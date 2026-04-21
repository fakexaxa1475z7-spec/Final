using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyType
{
    public GameObject prefab;
    public int weight = 1;
}

public class Spawner : MonoBehaviour
{
    [Header("Enemy List")]
    public List<EnemyType> enemies;
    public GameObject bossPrefab;

    [Header("Spawn Settings")]
    public float spawnRate = 2f;
    public int spawnAmount = 1;

    [Header("Map Area (Rectangle)")]
    public Vector2 mapCenter = Vector2.zero;
    public Vector2 mapSize = new Vector2(40, 40); // กว้าง x สูง

    [Header("Player Safety")]
    public Transform player;
    public float minDistanceFromPlayer = 5f;

    [Header("Spawn Mode")]
    public bool spawnOnEdge = false; // true = spawn ขอบแมพ

    float timer;

    void Start()
    {
        if (GameTimer.isBossLevel)
        {
            Instantiate(bossPrefab, GetSpawnPosition(), Quaternion.identity);
        }
    }

    void FindPlayer()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }
    void Update()
    {
        FindPlayer(); // 🔥 กัน null

        if (player == null) return; // ❗ กัน error

        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            for (int i = 0; i < spawnAmount; i++)
                Spawn();

            timer = 0f;
        }
    }

    void Spawn()
    {
        GameObject enemy = GetRandomEnemy();
        Vector2 pos = GetSpawnPosition();

        Instantiate(enemy, pos, Quaternion.identity);
    }

    // 🔀 random enemy (weight)
    GameObject GetRandomEnemy()
    {
        int totalWeight = 0;
        foreach (var e in enemies)
            totalWeight += e.weight;

        int rand = Random.Range(0, totalWeight);

        foreach (var e in enemies)
        {
            if (rand < e.weight)
                return e.prefab;

            rand -= e.weight;
        }

        return enemies[0].prefab;
    }

    // 📍 spawn position
    Vector2 GetSpawnPosition()
    {
        Vector2 pos;
        int attempts = 0;

        do
        {
            if (spawnOnEdge)
                pos = GetRandomPointOnEdge();
            else
                pos = GetRandomPointInside();

            attempts++;

            if (attempts > 20) break; // 🔥 กันค้าง
        }
        while (Vector2.Distance(pos, player.position) < minDistanceFromPlayer);

        return pos;
    }

    // 🔲 ในสี่เหลี่ยม
    Vector2 GetRandomPointInside()
    {
        float halfX = mapSize.x / 2f;
        float halfY = mapSize.y / 2f;

        float x = Random.Range(-halfX, halfX);
        float y = Random.Range(-halfY, halfY);

        return mapCenter + new Vector2(x, y);
    }

    // 🟥 ขอบสี่เหลี่ยม
    Vector2 GetRandomPointOnEdge()
    {
        float halfX = mapSize.x / 2f;
        float halfY = mapSize.y / 2f;

        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: // top
                return mapCenter + new Vector2(Random.Range(-halfX, halfX), halfY);
            case 1: // bottom
                return mapCenter + new Vector2(Random.Range(-halfX, halfX), -halfY);
            case 2: // left
                return mapCenter + new Vector2(-halfX, Random.Range(-halfY, halfY));
            default: // right
                return mapCenter + new Vector2(halfX, Random.Range(-halfY, halfY));
        }
    }

    // 🟩 Gizmos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(mapCenter, mapSize);
    }
}