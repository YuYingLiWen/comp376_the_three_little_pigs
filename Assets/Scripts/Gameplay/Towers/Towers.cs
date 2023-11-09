using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(SpriteRenderer), typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
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
        coll = gameObject.GetComponent<SphereCollider>();
        audioS = gameObject.GetComponent<AudioSource>();
        rend = gameObject.GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        so = TowersUpgrades.GetInstance().GetArrowTier1SO;
        coll.radius = so.Range;
        
        GetComponent<Rigidbody>().isKinematic = true;
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
        Debug.Log("Trigger enter: " + other.name);

        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exit: " + other.name);

        if (other.CompareTag("Enemy"))
        {
            if (other.transform == target) target = null;
            enemiesInRange.Remove(other.gameObject);
        }
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

    /// <summary>
    /// Upgrades tower to next tier.
    /// </summary>
    public virtual void Upgrade()
    {
        if (currentTier + 1 > so.MaxTier) return;

        currentTier += 1;
    }

    public void OnClick()
    {
        audioS.Stop();
        audioS.PlayOneShot(so.OnClickSFX);
    }

    // The tower fires its weapon
    [ContextMenu("Fire")]
    protected virtual void Fire()
    {
        Debug.DrawRay(transform.position, target.position - transform.position, Color.yellow, 5.0f);

        OnFire();
    }

    protected virtual void OnFire()
    {
        audioS.Stop();
        audioS.PlayOneShot(so.OnShootSFX);
    }

    protected virtual void OnTierChange()
    {
        rend.sprite = so.TowerSprite;
    }

    //Cache
    Vector3 position;
    [SerializeField] protected Transform target = null;

    private float elapsedTime = 0.0f;

    private SphereCollider coll;
    private AudioSource audioS;
    private SpriteRenderer rend;
}

internal interface ITower
{
    /// <summary>
    /// To have man the tower.
    /// </summary>
    void Garrison(GameObject pig);
    
    /// <summary>
    /// To have ALL units abandon tower.
    /// </summary>
    void Abandon();
}

internal interface IInteractable
{
    void OnClick();
}