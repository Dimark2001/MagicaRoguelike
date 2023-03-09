using UnityEngine;

public class LaserSmall : ProjectileWeapon
{
    [SerializeField] private float forceKnockBack;
    void Start()
    {
        EventGameManager.Instance.OnProjectileSpawn?.Invoke(this.gameObject);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out EnemyController enemy))
            {
                enemy.KnockBack(transform.forward, forceKnockBack);
                enemy.TakeDamage(dmg, DamageType.Magical);
                DestroyProjectile();
            }
        }
    }

    protected override void DestroyProjectile()
    {
        EventGameManager.Instance.OnProjectileCollision?.Invoke(transform);
        base.DestroyProjectile();
    }
}
