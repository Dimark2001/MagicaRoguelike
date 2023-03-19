using System;
using DG.Tweening;
using Gameplay.Character;
using Gameplay.Weapon;
using UnityEngine;
using UnityEngine.AI;

public class Pets : BaseCharacter
{
    [SerializeField] protected CharacterMovement characterMovement;
    [SerializeField] protected float argoRad;
    [SerializeField] protected int dmg;
    protected bool _isCanAttack;

    private void Awake()
    {
        _isCanAttack = true;
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        transform.parent = AbilityManager.Instance.transform;
    }

    
        
        
    

    public override void TakeDamage(int dmg, DamageType type, Weapon source)
    {
        
    }

    public override void KnockBack(Vector3 dir, float force)
    {
        
    }

    protected virtual void Update()
    {
        if (Vector3.Distance(transform.position, LevelManager.Instance.GetPlayerPos()) <= 10)
        {
            navMeshAgent.enabled = false;
            transform.position = LevelManager.Instance.GetPlayerPos();
            navMeshAgent.enabled = true;
        }
        
        if(LevelManager.Instance.player == null) return;
        
        characterMovement.MovementToTheSelectionPosition(LevelManager.Instance.player.transform.position, navMeshAgent.stoppingDistance, navMeshAgent);
        if (_isCanAttack)
        {
            if (FindEnemyInRadius() != null)
            {
                UseAbility(FindEnemyInRadius());
            }
        }
    }
    
    protected Transform FindEnemyInRadius()
    {
        var hitColliders = Physics.OverlapSphere(transform.position, argoRad);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out Enemy enemy))
            {
                return enemy.transform;
            }
        }

        return null;
    }

    protected virtual void UseAbility(Transform enemy)
    {
        _isCanAttack = false;
        var inVal = 0f;
        DOTween.To(() => inVal, x => inVal = x, 1, AttackCooldown).OnComplete(() =>
        {
            _isCanAttack = true;
        });
    }
}
