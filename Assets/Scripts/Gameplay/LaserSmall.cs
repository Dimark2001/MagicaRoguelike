using UnityEngine;

public class LaserSmall : ProjectileWeapon
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out EnemyController enemy))
            {
                enemy.KnockBack(transform.forward);
                enemy.TakeDamage(dmg);
            }
        }
    }
}
