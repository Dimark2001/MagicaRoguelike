using UnityEngine;

public class PlayerProjectile : ProjectileWeapon
{
    [SerializeField] private float forceKnockBack;
    protected virtual void Start()
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
                enemy.TakeDamage(dmg, DamageType.Magical, this);
                DestroyProjectile();
            }
        }
    }

    protected override void DestroyProjectile()
    {
        EventGameManager.Instance.OnProjectileCollision?.Invoke(gameObject);
        base.DestroyProjectile();
    }
}
