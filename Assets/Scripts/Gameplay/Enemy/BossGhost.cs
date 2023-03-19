using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Character;
using Gameplay.Weapon;
using UnityEngine;

public class BossGhost : EnemyController
{
    [SerializeField] private float timeToCast;
    [SerializeField] private float rad;

    protected override void Awake()
    {
        base.Awake();
        EventGameManager.Instance.OnBossHpChange?.Invoke(this);
    }

    protected override void Update()
    {
        if(IsMoveBlock != 0) return;
        
        if (timeShoot > 0)
        {
            timeShoot -= Time.deltaTime;
        }
        else
        { 
            timeShoot = AttackCooldown;
            
            MoveToRandomPoint();
            if (navMeshAgent.velocity.magnitude <= 0.1f)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        transform.LookAt(LevelManager.Instance.GetPlayerPos());
        var inVal = 0f;
        DOTween.To(() => inVal, x => inVal = x, 1, timeToCast).OnComplete(() =>
        {
            PerformAttack();
            Invoke(nameof(PerformAttack), 0.2f);
            Invoke(nameof(PerformAttack), 0.4f);
            Invoke(nameof(PerformAttack), 0.6f);
            Invoke(nameof(PerformAttack), 0.8f);
        });
    }

    private void MoveToRandomPoint()
    {
        var a = GetRandomPos();
        characterMovement.MovementOnDirection(a, navMeshAgent);
        transform.LookAt(a);
    }

    protected override void EnemyMoveToPlayer()
    {
        
    }

    protected override void RotateEnemy(Vector3 angle)
    {
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        
    }

    protected override void PerformAttack()
    {
        attackController.PerformAttack();
    }

    private Vector3 GetRandomPos()
    {
        return new Vector3(Random.insideUnitCircle.x * rad, LevelManager.Instance.player.transform.position.y-1, Random.insideUnitCircle.y * rad);
    }

    public override void TakeDamage(int amount, DamageType type, Weapon source)
    {
        base.TakeDamage(amount, type, source);
        EventGameManager.Instance.OnBossHpChange?.Invoke(this);
    }
}
