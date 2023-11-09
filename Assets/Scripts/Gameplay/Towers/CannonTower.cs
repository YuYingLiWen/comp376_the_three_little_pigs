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
        base.Upgrade();

        switch (currentTier)
        {
            case 1:
                this.so = TowersUpgrades.GetInstance().GetCannonTier1SO;
                break;
            case 2:
                this.so = TowersUpgrades.GetInstance().GetArrowTier2SO;
                break;
            default:
                this.so = TowersUpgrades.GetInstance().GetArrowTierDebugSO;
                break;
        }
    }
}
