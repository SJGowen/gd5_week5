using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody enemyRb;
    GameObject player;

    public float allowedStillTime = 4f;
    private float stillTime = 0f;
    private Vector3 lastPosition;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(speed * enemyRb.mass * lookDirection);

        bool destroyMe = EnemyIsStationary();
        // Update last position to the current position, used by EnemyIsStationary
        lastPosition = transform.position;

        if (destroyMe || enemyRb.transform.position.y < -10f)
        {
            // Debug.Log("Enemy destroyed: " + gameObject.name);
            Destroy(gameObject);
        }
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
