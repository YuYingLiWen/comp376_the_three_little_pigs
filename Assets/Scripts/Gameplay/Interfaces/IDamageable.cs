using UnityEngine;

internal interface IDamageable
{
    // Take Damage
    void TakeDamage(int damage);

    // Check if this object is dead.
    bool IsDead();

    /// <summary>
    /// Return reference to this object.
    /// </summary>
    GameObject ThisObject();
}
