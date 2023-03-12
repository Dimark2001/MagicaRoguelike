using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Character;
using UnityEngine;

public class CircularProjectile : PlayerProjectile
{
    [SerializeField] private float forceKnock;
    private bool _isDmgPlayer = false;
    [SerializeField] private float rot;
    protected override void Start()
    {
        
    }

    protected override void Move()
    {
        visual.transform.Rotate(new Vector3(0,visual.transform.rotation.eulerAngles.y,0) + new Vector3(0,rot,0));
        if (Vector3.Distance(LevelManager.Instance.player.transform.position, transform.position) >= 5)
            transform.LookAt(LevelManager.Instance.player.transform);
        base.Move();
    }
    
    
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (_isDmgPlayer && other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out Player player))
            {
                player.KnockBack(transform.forward, forceKnock);
                player.TakeDamage(dmg, DamageType.Physical, this);
            }
            
            DestroyProjectile();//вызывает событие, которого здесь не должно быть
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isDmgPlayer = true;
        }
    }
    // protected override void DestroyProjectile()
    // {
    //     // EventGameManager.Instance.OnProjectileCollision?.Invoke(gameObject);
    //     //base.DestroyProjectile();
    // }
}
