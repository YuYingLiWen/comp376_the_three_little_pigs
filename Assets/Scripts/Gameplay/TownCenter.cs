
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

        currentTier += 1;

        Debug.Log("Town Center Upgraded to tier : " + currentTier);

        if(currentTier >= maxTier)
            OverlayUIController.Instance.DisplayTC_Menu(false);

        //audioS.PlayOneShot(upgradeSFX);

        ChangeSprite();
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
}
