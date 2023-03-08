using System;
using DG.Tweening;
using Gameplay.Weapon;
using UnityEngine;

public abstract class ProjectileWeapon : Weapon
{
    [SerializeField] private float timeToDestroy;
    
    [SerializeField] protected float lifeTime;
    [SerializeField] protected float speed;
    [SerializeField] protected GameObject visual;
    
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

    protected virtual void DestroyProjectile()
    {
        visual.SetActive(false);
        transform.GetComponent<Collider>().enabled = false;
        speed = 0;
        var inVal = 0f;
        DOTween.To(() => inVal, x => inVal = x, 1, timeToDestroy).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
