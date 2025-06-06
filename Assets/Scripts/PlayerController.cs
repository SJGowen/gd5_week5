using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private string powerupName;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] public Transform focalPoint;

    public bool hasPowerup;
    public float powerupStrength;
    public GameObject powerupIndicator;
    public GameObject projectilePrefab;
    public int projectileSpeed = 50;
    public float jumpForce = 8f;

    private int livesCount = 3;
    [SerializeField]
    public TextMeshProUGUI livesCountGUI;

    public int LivesCount
    {
        get { return livesCount; }
        set
        {
            livesCount = value;
            livesCountGUI.text = $"Lives = {livesCount}";
        }
    }

    private int gemsCount;
    [SerializeField]
    public TextMeshProUGUI gemsCountGUI;

    public int GemsCount
    {
        get { return gemsCount; }
        set
        {
            gemsCount = value;
            gemsCountGUI.text = $"Gems = {gemsCount}";
        }
    }

    private int starsCount;
    [SerializeField]
    public TextMeshProUGUI starsCountGUI;

    public int StarsCount
    {
        get { return starsCount; }
        set
        {
            starsCount = value;
            starsCountGUI.text = $"Stars = {starsCount}";
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

        rb.AddForce((focalPoint.forward * moveDirection.y + focalPoint.right * moveDirection.x) * playerSpeed);

        powerupIndicator.SetActive(hasPowerup);
        powerupIndicator.transform.position = transform.position;
        powerupIndicator.transform.Rotate(200 * Time.deltaTime * Vector3.up);

        if (hasPowerup)
        {
            if (powerupName.StartsWith("FireIcon")) FireProjectilesAtEnemies();
            if (powerupName.StartsWith("GoldCupIcon")) AddExtraLife(); 
            if (powerupName.StartsWith("MultiplierIcon")) KillAllEnemies();
            if (powerupName.StartsWith("PowerIcon")) CauseExplosion();
        }

        if (rb.transform.position.y < -10f)
        {
            // Reset the player's position if they fall below a certain height
            rb.transform.position = new Vector3(0f, 0f, 0f);
            // Make the reset player not move until the next input
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            LivesCount--;
            if (LivesCount == 0)
            {
                // Game over swicth scenes
            }
        }
    }

    private void FireProjectilesAtEnemies()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.mass *= 5;
            try
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    ShootProjectile(enemy.transform.position);
                }
            }
            finally
            {
                rb.mass /= 5;
            }
        }
    }

    private void ShootProjectile(Vector3 targePosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = (targePosition - transform.position).normalized;
            rb.linearVelocity = direction * projectileSpeed;
            Destroy(projectile, 4);
        }
    }

    private void AddExtraLife()
    {
        LivesCount++;
        hasPowerup = false;
    }

    private void KillAllEnemies()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
            hasPowerup = false;
        }
    }

    private void CauseExplosion()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 12f);
            foreach (Collider collider in colliders)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null && rb.name != "Player")
                {
                    rb.AddExplosionForce(400f, transform.position, 12f);
                }
            }

            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            hasPowerup = false;
        }
    }

    private void LateUpdate()
    {
        focalPoint.position = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            if (!hasPowerup)
            {
                hasPowerup = true;
                powerupName = other.name;
                StartCoroutine(PowerupCountdownRoutine(5));
            }
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Reward"))
        {
            if (other.name == "Gem_01") GemsCount++;
            if (other.name == "Star_01") StarsCount++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasPowerup && collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.transform.position - transform.position).normalized;
            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            // Debug.Log($"The player has collided with {collision.gameObject.name}");
        }
    }

    IEnumerator PowerupCountdownRoutine(int waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        hasPowerup = false;
    }
}
