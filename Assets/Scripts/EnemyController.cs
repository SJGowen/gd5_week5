using UnityEngine;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody enemyRb;
    GameObject player;

    public float allowedStillTime = 3f;
    private float stillTime = 0f;
    private Vector3 lastPosition;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        // enemyRb.AddForce((player.transform.position - transform.position).normalized * speed);
        //Debug.Log($"Player position ({player.transform.position}), Speed ({speed})");
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);

        bool destroyMe = false;
        //Debug.Log($"Distance = {Vector3.Distance(transform.position, lastPosition)}");
        if ((transform.position - lastPosition).sqrMagnitude < 0.0001f)
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

        lastPosition = transform.position;

        if (destroyMe || enemyRb.transform.position.y < -10f) Destroy(gameObject);
    }
}
