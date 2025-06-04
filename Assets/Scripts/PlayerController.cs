using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] public Transform focalPoint;

    public bool hasPowerup;
    public float powerupStrength;
    public GameObject powerupIndicator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // The focal point is now dragged from the inspector to the player controller
        // you can also set it programmatically if needed as below:
        //focalPoint = GameObject.Find("FocalPoint").transform;
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

        rb.AddForce((focalPoint.forward * moveDirection.y + focalPoint.right * moveDirection.x) * playerSpeed);

        powerupIndicator.SetActive(hasPowerup);
        powerupIndicator.transform.position = transform.position;
        

        if (rb.transform.position.y < -10f)
        {
            // Reset the player's position if they fall below a certain height
            rb.transform.position = new Vector3(0f, 0f, 0f);
            // Make the reset player not move until the next input
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
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
            hasPowerup = true;
            StartCoroutine(PowerupCountdownRoutine(5));
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasPowerup && collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.transform.position - transform.position).normalized;
            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);

            //Debug.Log($"The player has collided with {collision.gameObject.name}");
        }
    }

    IEnumerator PowerupCountdownRoutine(int waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        hasPowerup = false;
    }
}
