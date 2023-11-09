using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEditor.ShaderKeywordFilter;

public class PlayerUnit : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    public GameObject targetTree;// = Vector3.zero;
    bool carryingWood = false;
    Vector3 housePos = Vector3.zero;
    GameObject house;

    [SerializeField] Transform night_fov;

    void Start()
    {
        // By default, 2D sprites get tilted 90 degrees upon pressing play if they have a NavMeshAgent 
        // attached, making it invisible. So this is to prevent that from happening.
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        house = GameObject.FindWithTag("House");
        if (house != null)
        {
            housePos = house.transform.position;
        } else
        {
            Debug.Log("Error: No object with tag house has been found! Needed for ressource deposition.");
        }
    }

    void Update()
    {
        night_fov.position = Camera.main.WorldToScreenPoint(transform.position);

        if (targetTree != null)
        {
            //Debug.Log("transform.position: " + transform.position + "targetTree.transform.position: " + targetTree.transform.position);
            if(Mathf.Abs(transform.position.x - targetTree.transform.position.x) < 0.001f && Mathf.Abs(transform.position.y - targetTree.transform.position.y) < 0.001f)
            {
                // pig is at the tree, now pig turns back towards house to deposit the wood
                carryingWood = true;

                this.SetDestination(housePos);

                // destroy tree object
                Destroy(targetTree);
            }
        }

        if(carryingWood)
        {

            if (Mathf.Abs(transform.position.x - housePos.x) < GetHouseSize().x && Mathf.Abs(transform.position.y - housePos.y) < GetHouseSize().y)
            {
                carryingWood = false;
                Debug.Log("Wood depositied! Update ressources here (like +10 wood)");

                // update ressources and related UI here

            }
        }
    }

    Vector2 GetHouseSize()
    {
        Renderer objectRenderer = house.GetComponent<Renderer>();
        Vector2 houseSize = Vector2.zero;

        if (objectRenderer != null)
        {
            Bounds objectBounds = objectRenderer.bounds;
            Vector3 objectSize = objectBounds.size;

            //Debug.Log("Object Size (X, Y, Z): " + objectSize.x + ", " + objectSize.y + ", " + objectSize.z);
            houseSize = new Vector2(objectSize.x, objectSize.y);
        }
        else
        {
            Debug.Log("Error: No Renderer component found for the house object.");
        }

        return houseSize;
    }

    public void SetDestination(Vector3 location)
    {
        navMeshAgent.SetDestination(location);
    }

}
