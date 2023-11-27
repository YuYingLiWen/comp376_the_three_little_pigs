using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PlayerUnit : MonoBehaviour, IInteractable
{
    NavMeshAgent navMeshAgent;
    public GameObject targetTree, targetStoneMine, targetTower;
    bool carryingWood = false;
    bool carryingStone = false;
    Vector3 housePos = Vector3.zero;
    GameObject house;
    AudioSource audioSource;
    public AudioClip woodDepositClip, stoneDepositClip, woodChopClip, stoneMiningClip;

    [SerializeField] Transform night_fov;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
    }

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

        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            Debug.LogWarning("Didn't find an audio source for the player unit!");
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

                audioSource.clip = woodChopClip;
                audioSource.Play();

                this.SetDestination(housePos);

                // destroy tree object
                Destroy(targetTree);

                // Spawn new tree sp that num of trees is always same
                GameObject envGameObject = GameObject.Find("Environment");
                if (envGameObject != null)
                {
                    // Access the ScriptA component and call the public function
                    SpawnEnvironment scriptSpawnEnv = envGameObject.GetComponent<SpawnEnvironment>();
                    if (scriptSpawnEnv != null)
                    {
                        scriptSpawnEnv.SpawnOneTree(); // actually spawn 1 tree
                    }
                    else
                    {
                        Debug.LogError("No SpawnEnvironment component on the Environement GameObject.");
                    }
                }
                else
                {
                    Debug.LogError("Didn't find object called Environment! (Needed for spawning resources)");
                }
            }
        }

        
        if (targetStoneMine != null)
        {
            if (Mathf.Abs(transform.position.x - targetStoneMine.transform.position.x) < 0.01f && Mathf.Abs(transform.position.y - targetStoneMine.transform.position.y) < 0.01f)
            {
                // pig is at the stone mine, now pig turns back towards house to deposit the stone
                carryingStone = true;

                audioSource.clip = stoneMiningClip;
                audioSource.Play();

                this.SetDestination(housePos);
            }
        }

        if (targetTower != null)
        {
            if (Mathf.Abs(transform.position.x - targetTower.transform.position.x) < 0.01f && Mathf.Abs(transform.position.y - targetTower.transform.position.y) < 0.01f)
            {
                // pig is at the tower
                Debug.Log("Pig is at tower, garrisoning!");
                targetTower.GetComponent<Towers>().Garrison(this.gameObject);
                targetTower = null;
            }
        }


        if (carryingWood)
        {
            // deposit wood at house
            if (Mathf.Abs(transform.position.x - housePos.x) < GetHouseSize().x/2 && Mathf.Abs(transform.position.y - housePos.y) < GetHouseSize().y/2)
            {
                carryingWood = false;
                // Debug.Log("Wood depositied! Update ressources here (like +10 wood)");

                audioSource.clip = woodDepositClip;
                audioSource.Play();

                // update ressources 
                LevelManager.Instance.AddWood(10);
            }
        } else if (carryingStone)
        {
            // deposit stone at house
            if (Mathf.Abs(transform.position.x - housePos.x) < GetHouseSize().x/2 && Mathf.Abs(transform.position.y - housePos.y) < GetHouseSize().y/2)
            {
                carryingStone = false;

                // Stone deposit sound
                audioSource.clip = stoneDepositClip;
                audioSource.Play();

                // update ressources 
                LevelManager.Instance.AddStone(10);
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
        // orient the pig in the direction he's going
        Vector3 direction = location - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(0f, 0f, angle-90f);

        navMeshAgent.SetDestination(location);
    }

    public void OnClick()
    {
        OverlayUIController.Instance.DisplayBuildMenu(true);

        // Add circle around player to indicate selection
        Transform circleTransform = transform.Find("CircleSelect");
        if (circleTransform != null) {
            circleTransform.gameObject.SetActive(true);
        } else {
            Debug.LogError("Child object 'CircleSelect' not found.");
        }

        // pig sound here
    }
    public void Deselect()
    {
        OverlayUIController.Instance.DisplayBuildMenu(false);
        
        Transform circleTransform = transform.Find("CircleSelect");

        if (circleTransform != null) {
            circleTransform.gameObject.SetActive(false);
        } else {
            Debug.LogError("Child object 'CircleSelect' not found.");
        }

    }
    // Change the color of a selected object
    private void ChangeObjectColor(Color color)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }
}
