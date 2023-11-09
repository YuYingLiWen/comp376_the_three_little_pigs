
using UnityEngine;

public class YuTC : MonoBehaviour
{
    [SerializeField] Transform night_fov;

    void Update()
    {
        night_fov.position = Camera.main.WorldToScreenPoint(transform.position);
    }
}
