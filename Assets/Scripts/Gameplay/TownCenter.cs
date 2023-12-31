
using System;
using UnityEngine;


public sealed class TownCenter : MonoBehaviour, IInteractable, IUpgradable, IDamageable
{
    [SerializeField] Transform night_fov;
    float night_fov_size = 25f; // adjust night fov circle size here
    public AudioClip barkClip;


    //public AudioClip houseClickClip, houseUpgradeClip;

    private void Awake()
    {
        audioS = GetComponent<AudioSource>();
        sRend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        night_fov.localScale = Vector3.one * fovRange;

        health = new Health(10);
        healthBar.SetActive(false);

        OnHouseUpgrade?.Invoke(currentTier);
    }

    void Update()
    {
        night_fov.position = Camera.main.WorldToScreenPoint(transform.position);
        night_fov.localScale = new Vector3(night_fov_size / CameraController.newOrthographicSize, night_fov_size / CameraController.newOrthographicSize, 1);

        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2.8f);
    }

    public void OnClick()
    {
        Debug.Log("Clicked " + name);
        audioS.PlayOneShot(onClickSFX);

        healthBar.SetActive(true);
        healthBar.OnHpUpdate(health.Points, health.MaxHealth);

        /*audioS.clip = houseClickClip;
        audioS.Play();*/

        if (currentTier < maxTier)
            OverlayUIController.Instance.DisplayTC_Menu(true);
    }

    public void Deselect()
    {
        Debug.Log("Deseelect " + name);
        healthBar.SetActive(false);


        if (currentTier < maxTier)
            OverlayUIController.Instance.DisplayTC_Menu(false);
    }

    public void Upgrade()
    {
        if (currentTier + 1 > maxTier) return;

        if (!LevelManager.Instance.HasResources(woodCost, stoneCost)) return;

        LevelManager.Instance.ConsumeResources(woodCost, stoneCost);
        currentTier += 1;

        OnUpgraded();


        Debug.Log("Town Center Upgraded to tier : " + currentTier);

        if(currentTier >= maxTier)
            OverlayUIController.Instance.DisplayTC_Menu(false);
    }

    void OnUpgraded()
    {
        ChangeSprite();
        audioS.PlayOneShot(upgradeSFX);
        // Play VFX?
        /*audioS.clip = houseUpgradeClip;
        audioS.Play();*/

        IncreaseCosts();
    }

    void IncreaseCosts()
    {
        woodCost = (int)((double)woodCost * 1.5f);
        stoneCost = (int)((double)stoneCost * 1.5f);

        UpdateUI();
    }

    void UpdateUI()
    {
        OverlayUIController.Instance.DisplayTC_Menu(false);
        OverlayUIController.Instance.DisplayTC_Menu(true);
    }

    void ChangeSprite()
    {
        if(currentTier == 2)
        {
            sRend.sprite = tierTwoSprite;
        }
        else if (currentTier == 3)
        {
            sRend.sprite = tierThreeSprite;
        }

        OnHouseUpgrade?.Invoke(currentTier);
    }

    public void TakeDamage(int damage)
    {
        audioS.PlayOneShot(barkClip);

        Debug.Log($"TC took {damage}.");

        health.TakeDamage(damage);
        healthBar.OnHpUpdate(health.Points, health.MaxHealth);

        if(IsDead())
        {
            Debug.Log("Game Lost TC Destroyed.");
            LevelManager.Instance.OnGameOver?.Invoke();
        }
    }

    public bool IsDead()
    {
        return !health.IsAlive();
    }

    public GameObject ThisObject()
    {
        return gameObject;
    }

    public void InstantDeath()
    {
        TakeDamage(9999);
    }

    public int UpgradeCostStone => stoneCost;
    public int UpgradeCostWood => woodCost;

    SpriteRenderer sRend;
    [SerializeField] Sprite tierTwoSprite;
    [SerializeField] Sprite tierThreeSprite;


    AudioSource audioS;
    [SerializeField] AudioClip upgradeSFX;
    [SerializeField] AudioClip onClickSFX;


    Health health;
    [SerializeField] HPBar healthBar;

    float fovRange = 3.0f;

    int currentTier = 1;
    readonly int maxTier = 3;

    int woodCost = 50;
    int stoneCost = 60;

    public Action<int> OnHouseUpgrade;
}
