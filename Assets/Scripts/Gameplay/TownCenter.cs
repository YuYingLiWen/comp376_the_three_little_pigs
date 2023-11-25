
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
        OverlayUIController.Instance.DisplayTC_Menu(true);
    }

    public void Deselect()
    {
        Debug.Log("Deseelect " + name);
        OverlayUIController.Instance.DisplayTC_Menu(false);
    }

    public void Upgrade()
    {
        if (currentTier + 1 > maxTier) return;

        currentTier += 1;

        Debug.Log("Town Center Upgraded to tier : " + currentTier);

        //audioS.PlayOneShot(upgradeSFX);

        ChangeSprite();
    }

    void ChangeSprite()
    {
        if(currentTier == 2)
        {
            sRend.sprite = tier2sprite;
        }
        else if (currentTier == 3)
        {
            sRend.sprite = tier3sprite;
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


    SpriteRenderer sRend;
    [SerializeField] Sprite tier2sprite;
    [SerializeField] Sprite tier3sprite;


    AudioSource audioS;
    [SerializeField] AudioClip upgradeSFX;
    [SerializeField] AudioClip onClickSFX;


    Health health;

    float fovRange = 3.0f;

    int currentTier = 0;
    readonly int maxTier = 3;
}
