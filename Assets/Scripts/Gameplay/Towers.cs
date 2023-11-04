using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(SpriteRenderer), typeof(AudioSource))]
public abstract class Towers : MonoBehaviour, ITower, IInteractable
{
    [SerializeField] private Vector3 exit;

    private List<GameObject> garrisonedUnits = new();
    private List<GameObject> enemiesInRange = new();

    [SerializeField] private float range;
    [SerializeField] private int attack;
    [SerializeField] private float delayPerShot;
    [SerializeField] private int currentLevel; // Used for upgrade

    public int GetCurrentLevel() => currentLevel;

    void Awake()
    {
        coll = gameObject.GetComponent<CircleCollider2D>();
        audioS = gameObject.GetComponent<AudioSource>();
        rend = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        coll.radius = range;
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

    private void Attack()
    {
        if (elapsedTime >= delayPerShot)
        {
            elapsedTime = 0;
            throw new NotImplementedException(); // Fire!
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

    public void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    public void OnClick()
    {
        audioS.Play();
    }

    //Cache
    Vector3 position;
    private Transform target = null;

    private float elapsedTime = 0.0f;

    private CircleCollider2D coll;
    private AudioSource audioS;
    private SpriteRenderer rend;
}

public class TowerUpgradeData
{
    private TowerUpgradeData instance;
    public TowerUpgradeData GetInstance() => instance;

    // SOs
    // TowerScriptableObject GetScriptableObject(Towers tower)
/*    switch(tower.GetCurrentLevel())
        {
            case 0:
                // Set SO level 1;
                break;
            case 1:
                // Set SO level 2;
                break;
            default:
                Debug.LogWarning("Something went wrong", tower);
                break;
        }*/

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


    /// <summary>
    /// Upgrades tower to next tier.
    /// </summary>
    void Upgrade();

}

internal interface IInteractable
{
    void OnClick();
}