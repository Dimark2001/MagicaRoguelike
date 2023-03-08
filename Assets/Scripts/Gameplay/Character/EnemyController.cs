using System.Collections;
using DG.Tweening;
using Gameplay.Character;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : BaseCharacter
{
    [SerializeField] protected Enemy enemy;
    [SerializeField] protected CharacterMovement characterMovement;
    [SerializeField] protected AttackController attackController;
    [SerializeField] protected AttackController protectionController;
    [SerializeField] private bool isCanAttack;
    [SerializeField] private bool isCanProtection;


    private bool _isMoveBlock = false;
    private bool _isTakeDamage = false;

    private void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        characterRenderer = GetComponent<Renderer>();
        
        if (isCanAttack)
        {
            if(projectilePrefabs.Length != 0)
                SetWeaponPrefab(projectilePrefabs);
            else if(meleeWeaponPrefabs.Length != 0)
                SetWeaponPrefab(meleeWeaponPrefabs);
        }
    }

    protected virtual void Update()
    {
        if(_isMoveBlock) return;
        if(LevelManager.Instance.player == null) return;
        RotateEnemy(LevelManager.Instance.player.transform.position);
        
        if (CheckPlayerInRadius())
        {
            EnemyMoveToPlayer();
        }
        else
        {
            EnemyStay();
        }
        
        if (timeShoot > 0)
        {
            timeShoot -= Time.deltaTime;
        }
        else
        {
            if (CheckPlayerInRadius())
            {
                timeShoot = attackCooldown;
                if (isCanAttack)
                {
                    if(projectilePrefabs.Length != 0)
                        SetWeaponPrefab(projectilePrefabs);
                    else if(meleeWeaponPrefabs.Length != 0)
                        SetWeaponPrefab(meleeWeaponPrefabs);
                    PerformAttack();
                }
            }
        }
        
        if (isCanProtection)
        {
            if (CheckProjectileInRadius())
            {
                if(protectionsPrefab.Length != 0)
                    SetWeaponPrefab(protectionsPrefab);
                PerformProtection();
            }
        }
    }
    
    private void RotateEnemy(Vector3 angle)
    {
        transform.LookAt(angle);
    }

    private bool CheckPlayerInRadius()
    {
        if (Physics.CheckSphere(transform.position, enemy.radiusAgro))
        {
            var hitColliders = Physics.OverlapSphere(transform.position, enemy.radiusAgro);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    private bool CheckProjectileInRadius()
    {
        if (Physics.CheckSphere(transform.position, enemy.stoppingDistance))
        {
            var hitColliders = Physics.OverlapSphere(transform.position, enemy.stoppingDistance);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("PlayerProjectile"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void EnemyMoveToPlayer()
    {
        if (LevelManager.Instance.player == null)
        {
            BlockMove();
            characterMovement.StopMovement(navMeshAgent);
            return;
        }

        var playerPos = LevelManager.Instance.player.transform.position;
        characterMovement.MovementToTheSelectionPosition(playerPos, enemy.stoppingDistance, navMeshAgent);
    }

    private void PerformAttack()
    {
        if(attackController == null)
            return;
        
        if (CheckRaycast())
        {
            attackController.PerformAttack();
        }
        
        bool CheckRaycast()
        {
            if (LevelManager.Instance.player == null)
            {
                BlockMove();
                characterMovement.StopMovement(navMeshAgent);
                return false;
            }
            var target = LevelManager.Instance.player.transform;
            var position = transform.position;
            var dir = target.position - position;
            Physics.Raycast(position, dir, out var hit, enemy.stoppingDistance);
            if (hit.transform == target)
            {
                return true;
            }
            
            return false;
        }
    }

    private void PerformProtection()
    {
        if(protectionController == null)
            return;
        if(!isCanProtection) return;

        isCanProtection = false;
        protectionController.PerformProtection();
        var inVal = 0f;
        DOTween.To(() => inVal, x => inVal = x, 1, protectionCooldown).OnComplete(() =>
        {
            isCanProtection = true;
        });
    }

    private void EnemyStay()
    {
        characterMovement.StopMovement(navMeshAgent);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            var dir = -transform.forward;
            KnockBack(dir, enemy.force);
            
            player.TakeDamage(enemy.dmg);
            StartCoroutine(nameof(DestroyEnemy));
        }
    }

    public override void TakeDamage(int amount)
    {
        if(_isTakeDamage)
            return;

        _isTakeDamage = true;
        hp -= amount;
        BlockMove();

        if (hp <= 0)
        {
            SetColor(Color.gray);
            DestroyEnemy();
        }
        else
        {
            ReturnNormalState();
        }
    }
    
    private void ReturnNormalState()
    {
        var inVal = 0f;
        DOTween.To(() => inVal, x => inVal = x, 1, enemy.timeKnockBack).OnComplete(() =>
        {
            isKnockBack = false; 
            _isTakeDamage = false; 
            rb.isKinematic = true;
            AllowMove();
        });
    }

    protected void DestroyEnemy()
    {
        BlockMove();
        EnemyStay();
        var inVal = 0f;
        DOTween.To(() => inVal, x => inVal = x, 1, enemy.timeToDeath).OnComplete(() =>
        {
            Destroy(navMeshAgent);
            Destroy(this);
        });
    }

    public override void KnockBack(Vector3 dir, float force)
    {
        if(isKnockBack)
            return;

        isKnockBack = true;
        rb.isKinematic = false;
        rb.AddForce(dir * force, ForceMode.Impulse);
    }

    private void BlockMove()
    {
        _isMoveBlock = true;
    }

    private void AllowMove()
    {
        _isMoveBlock = false;
    }

    private void SetColor(Color color)
    {
        characterRenderer.material.color = color;
    }
}
