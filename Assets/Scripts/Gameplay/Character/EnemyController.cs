using System.Collections;
using System.Linq;
using DG.Tweening;
using Gameplay.Character;
using Gameplay.Weapon;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : BaseCharacter
{
    [SerializeField] protected Enemy enemy;
    [SerializeField] protected CharacterMovement characterMovement;
    [SerializeField] protected AttackController attackController;
    [SerializeField] protected AttackController protectionController;
    [SerializeField] protected bool isCanAttack;
    [SerializeField] private bool isCanProtection;

    protected int IsMoveBlock = 0;
    protected bool IsTakeDamage = false;

    protected virtual void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        characterRenderer = GetComponent<Renderer>();
        
        if (isCanAttack)
        {
            if(projectilePrefabs.Count != 0)
                SetWeaponPrefab(projectilePrefabs);
            else if(meleeWeaponPrefabs.Count != 0)
                SetWeaponPrefab(meleeWeaponPrefabs);
        }
    }

    protected virtual void Update()
    {
        if(IsMoveBlock != 0) return;
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
                timeShoot = AttackCooldown;
                if (isCanAttack)
                {
                    if(projectilePrefabs.Count != 0)
                        SetWeaponPrefab(projectilePrefabs);
                    else if(meleeWeaponPrefabs.Count != 0)
                        SetWeaponPrefab(meleeWeaponPrefabs);
                    PerformAttack();
                }
            }
        }
        
        if (isCanProtection)
        {
            if (CheckProjectileInRadius())
            {
                if(protectionsPrefab.Count != 0)
                    SetWeaponPrefab(protectionsPrefab);
                PerformProtection();
            }
        }
    }
    
    private void RotateEnemy(Vector3 angle)
    {
        transform.LookAt(angle);
    }

    protected bool CheckPlayerInRadius()
    {
        var lm = LevelManager.Instance;
        if (lm.GetPlayerPos() == Vector3.zero)
            return false;
        if(Vector3.Distance(transform.position, lm.GetPlayerPos()) <= enemy.radiusAgro)
            return true;

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

    protected void EnemyMoveToPlayer()
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

    protected virtual void PerformAttack()
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

    protected virtual void PerformProtection()
    {
        if(protectionController == null)
            return;
        if(!isCanProtection) return;

        isCanProtection = false;
        protectionController.PerformProtection();
        var inVal = 0f;
        DOTween.To(() => inVal, x => inVal = x, 1, ProtectionCooldown).OnComplete(() =>
        {
            isCanProtection = true;
        });
    }

    protected void EnemyStay()
    {
        if(navMeshAgent != null)
            characterMovement.StopMovement(navMeshAgent);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            var dir = -transform.forward;
            KnockBack(dir, enemy.force);
            
            player.TakeDamage(enemy.dmg, DamageType.Touch, null);
            StartCoroutine(nameof(DestroyEnemy));
        }
    }

    public override void TakeDamage(int amount, DamageType type, Weapon source)
    {
        if(IsTakeDamage)
            return;
        if (immunityList.Any(immunity => type.ToString() == immunity.ToString())) 
            return;
        if (source != null && source.gameObject.CompareTag("PlayerProjectile"))
            LevelManager.Instance.player.GetHp(source.dmg);
        IsTakeDamage = true;
        hp -= amount;
        BlockMove();

        if (hp <= 0)
        {
            DestroyEnemy();
        }
        else
        {
            ReturnNormalState(enemy.timeKnockBack);
        }
    }

    public void ElectricStun(float duration)
    {
        if(immunityList.Any(el => el == ImmunityType.Electric)) return;
        
        BlockMove();
        characterMovement.StopMovement(navMeshAgent);
        var a = Instantiate(AbilityManager.Instance.vfxLightningAura, transform);
        var inVal = 0f;
        DOTween.To(() => inVal, x => inVal = x, 1, duration).OnComplete(() =>
        {        
            DestroyThis(a);
            AllowMove();
        });
    }

    private void DestroyThis(GameObject thisObject)
    {
        Destroy(thisObject);
    }
    
    private void ReturnNormalState(float duration)
    {
        var inVal = 0f;
        DOTween.To(() => inVal, x => inVal = x, 1, duration).OnComplete(() =>
        {
            isKnockBack = false; 
            IsTakeDamage = false; 
            rb.isKinematic = true;
            AllowMove();
        });
    }

    protected void DestroyEnemy()
    {
        BlockMove();
        EnemyStay();
        var inVal = 0f;
        Destroy(enemy);
        DOTween.To(() => inVal, x => inVal = x, 1, enemy.timeToDeath).OnComplete(() =>
        {
            BlockMove();
            DeadBodyCleaner.Instance.enemyBody.Add(gameObject);
        });
    }

    public override void KnockBack(Vector3 dir, float force)
    {
        if(isKnockBack)
            return;
        if(immunityList.Contains(ImmunityType.KnockBack))
            return;
        
        isKnockBack = true;
        rb.isKinematic = false;
        rb.AddForce(dir.normalized * force, ForceMode.Impulse);
    }

    private void BlockMove()
    {
        IsMoveBlock++;
    }

    private void AllowMove()
    {
        IsMoveBlock--;
    }
}
