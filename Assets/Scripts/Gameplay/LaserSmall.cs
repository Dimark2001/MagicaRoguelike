using System.Collections.Generic;
using UnityEngine;

public class LaserSmall : ProjectileWeapon
{
    [SerializeField] private List<GameObject> vfxEffects;
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out EnemyController enemy))
            {
                EventGameManager.Instance.OnProjectileCollision?.Invoke(transform);
                enemy.KnockBack(transform.forward);
                enemy.TakeDamage(dmg);
            }
        }
    }
}
