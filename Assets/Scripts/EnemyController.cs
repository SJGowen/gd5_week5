using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody enebyRb;
    GameObject player;

    void Start()
    {
        enebyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        // enebyRb.AddForce((player.transform.position - transform.position).normalized * speed);
        Debug.Log($"Player position ({player.transform.position}), Speed ({speed})");
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enebyRb.AddForce(lookDirection * speed);
    }
}
