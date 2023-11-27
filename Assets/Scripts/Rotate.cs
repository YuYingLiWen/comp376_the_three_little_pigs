
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] Vector3 rotation;
    [SerializeField] float speed;

    void Update()
    {
        transform.Rotate(rotation * speed * Time.deltaTime, Space.World);  
    }
}
