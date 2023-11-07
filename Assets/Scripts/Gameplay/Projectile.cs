
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class Projectile : MonoBehaviour
{
    protected Rigidbody rigid;

    [SerializeField] protected float speed = 13.0f;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigid.MovePosition(rigid.position + transform.up * speed * Time.deltaTime);   
    }
}
