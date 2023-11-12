using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public sealed class RifleTower : Towers, IInteractable
{
    ParticleSystem ps;
    LineRenderer line;

    protected override void Awake()
    {
        base.Awake();

        ps = GetComponentInChildren<ParticleSystem>();
        line = GetComponent<LineRenderer>();
    }

    protected override void Fire()
    {
        base.Fire();

        StartCoroutine(PlayVFX());

        ps.transform.up = target.position - transform.position;
        ps.Play();

        var enemy = target.GetComponent<IDamageable>();

        enemy.TakeDamage(so.Attack);
        if(enemy.IsDead())
        {
            target = null;
            enemiesInRange.Remove(enemy.ThisObject());
        }
    }

    private IEnumerator PlayVFX()
    {
        SetPositions(transform.position, target.position);
        yield return waitForLineVFX;
        ResetPositions();
    }

    public override void Upgrade()
    {
        base.Upgrade();

        switch (currentTier)
        {
            case 1:
                this.so = TowersUpgrades.GetInstance().GetArrowTier1SO;
                break;
            case 2:
                this.so = TowersUpgrades.GetInstance().GetArrowTier2SO;
                break;
            default:
                this.so = TowersUpgrades.GetInstance().GetArrowTierDebugSO;
                break;
        }
    }

    public void SetPositions(Vector3 start, Vector3 end)
    {
        line.SetPosition(0, start - Vector3.forward);
        line.SetPosition(1, end - Vector3.forward);
    }

    public void ResetPositions()
    {
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);
    }

    //Cache
    static readonly WaitForSeconds waitForLineVFX = new (0.02f);
}
