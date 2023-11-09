using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour, IDamageable
{
    Transform target;
    NavMeshAgent agent;

    Health health;

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);

        if(!health.IsAlive())
        {
            // VFX SFX & etc
            WolfPooler.Instance.Pool.Release(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GameObject house = GameObject.FindWithTag("House");
        if(house != null)
        {
            // The target of the wolf is the house
            target = house.transform;
        }
    }

    private void OnEnable()
    {
        health = new Health(3);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
    }

    public bool IsDead()
    {
        return !health.IsAlive();
    }

    public GameObject ThisObject()
    {
        return gameObject;
    }
}