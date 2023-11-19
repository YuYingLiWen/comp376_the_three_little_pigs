using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(SpriteRenderer), typeof(AudioSource))]
public abstract class Towers : MonoBehaviour, ITower, IInteractable
{
    [SerializeField] private Vector3 exit;
    [SerializeField] Transform night_fov;
    private List<GameObject> garrisonedUnits = new();
    protected List<GameObject> enemiesInRange = new();

    protected int currentTier = 0; // Used for upgrade

    protected TowerScriptableObject so;

    // Returns the upgrade level/tier of the tower.
    public int GetCurrentTier() => currentTier;

    protected virtual void Awake()
    {
        if (!levelManager) levelManager = FindFirstObjectByType<LevelManager>();
        if (!uiControl) uiControl = FindFirstObjectByType<OverlayUIController>();

        coll = gameObject.GetComponent<SphereCollider>();
        audioS = gameObject.GetComponent<AudioSource>();
        rend = gameObject.GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        so = TowersUpgrades.GetInstance().GetArrowTier1SO;
        
        // Set ranges
        coll.radius = so.Range;
        rangeIndicator.transform.localScale = Vector3.one * so.Range;
        night_fov.localScale = Vector3.one * so.Range;
    }

    protected virtual void Update()
    {
        night_fov.position = Camera.main.WorldToScreenPoint(transform.position);


        if (enemiesInRange.Count <= 0 && target == null) return;

        if(!target)
        {
            elapsedTime = 0.0f;
            SelectTarget();
        }

        if (target) Attack();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision enter: " + collision.collider.name);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger enter: " + other.name);

        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        //Debug.Log("Trigger Exit: " + other.name);

        if (other.CompareTag("Enemy"))
        {
            if (other.transform == target) target = null;
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private void OnDisable()
    {
        levelManager = null;
        uiControl = null;
    }

    // Pick a target. Default pick the closest.
    protected virtual void SelectTarget()
    {
        Transform current = enemiesInRange[0].transform; 
        float minRange = (enemiesInRange[0].transform.position - position).magnitude;

        for(int i = 1; i < enemiesInRange.Count; i++)
        {
            float distance = (enemiesInRange[0].transform.position - position).magnitude;

            if (distance < minRange)
            {
                minRange = distance;
                current = enemiesInRange[i].transform;
            }
        }

        target = current;
    }

    // Fires an arrow towards the target.
    private void Attack()
    {
        if (elapsedTime >= so.DelayPerShot)
        {
            elapsedTime = 0;

            Fire();
            OnFire(); // Fire!
        }
        elapsedTime += Time.deltaTime;
    }

    public void Abandon()
    {
        foreach(GameObject pig in garrisonedUnits)
        {
            pig.SetActive(true);
            pig.transform.position = transform.position + exit;
        }
        garrisonedUnits.Clear();
    }

    public void Garrison(GameObject pig)
    {
        garrisonedUnits.Add(pig);
        pig.SetActive(false);
    }

    public virtual void Upgrade()
    {
        if (currentTier + 1 > so.MaxTier) return;

        currentTier += 1;
    }

    // The tower fires its weapon
    protected virtual void Fire()
    {
        Debug.DrawRay(transform.position, target.position - transform.position, Color.yellow, 5.0f);

        OnFire();
    }

    protected virtual void OnFire()
    {
        //audioS.Stop();
        //audioS.PlayOneShot(so.OnShootSFX);
    }

    protected virtual void OnTierChange()
    {
        rend.sprite = so.TowerSprite;
    }

    public void OnClick()
    {
        //audioS.Stop();
        //audioS.PlayOneShot(so.OnClickSFX);

        rangeIndicator.SetActive(true);
        uiControl.DisplayUpgradeTowerMenu(true, this.gameObject);

        Debug.Log("Clicked " + name);
    }

    public void Deselect()
    {
        rangeIndicator.SetActive(false);
        uiControl.DisplayUpgradeTowerMenu(false);

        Debug.Log("Deseelect " + name);

    }

    public virtual void Sell()
    {
        Debug.Log("Sell Tower");

        Destroy(gameObject);
    }


    private static OverlayUIController uiControl;

    //Cache
    Vector3 position;
    [SerializeField] protected Transform target = null;

    private float elapsedTime = 0.0f;

    private SphereCollider coll;
    private AudioSource audioS;
    private SpriteRenderer rend;

    [SerializeField] GameObject rangeIndicator;

    private static LevelManager levelManager;
}

public interface ITower
{
    /// <summary>
    /// To have man the tower.
    /// </summary>
    void Garrison(GameObject pig);
    
    /// <summary>
    /// To have ALL units abandon tower.
    /// </summary>
    void Abandon();


    /// <summary>
    /// Upgrades tower to next tier.
    /// </summary>
    void Upgrade();

    /// <summary>
    /// Sell the tower.
    /// </summary>
    void Sell();
}

internal interface IInteractable
{
    void OnClick();

    void Deselect();
}