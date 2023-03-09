using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Character;
using UnityEngine;

public class CircularProjectile : ProjectileWeapon
{
    [SerializeField] private float forceKnockBack;
    private bool _isDmgPlayer = false;

    protected override void Move()
    {
        if (Vector3.Distance(LevelManager.Instance.player.transform.position, transform.position) >= 5)
            transform.LookAt(LevelManager.Instance.player.transform);
        base.Move();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out EnemyController enemy))
            {
                enemy.KnockBack(transform.forward, forceKnockBack);
                enemy.TakeDamage(dmg, DamageType.Physical);
                DestroyProjectile();
            }
        }
        if (_isDmgPlayer && other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out Player player))
            {
                player.KnockBack(transform.forward, forceKnockBack);
                player.TakeDamage(dmg, DamageType.Physical);
            }
            
            DestroyProjectile();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isDmgPlayer = true;
        }
    }
}
