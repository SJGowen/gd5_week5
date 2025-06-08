using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    private int enemyCount;
    private int waveNumber = 0;
    private readonly static float halfIslandWidth = 20f;
    private readonly static float halfIslandHeight = Mathf.Sqrt((halfIslandWidth * halfIslandWidth) - ((halfIslandWidth / 2) * (halfIslandWidth / 2)));

    private readonly List<(float xPos, float yPos)> spawnPositions = new()
    {
        (halfIslandWidth / 2, halfIslandHeight),
        (halfIslandWidth, 0f),
        (halfIslandWidth / 2, -halfIslandHeight),
        (-halfIslandWidth / 2, -halfIslandHeight),
        (-halfIslandWidth, 0f),
        (-halfIslandWidth / 2, halfIslandHeight),
    };

    public PlayerController playerController;
    public GameObject[] enemies;
    public GameObject[] powerups;
    public GameObject[] bosses;
    public GameObject[] rewards;
    public int PowerupsAllowed = 9; // Maximum number of powerups allowed in the scene at once
    public int RewardsAllowed = 3; // Maximum number of rewards allowed in the scene at once

    void Start()
    {
        //Debug.Log($"Half Island Width: {halfIslandWidth}, Half Island Height: {halfIslandHeight}");
    }

    void Update()
    {
        // TODO: Manually count this instead of using FindObjectsByType, which is expensive.
        //enemyCount = FindObjectsByType<EnemyController>(FindObjectsSortMode.None).Length;

        if (playerController != null)
        {
            enemyCount = playerController.FoesCount + playerController.BossFoesCount;
        }

        if (enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber * 2);
            SpawnPowerups(waveNumber);
            SpawnRewards(waveNumber);
            if (waveNumber % 5 == 0)
            {
                SpawnBosses(waveNumber / 5);
            }
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        if (enemies.Length == 0) return; // No enemies to spawn

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Debug.Log($"Spawning enemy {i + 1} of {enemiesToSpawn} in wave {waveNumber}");
            Instantiate(enemies[Random.Range(0, enemies.Length)],
                RandomEnemySpawnPosition(), Quaternion.identity);
        }

        if (playerController != null) playerController.FoesCount += enemiesToSpawn;
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
        if (powerups.Length > 0)
        {
            int existingPowerups = GameObject.FindGameObjectsWithTag("Powerup").Length;

            while (existingPowerups <= Mathf.Min(waveNumber, PowerupsAllowed))
            {
                Instantiate(powerups[Random.Range(0, powerups.Length)],
                    RandomPowerupSpawnPosition(), Quaternion.identity);
            }
        }
    }

    private void SpawnBosses(int bossesToSpawn)
    {
        if (bosses.Length > 0)
        {
            for (int i = 0; i < bossesToSpawn; i++)
            {
                Instantiate(bosses[Random.Range(0, bosses.Length)],
                    RandomEnemySpawnPosition(1), Quaternion.identity);
            }

            if (playerController != null) playerController.BossFoesCount += bossesToSpawn;
        }
    }

    private void SpawnRewards(int itemsToSpawn)
    {
        if (rewards.Length > 0)
        {
            int existingRewards = GameObject.FindGameObjectsWithTag("Reward").Length;

            while (existingRewards <= Mathf.Min(waveNumber, RewardsAllowed))
            {
                Instantiate(rewards[Random.Range(0, rewards.Length)],
                    RandomPowerupSpawnPosition(), Quaternion.identity);
                existingRewards++;
            }
        }
    }

    Vector3 RandomPowerupSpawnPosition(float yPosition = 0)
    {
        int attempts = 0;
        float xPosition, zPosition;
        do
        {
            attempts++;
            xPosition = Random.Range(-halfIslandWidth, halfIslandWidth);
            zPosition = Random.Range(-halfIslandHeight, halfIslandHeight);
        } while (!IsPositionAboveIsland(xPosition, zPosition) && attempts <= 9);

        return new Vector3(xPosition, yPosition, zPosition);
    }

    private bool IsPositionAboveIsland(float xPosition, float zPosition)
    {
        RaycastHit hit;
        Vector3 origin = new Vector3(xPosition, 2.5f, zPosition);
        if (Physics.Raycast(origin, Vector3.down, out hit))
        {
            if (hit.collider.CompareTag("Island"))
            {
                return true;
            }
        }

        return false;
    }
}
