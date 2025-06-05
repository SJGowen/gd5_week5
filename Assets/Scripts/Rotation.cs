using UnityEngine;

public class Rotation : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        gameObject.transform.Rotate(200 * Time.deltaTime * Vector3.up);
    }
}
