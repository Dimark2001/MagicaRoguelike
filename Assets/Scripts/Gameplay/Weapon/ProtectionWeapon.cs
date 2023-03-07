using DG.Tweening;
using Gameplay.Weapon;
using UnityEngine;

public class ProtectionWeapon : Weapon
{
    [SerializeField] protected float lifeTime;
    [SerializeField] private float endValue;
    [SerializeField] private float duration;
    [SerializeField] private float speed;
    private bool _isMove;
    private Vector3 _defScale;
    private void Awake()
    {
        _defScale = transform.localScale;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        _isMove = true;
        transform.DOScale(_defScale, duration).OnComplete(() =>
        {
            _isMove = false;
        });
        
        Invoke(nameof(DestroyProjectile), lifeTime);
    }
    
    private void Update()
    {
        if(_isMove)
            Move();
    }

    protected virtual void Move()
    {
        transform.Translate(0, 0, Time.deltaTime * speed, Space.Self);
    }
    
    protected void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
