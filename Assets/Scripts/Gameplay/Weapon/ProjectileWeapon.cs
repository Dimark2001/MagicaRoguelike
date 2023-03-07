using System;
using Gameplay.Weapon;
using UnityEngine;

public abstract class ProjectileWeapon : Weapon
{
    [SerializeField] protected float lifeTime;
    [SerializeField] protected float speed;
    
    [SerializeField] private bool isShootThroughWall;
    
    private void Start()
    {
        Invoke(nameof(DestroyProjectile), lifeTime);
    }

    private void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        transform.Translate(0, 0, Time.deltaTime * speed, Space.Self);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            if (!isShootThroughWall)
                DestroyProjectile();
        }
    }

    protected void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
