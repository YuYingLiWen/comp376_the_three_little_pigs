
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigid.MovePosition(rigid.position + transform.up * 13.0f * Time.deltaTime);   
    }
}
