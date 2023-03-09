using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Character;
using UnityEngine;

public class CircularProjectile : ProjectileWeapon
{
    
    [SerializeField] private float forceKnockBack;
    private Rigidbody rb;
    private bool _isDmgPlayer = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void Move()
    {
        if (Vector3.Distance(LevelManager.Instance.player.transform.position, transform.position) >= 5)
            transform.LookAt(LevelManager.Instance.player.transform);
        rb.AddForce(transform.forward * speed, ForceMode.Force);
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
                player.KnockBack(player.transform.position - transform.position, forceKnockBack);
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
