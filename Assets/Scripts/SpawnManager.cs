using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    private int enemyCount;
    private int waveNumber = 1;
    private readonly static float halfIslandWidth = 20f;
    private readonly static float halfIslandHeight = Mathf.Sqrt((halfIslandWidth * halfIslandWidth) + ((halfIslandWidth / 2) * (halfIslandWidth /2)));

    private readonly List<(float xPos, float yPos)> spawnPositions = new()
    {
        (halfIslandWidth / 2, halfIslandHeight),
        (halfIslandWidth, 0f),
        (halfIslandWidth / 2, -halfIslandHeight),
        (-halfIslandWidth / 2, -halfIslandHeight),
        (-halfIslandWidth, 0f),
        (-halfIslandWidth / 2, halfIslandHeight),
    };

    public GameObject[] enemies;
    public GameObject[] powerups;
    public GameObject[] bosses;
    public GameObject[] rewards;
    public int PowerupFlagsAllowed = 9; // Maximum number of powerups allowed in the scene at once

    void Start()
    {

    }

    void Update()
    {
        // TODO: Manually count this instead of using FindObjectsByType, which is expensive.
        enemyCount = FindObjectsByType<EnemyController>(FindObjectsSortMode.None).Length;

        if (enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber * 2);
            SpawnPowerups(waveNumber);
            if (waveNumber % 5 == 0)
            {
                SpawnBossesAndRewards(waveNumber / 5);
            }
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        if (enemies.Length == 0) return; // No enemies to spawn

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], 
                RandomEnemySpawnPosition(), Quaternion.identity);
        }
    }


    Vector3 RandomEnemySpawnPosition(float yPosition = 0)
    {
        int index = Random.Range(0, 6);
        return new Vector3(
            spawnPositions[index].xPos, 
            yPosition, 
            spawnPositions[index].yPos);
    }

    void SpawnPowerups(int waveNumber)
    {
        if (powerups.Length == 0) return; // No powerups to spawn
        if (FindObjectsByType<Rotation>(FindObjectsSortMode.None).Length >= PowerupFlagsAllowed) return;

        for (int i = 0; i < waveNumber; i++)
        {
            Instantiate(powerups[Random.Range(0, powerups.Length)],
                RandomPowerupSpawnPosition(), Quaternion.identity);
        }
    }

    Vector3 RandomPowerupSpawnPosition(float yPosition = 0)
    {
        // This can return a position outside the island, but that is part of challenge.
        return new Vector3(
            Random.Range(-halfIslandWidth, halfIslandWidth),
            yPosition,
            Random.Range(-halfIslandHeight, halfIslandHeight)
        );
    }

    private void SpawnBossesAndRewards(int itemsToSpawn)
    {
        if (bosses.Length > 0)
        {
            for (int i = 0; i < itemsToSpawn; i++)
            {
                Instantiate(bosses[Random.Range(0, bosses.Length)],
                    RandomEnemySpawnPosition(1), Quaternion.identity);

            }
        }

        if (rewards.Length > 0)
        {
            for (int i = 0; i < itemsToSpawn; i++)
            {
                Instantiate(rewards[Random.Range(0, rewards.Length)],
                    RandomPowerupSpawnPosition(1), Quaternion.identity);
            }
        }
    }
}
