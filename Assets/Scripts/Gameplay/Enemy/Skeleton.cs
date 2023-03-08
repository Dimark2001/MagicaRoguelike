using System.Collections;
using System.Collections.Generic;
using Gameplay.Character;
using UnityEngine;

public class Skeleton : EnemyController
{
    [SerializeField] private float explotanoRad;
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Explosion();
            StartCoroutine(nameof(DestroyEnemy));
        }
    }
    
    private void Explosion()
    {
        var others = Physics.OverlapSphere(transform.position, explotanoRad);
        
        foreach (var other in others)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.KnockBack(transform.position - player.transform.position);
                player.TakeDamage(base.enemy.dmg);
            }
            if (other.TryGetComponent(out EnemyController enemy))
            {
                if(enemy != this)
                    enemy.KnockBack(-transform.forward);
                enemy.TakeDamage(base.enemy.dmg);
            }
        }
    }
}
