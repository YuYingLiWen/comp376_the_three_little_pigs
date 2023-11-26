using UnityEngine;

public sealed class CannonTower : Towers
{
    [SerializeField] GameObject projectile;

    protected override void Fire()
    {
        base.Fire();

        var obj = Instantiate(projectile);
        obj.transform.parent = transform;
        obj.transform.position = transform.position;
        obj.transform.up = target.position - transform.position;
    }

    public override void Upgrade()
    {
        if (currentTier + 1 > so.MaxTier) return;
        if (!LevelManager.Instance.HasResources(UpgradeCostWood, UpgradeCostStone)) return;

        base.Upgrade();

        switch (currentTier)
        {
            case 2:
                this.so = TowersUpgrades.GetInstance().GetArrowTier2SO;
                break;
            case 3:
                this.so = TowersUpgrades.GetInstance().GetCannonTier3SO;
                break;
            default:
                this.so = TowersUpgrades.GetInstance().GetArrowTierDebugSO;
                break;
        }

        base.OnUpgraded();
    }
}
