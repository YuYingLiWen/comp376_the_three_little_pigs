
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(SpriteRenderer), typeof(AudioSource))]
public abstract class Towers : MonoBehaviour, ITower, IInteractable, IUpgradable
{
    [SerializeField] private Vector3 exit;
    [SerializeField] Transform night_fov;
    float night_fov_size = 30f; // adjust night fov circle size here
    private List<GameObject> garrisonedUnits = new();
    protected List<GameObject> enemiesInRange = new();

    protected int currentTier = 1; // Used for upgrade

    [SerializeField] protected TowerScriptableObject so; // Default assign tier 1.

    public AudioClip towerSelectClip, towerUpgradeClip, towerBuildClip;
    
    float range_indicator_size; // size of range indicator shall be same as night_fov

    // Returns the upgrade level/tier of the tower.
    public int GetCurrentTier() => currentTier;

    protected virtual void Awake()
    {
        coll = GetComponent<SphereCollider>();
        audioS = GetComponent<AudioSource>();
        rend = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        // Set ranges
        coll.radius = so.Range;
        rangeIndicator.transform.localScale = Vector3.one * so.Range;
        night_fov.localScale = Vector3.one * so.Range;

        audioS.clip = towerBuildClip;
        audioS.Play();
    }

    protected virtual void Update()
    {
        night_fov.position = Camera.main.WorldToScreenPoint(transform.position);
        // scale the night fov circle with the zooming so that it remains the same
        night_fov.localScale = new Vector3(night_fov_size / CameraController.newOrthographicSize, night_fov_size / CameraController.newOrthographicSize, 1);

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

        // Indicate that tower is occupied
        Transform circleTransform = transform.Find("CircleSelect");
        if (circleTransform != null)
        {
            circleTransform.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Child object 'CircleSelect' not found.");
        }
    }

    public void Garrison(GameObject pig)
    {
        garrisonedUnits.Add(pig);
        pig.SetActive(false);

        // Indicate that tower is occupied
        Transform circleTransform = transform.Find("CircleSelect");
        if (circleTransform != null)
        {
            circleTransform.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Child object 'CircleSelect' not found.");
        }
    }

    public virtual void Upgrade()
    {
        Debug.Log($"{this.GetType()} is trying to upgrade to next tier.", this.gameObject);

        LevelManager.Instance.ConsumeResources(UpgradeCostWood, UpgradeCostStone);
        currentTier += 1;

        if (currentTier >= so.MaxTier)
            OverlayUIController.Instance.DisplayUpgradeTowerMenu(false);

        Debug.Log($"{this.GetType()} is now tier {currentTier}.", this.gameObject);
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

    protected virtual void OnUpgraded()
    {
        rend.sprite = so.TowerSprite;
        // Play SFX
        audioS.clip = towerUpgradeClip;
        audioS.Play();
        // Play VFX?
    }

    public void OnClick()
    {
        audioS.clip = towerSelectClip;
        audioS.Play();

        rangeIndicator.SetActive(true);

        if (currentTier < so.MaxTier)
            OverlayUIController.Instance.DisplayUpgradeTowerMenu(true, this.gameObject);

        Debug.Log("Clicked " + name);
    }

    public void Deselect()
    {
        rangeIndicator.SetActive(false);

        if (currentTier < so.MaxTier)
            OverlayUIController.Instance.DisplayUpgradeTowerMenu(false);

        Debug.Log("Deseelect " + name);

    }

    public virtual void Sell()
    {
        Debug.Log("Sell Tower");

        Destroy(gameObject);
    }

    public int UpgradeCostStone => so.StoneCost;
    public int UpgradeCostWood => so.WoodCost;

    //Cache
    Vector3 position;
    [SerializeField] protected Transform target = null;

    private float elapsedTime = 0.0f;

    private SphereCollider coll;
    private AudioSource audioS;
    private SpriteRenderer rend;

    [SerializeField] GameObject rangeIndicator;
}
