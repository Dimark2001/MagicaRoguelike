using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Character;
using UnityEngine;

public class BossGhost : EnemyController
{
    [SerializeField] private float timeToCast;
    private int _maxHp;

    protected override void Awake()
    {
        _maxHp = hp;
        base.Awake();
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
        CreateVampireCircle();
        var inVal = 0f;
        DOTween.To(() => inVal, x => inVal = x, 1, timeToCast).OnComplete(() =>
        {
            PerformAttack();
            Invoke(nameof(PerformAttack), 0.2f);
            Invoke(nameof(PerformAttack), 0.4f);
        });
    }

    private void CreateVampireCircle()
    {
        
    }

    private void MoveToRandomPoint()
    {
        characterMovement.MovementToTheSelectionPosition(GetRandomPos(), 0, navMeshAgent);
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
        return new Vector3(Random.insideUnitCircle.x * 10, LevelManager.Instance.player.transform.position.y-1, Random.insideUnitCircle.y * 10);
    }
    
    

    private void Laser(Vector3 pos)
    {
        var playerPos = pos;
        var attack = Instantiate(AbilityManager.Instance.meteoriteRain);
        
    }
}
