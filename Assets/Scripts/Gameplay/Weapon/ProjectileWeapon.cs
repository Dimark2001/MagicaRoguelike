using System;
using DG.Tweening;
using Gameplay.Weapon;
using UnityEngine;

public abstract class ProjectileWeapon : Weapon
{
    [SerializeField] private float timeToDestroy;
    [SerializeField] protected GameObject visual;
    [SerializeField] private bool isShootThroughWall;
    protected float speedLimit;

    protected Rigidbody Rb;
    
    private void Awake()
    {
        speedLimit = speed;
        Rb = GetComponent<Rigidbody>();
        Invoke(nameof(DestroyProjectile), lifeTime);
    }

    private void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        if(Rb.velocity.magnitude < speedLimit || Rb.velocity.normalized != transform.forward.normalized)
            Rb.AddForce(transform.forward * speed, ForceMode.Force);
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
        Rb.isKinematic = true;
        var inVal = 0f;
        DOTween.To(() => inVal, x => inVal = x, 1, timeToDestroy).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
