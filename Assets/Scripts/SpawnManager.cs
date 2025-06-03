using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemies;
    [SerializeField] private float xPositionRange = 20;
    [SerializeField] private float zPositionRange = 30;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (enemies.Length == 0)
            {
                Debug.Log("enemies.Length == 0, in SpawnManager!");
                return;
            }
            GameObject enemy = enemies[Random.Range(0, enemies.Length)];
            Vector3 spawnPosition = randomSpawnPosition();
            Instantiate(enemy, spawnPosition, Quaternion.identity);
        }
    }

    Vector3 randomSpawnPosition(float yPosition = 0)
    {
        return new Vector3(
            Random.Range(-xPositionRange, xPositionRange), yPosition,
            Random.Range(-zPositionRange, zPositionRange));
    }
}
