using UnityEngine;

internal interface IDamageable
{
    // Take Damage
    void TakeDamage(int damage);

    // Check if this object is dead.
    bool IsDead();

    // Kill the object immediately.
    void InstantDeath();

    /// <summary>
    /// Return reference to this object.
    /// </summary>
    GameObject ThisObject();
}
