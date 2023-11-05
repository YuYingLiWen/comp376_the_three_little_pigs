using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;

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

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
    }
}