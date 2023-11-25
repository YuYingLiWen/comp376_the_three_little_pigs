using UnityEngine;

public class Build : MonoBehaviour
{
    public GameObject tower_blueprint_prefab;

    [SerializeField] int stoneCost;
    [SerializeField] int woodCost;



    LevelManager levelManager;
    private void Awake()
    {
        if (!levelManager) levelManager = LevelManager.Instance;
    }

    public void Spawn_Tower_Blueprint()
    {
        if(Buy())
        {
            Debug.Log("inmstantiate bluwpint");
            Instantiate(tower_blueprint_prefab);
        }
    }

    public bool Buy()
    {
        Debug.Log("Buy Tower: " + tower_blueprint_prefab.name);
        Debug.Log($"Current Wood: {levelManager.Wood} | Stone: {levelManager.Stone}");

        if (woodCost > levelManager.Wood || stoneCost > levelManager.Stone)
        {
            Debug.Log($"Not enough resources: {(woodCost > levelManager.Wood ? "Wood" : "Stone")}");
            return false;
        }
        Debug.Log($"After Wood: {levelManager.Wood} | Stone: {levelManager.Stone}");

        levelManager.ConsumeResources(woodCost, stoneCost);

        return true;
    }
}
