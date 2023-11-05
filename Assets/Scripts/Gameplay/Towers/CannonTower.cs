
public sealed class CannonTower : Towers
{
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
