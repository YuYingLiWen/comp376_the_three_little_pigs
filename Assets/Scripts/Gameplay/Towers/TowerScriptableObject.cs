using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "ScriptableObjects/Tower")]
public class TowerScriptableObject : ScriptableObject
{
    [SerializeField] protected int woodCost;
    [SerializeField] protected int stoneCost;

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

    public int WoodCost => woodCost;
    public int StoneCost => stoneCost;
}