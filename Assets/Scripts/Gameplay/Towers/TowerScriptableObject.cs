using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "ScriptableObjects/Tower")]
public class TowerScriptableObject : ScriptableObject
{

    [SerializeField, Tooltip("Cost to get to next tier.")] protected int woodCost;
    [SerializeField, Tooltip("Cost to get to next tier.")] protected int stoneCost;

    [SerializeField] protected int attack;
    [SerializeField] protected float range;
    [SerializeField] protected float delayPerShot;
    [SerializeField] protected int maxTier;
    [SerializeField] protected Sprite tower;
    [SerializeField] protected AudioClip onClickSFX;
    [SerializeField] protected AudioClip onShootSFX;

    public int Attack => attack;
    public float Range => range;
    public float DelayPerShot => delayPerShot;
    public int MaxTier => maxTier;
    public Sprite TowerSprite => tower;
    public AudioClip OnClickSFX => onClickSFX;
    public AudioClip OnShootSFX => onShootSFX;

    // Cost to get to next tier.
    public int WoodCost => woodCost;

    // Cost to get to next tier.
    public int StoneCost => stoneCost;
}