using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TeslaCoil : Pets
{
    [SerializeField] private float durationStun;
    protected override void Update()
    {
        if(LevelManager.Instance.player == null) return;
        
        if (_isCanAttack)
        {
            characterMovement.MovementToTheSelectionPosition(LevelManager.Instance.player.transform.position, navMeshAgent.stoppingDistance, navMeshAgent);
            if (FindEnemyInRadius() != null)
            {
                UseAbility(FindEnemyInRadius());
            }
        }
    }
    
    protected override void UseAbility(Transform enemy)
    {
        base.UseAbility(enemy);
        if (CheckRaycastEnemy())
        {
            var vfx = Instantiate(AbilityManager.Instance.vfxLightningTouch, enemy.transform.position, Quaternion.identity, enemy);
            var inVal = 0f;
            DOTween.To(() => inVal, x => inVal = x, 1, 0.1f).OnComplete(() =>
            {
                Destroy(vfx);
            });
            
            var enemyController = enemy.GetComponent<EnemyController>();
            enemyController.TakeDamage(dmg, DamageType.Electric, null);
            enemyController.ElectricStun(durationStun);
        }
    }
    
    private bool CheckRaycastEnemy()
    {
        if (FindEnemyInRadius() == null)
        {
            return false;
        }
        var target = FindEnemyInRadius();
        var position = transform.position;
        var dir = target.position - position;
        Physics.Raycast(position, dir, out var hit, argoRad);
        if (target.CompareTag("Enemy"))
        {
            return true;
        }
            
        return false;
    }
}
