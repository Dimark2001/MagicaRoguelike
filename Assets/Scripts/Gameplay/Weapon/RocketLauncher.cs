using Gameplay.Character;
using UnityEngine;

public class RocketLauncher : ProjectileWeapon
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private AnimationCurve speedCurve;
    private float time = 0;
    protected override void OnTriggerEnter(Collider other)
    {
        Explosion(); 
        DestroyProjectile();
    }

    protected override void Move()
    {
        time += Time.deltaTime;
        speed = speedCurve.Evaluate(time);
        //transform.Translate(0, 0, Time.deltaTime * speedCurve.Evaluate(time), Space.Self);
        base.Move();
    }

    private void Explosion()
    {
        var others = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach (var other in others)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.KnockBack(player.transform.position - transform.position);
                player.TakeDamage(dmg);
            }
            if (other.TryGetComponent(out EnemyController enemy))
            {
                enemy.KnockBack(enemy.transform.position - transform.position);
                enemy.TakeDamage(dmg);
            }
        }
    }
}
