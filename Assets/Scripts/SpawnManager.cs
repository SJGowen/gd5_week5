using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject[] powerups;

    readonly List<(float xPos, float yPos)> spawnPositions = new()
    {
        (10f, 17.32f),
        (20f, 0f),
        (10f, -17.32f),
        (-10f, -17.32f),
        (-20f, 0f),
        (-10f, 17.32f),
    };

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (enemies.Length > 0)
            {
                GameObject enemy = enemies[Random.Range(0, enemies.Length)];
                Vector3 spawnPosition = randomSpawnPosition();
                Instantiate(enemy, spawnPosition, Quaternion.identity);
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (powerups.Length > 0)
            {
                GameObject powerup = powerups[Random.Range(0, powerups.Length)];
                Vector3 spawnPosition = randomSpawnPosition();
                Instantiate(powerup, spawnPosition, Quaternion.identity);
            }
        }
    }

    Vector3 randomSpawnPosition(float yPosition = 0)
    {
        int index = Random.Range(0, 6);
        return new Vector3(spawnPositions[index].xPos, yPosition, spawnPositions[index].yPos);
    }
}
