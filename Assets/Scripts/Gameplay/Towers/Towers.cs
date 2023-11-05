using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider), typeof(SpriteRenderer), typeof(AudioSource))]
public abstract class Towers : MonoBehaviour, ITower, IInteractable
{
    [SerializeField] private Vector3 exit;

    private List<GameObject> garrisonedUnits = new();
    private List<GameObject> enemiesInRange = new();

    protected int currentTier = 0; // Used for upgrade

    protected TowerScriptableObject so;

    // Returns the upgrade level/tier of the tower.
    public int GetCurrentTier() => currentTier;

    void Awake()
    {
        coll = gameObject.GetComponent<CapsuleCollider>();
        audioS = gameObject.GetComponent<AudioSource>();
        rend = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        coll.radius = so.Range;
        coll.height= so.Range;
    }

    private void Update()
    {
        if (enemiesInRange.Count <= 0 && target == null) return;

        if(!target)
        {
            elapsedTime = 0.0f;
            SelectTarget();
        }

        if (target) Attack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.gameObject);
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
    protected virtual void Fire()
    {


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
    private Transform target = null;

    private float elapsedTime = 0.0f;

    private CapsuleCollider coll;
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