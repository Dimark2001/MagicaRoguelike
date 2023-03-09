using Gameplay.Character;
using UnityEngine;

namespace Gameplay.Weapon
{
    public class EnemyProjectile : ProjectileWeapon
    {
        [SerializeField] private float forceKnockBack;
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out Player player))
                {
                    player.KnockBack(player.transform.position - transform.position, forceKnockBack);
                    player.TakeDamage(dmg, DamageType.Magical);
                }
            
                DestroyProjectile();
            }
        }
    }
}