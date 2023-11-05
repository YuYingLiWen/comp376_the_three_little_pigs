
using UnityEngine;

public class YuProjectile : MonoBehaviour
{

    Rigidbody rigid;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid.AddForce(Vector3.up);
    }
}
