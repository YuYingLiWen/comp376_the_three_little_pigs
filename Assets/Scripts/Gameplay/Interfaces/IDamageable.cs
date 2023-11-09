using UnityEngine;

internal interface IDamageable
{
    void TakeDamage(int damage);

    bool IsDead();

    GameObject ThisObject();
}
