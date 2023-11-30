using System.Collections;

using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour, IDamageable
{
    AudioSource audioS;
    [SerializeField] AudioClip attackSFX;
    [SerializeField] AudioClip onClickSFX;
    [SerializeField] AudioClip onTakeDamageSFX;
    public GameObject bloodSplatter;


    Transform target;
    NavMeshAgent agent;

    Health health;

    [SerializeField] int attackDamage = 1;

    [SerializeField] float distanceToTarget = 2.0f;

    Coroutine attackRoutine = null;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.stoppingDistance = distanceToTarget;

        audioS = GetComponent<AudioSource>();
    }

    void Start()
    {



    }

    private void OnEnable()
    {
        health = new Health(3);

        FindTargetHouse();
        GoToTarget();
    }

    void Update()
    {
        if(ReachedTarget())
        {
            if (attackRoutine == null)
            {
                attackRoutine = StartCoroutine(AttackRoutine());
            }
        }
    }


    void OnCollisionEnter(Collision other)
    {
        // chase pig for future update?
        Debug.Log("other: " + other.collider.name);
        //TakeDamage
    }


    bool ReachedTarget()
    {
        Vector3 direction = transform.position - target.position;
        if (direction.magnitude <= distanceToTarget) return true;
        else return false;
    }

    void FindTargetHouse()
    {
        GameObject house = GameObject.FindWithTag("House");
        if (house != null)
        {
            // The target of the wolf is the house
            target = house.transform;
        }
    }

    void GoToTarget()
    {
        agent.SetDestination(target.position);
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);

        if (!health.IsAlive())
        {
            // VFX SFX & etc
            GameObject blood = Instantiate(bloodSplatter, transform.position, transform.rotation);
            Destroy(blood, 1f); // destroy effect after 1 seconds

            WolfPooler.Instance.Pool.Release(this.gameObject);

            StopAllCoroutines();
            attackRoutine = null;
        }
    }

    public bool IsDead()
    {
        return !health.IsAlive();
    }

    public GameObject ThisObject()
    {
        return gameObject;
    }

    IEnumerator AttackRoutine()
    {
        WaitForSeconds waitFor = new WaitForSeconds(1.0f);

        while(true)
        {
            Debug.Log($"{name} is attacking.", gameObject);
            // Play Animation
            //audioS.PlayOneShot(attackSFX);
            Attack();
            yield return waitFor;
        }
    }

    private void Attack()
    {
        target.GetComponent<IDamageable>().TakeDamage(attackDamage);
        // kill the wolf
        InstantDeath();
    }

    public void InstantDeath()
    {
        TakeDamage(9999);
    }
}
