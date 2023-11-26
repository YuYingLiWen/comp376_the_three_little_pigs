using UnityEngine;
/// <summary>
/// Contains the data needed to upgrade each type of towers.
/// </summary>
public class TowersUpgrades : MonoBehaviour
{
    private static TowersUpgrades instance = null;
    public static TowersUpgrades GetInstance() => instance;
    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Note that tier 1 is assigned by default to the tower object.
    /// </summary>


    [SerializeField]private TowerScriptableObject arrowTowerTier3SO;
    [SerializeField]private TowerScriptableObject arrowTowerTier2SO;
    [SerializeField]private TowerScriptableObject arrowTowerTierDebugSO;
    
    [SerializeField]private TowerScriptableObject cannonTier3SO;
    [SerializeField]private TowerScriptableObject cannonTier2SO;
    [SerializeField]private TowerScriptableObject cannonTierDebugSO;

    public TowerScriptableObject GetArrowTier3SO     => arrowTowerTier3SO;
    public TowerScriptableObject GetArrowTier2SO     => arrowTowerTier2SO;
    public TowerScriptableObject GetArrowTierDebugSO => arrowTowerTierDebugSO;

    public TowerScriptableObject GetCannonTier3SO     => cannonTier3SO;
    public TowerScriptableObject GetCannonTier2SO     => cannonTier2SO;
    public TowerScriptableObject GetCannonTierDebugSO => cannonTierDebugSO;
}
