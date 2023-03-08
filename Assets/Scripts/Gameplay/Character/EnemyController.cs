using System.Collections;
using DG.Tweening;
using Gameplay.Character;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private AttackController attackController;
    [SerializeField] private AttackController protectionController;
    [SerializeField] private bool isCanAttack;
    [SerializeField] private bool isCanProtection;


    private bool _isMoveBlock = false;
    private bool _isTakeDamage = false;

    private void Start()
    {
        if (isCanAttack)
        {
            if(enemy.projectilePrefabs.Length != 0)
                enemy.SetWeaponPrefab(enemy.projectilePrefabs);
            else if(enemy.meleeWeaponPrefabs.Length != 0)
                enemy.SetWeaponPrefab(enemy.meleeWeaponPrefabs);
        }
    }

    private void Update()
    {
        if(_isMoveBlock) return;
        
        if (CheckPlayerInRadius())
        {
            EnemyMoveToPlayer();
        }
        else
        {
            EnemyStay();
        }
        
        if (enemy.timeShoot > 0)
        {
            enemy.timeShoot -= Time.deltaTime;
        }
        else
        {
            if (CheckPlayerInRadius())
            {
                enemy.timeShoot = enemy.attackCooldown;
                if (isCanAttack)
                {
                    if(enemy.projectilePrefabs.Length != 0)
                        enemy.SetWeaponPrefab(enemy.projectilePrefabs);
                    else if(enemy.meleeWeaponPrefabs.Length != 0)
                        enemy.SetWeaponPrefab(enemy.meleeWeaponPrefabs);
                    PerformAttack();
                }
            }
        }
        
        if (isCanProtection)
        {
            if (CheckProjectileInRadius())
            {
                if(enemy.protectionsPrefab.Length != 0)
                    enemy.SetWeaponPrefab(enemy.protectionsPrefab);
                PerformProtection();
            }
        }
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
            characterMovement.StopMovement(enemy.navMeshAgent);
            return;
        }

        var playerPos = LevelManager.Instance.player.transform.position;
        characterMovement.MovementToTheSelectionPosition(playerPos, enemy.stoppingDistance, enemy.navMeshAgent);
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
                characterMovement.StopMovement(enemy.navMeshAgent);
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
        DOTween.To(() => inVal, x => inVal = x, 1, enemy.protectionCooldown).OnComplete(() =>
        {
            isCanProtection = true;
        });
    }

    private void EnemyStay()
    {
        characterMovement.StopMovement(enemy.navMeshAgent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {

            var dir = -transform.forward;
            KnockBack(dir);
            
            player.TakeDamage(enemy.dmg);
            StartCoroutine(nameof(DestroyEnemy));
        }
    }

    public void TakeDamage(int amount)
    {
        if(_isTakeDamage)
            return;

        _isTakeDamage = true;
        enemy.hp -= amount;
        BlockMove();

        if (enemy.hp <= 0)
        {
            SetColor(Color.gray);
            StartCoroutine(nameof(DestroyEnemy));
        }
        else
        {
            StartCoroutine(nameof(ReturnNormalState));
        }
    }
    
    private IEnumerator ReturnNormalState()
    {
        yield return new WaitForSeconds(enemy.timeKnockBack);
        enemy.isKnockBack = false; 
        _isTakeDamage = false; 
        enemy.rb.isKinematic = true;
        AllowMove();
    }
    
    private IEnumerator DestroyEnemy()
    {
        BlockMove();
        EnemyStay();
        enemy.characterRenderer.material.color = Color.gray;
        yield return new WaitForSeconds(enemy.timeToDeath);
        Destroy(enemy.navMeshAgent);
        Destroy(this);
    }

    public void KnockBack(Vector3 dir)
    {
        if(enemy.isKnockBack)
            return;

        enemy.isKnockBack = true;
        enemy.rb.isKinematic = false;
        enemy.rb.AddForce(dir * enemy.force, ForceMode.Impulse);
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
        enemy.characterRenderer.material.color = color;
    }
}
