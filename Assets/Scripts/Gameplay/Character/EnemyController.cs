using System.Collections;
using Gameplay.Character;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private AttackController attackController;
    [SerializeField] private bool isCanAttack;

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
            
            //player.TakeDamage(enemy.dmg);
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
        }
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
        isMoveBlock = true;
    }

    private void AllowMove()
    {
        isMoveBlock = false;
    }

    private void SetColor(Color color)
    {
        enemy.characterRenderer.material.color = color;
    }
}
