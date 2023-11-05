using UnityEngine;
using System.Collections;

public class Marker : MonoBehaviour
{
    // This function is called as an animation event in the animation of the marker
    void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
