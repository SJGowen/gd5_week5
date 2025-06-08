using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody enemyRb;
    private GameObject player;
    private PlayerController playerController;
    private float speed = 5f;
    private float allowedStillTime = 5f;
    private float stillTime = 0f;
    private Vector3 lastPosition;
    private GameObject projectilePrefab;
    private float projectileSpeed = 40;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.Log("Can't get a reference to Player Controller within Enemy Controller.");
        };
        projectilePrefab = playerController.projectilePrefab;
        GetBossesToShootAtPlayer();
    }

    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(speed * enemyRb.mass * lookDirection);

        bool destroyMe = EnemyIsStationary();
        // Update last position to the current position, used by EnemyIsStationary
        lastPosition = transform.position;

        // Destroy Enemies that have fallen off of the island
        if (destroyMe || enemyRb.transform.position.y < -10f)
        {
            if (gameObject.tag == "Enemy") playerController.FoesCount -= 1;
            if (gameObject.tag == "BossEnemy") playerController.BossFoesCount -= 1;

            // Debug.Log("Enemy destroyed: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    private void GetBossesToShootAtPlayer()
    {
        if (gameObject.tag == "BossEnemy")
        {
            ScheduleNextInvoke(2f);
        }
    }

    private void ScheduleNextInvoke(float delay)
    {
        Invoke(nameof(FireProjectileAtPlayer), delay);
    }

    private void FireProjectileAtPlayer()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = (playerController.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * projectileSpeed;
            Destroy(projectile, 4);
        }
        
        ScheduleNextInvoke(2f);
    }

    private bool EnemyIsStationary()
    {
        bool destroyMe = false;

        if ((transform.position - lastPosition).sqrMagnitude < 0.001f)
        {
            stillTime += Time.deltaTime;
            if (stillTime >= allowedStillTime)
            {
                destroyMe = true;
            }
        }
        else
        {
            stillTime = 0f;
        }

        return destroyMe;
    }
}
