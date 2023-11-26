
using UnityEngine;

public sealed class TownCenter : MonoBehaviour, IInteractable, IUpgradable, IDamageable
{
    [SerializeField] Transform night_fov;

    private void Awake()
    {
        audioS = GetComponent<AudioSource>();
        sRend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        night_fov.localScale = Vector3.one * fovRange;

        health = new Health(10);
    }

    void Update()
    {
        night_fov.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void OnClick()
    {
        Debug.Log("Clicked " + name);
        //audioS.PlayOneShot(onClickSFX);

        if(currentTier < maxTier)
            OverlayUIController.Instance.DisplayTC_Menu(true);
    }

    public void Deselect()
    {
        Debug.Log("Deseelect " + name);

        if (currentTier < maxTier)
            OverlayUIController.Instance.DisplayTC_Menu(false);
    }

    public void Upgrade()
    {
        if (currentTier + 1 > maxTier) return;

        if (!LevelManager.Instance.HasWood(woodCost)) return;

        LevelManager.Instance.ConsumeWood(woodCost);
        currentTier += 1;

        OnUpgraded();


        Debug.Log("Town Center Upgraded to tier : " + currentTier);

        if(currentTier >= maxTier)
            OverlayUIController.Instance.DisplayTC_Menu(false);
    }

    void OnUpgraded()
    {
        ChangeSprite();
        //audioS.PlayOneShot(upgradeSFX);
        // Play VFX?

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
            LevelManager.Instance.OnConstructedTier3TC?.Invoke();
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"TC took {damage}.");

        health.TakeDamage(damage);

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

    float fovRange = 3.0f;

    int currentTier = 1;
    readonly int maxTier = 3;

    int woodCost = 10;
    int stoneCost = 100;
}
