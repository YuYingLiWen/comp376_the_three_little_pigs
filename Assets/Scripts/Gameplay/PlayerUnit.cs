using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PlayerUnit : MonoBehaviour
{
    NavMeshAgent navMeshAgent;

    void Start()
    {
        // By default, 2D sprites get tilted 90 degrees upon pressing play if they have a NavMeshAgent 
        // attached, making it invisible. So this is to prevent that from happening.
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    public void SetDestination(Vector3 location)
    {
        navMeshAgent.SetDestination(location);
    }
}
