using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Character;
using UnityEngine;

public class BossCrab : EnemyController
{
    [SerializeField] private float timeToCastRain;
    [SerializeField] private int meteoriteRainDmg;
    [SerializeField] private float meteoriteRainRad;
    [SerializeField] private float meteoriteRainDuration;
    [SerializeField] private float meteoriteCooldown;

    private int _maxHp;

    protected override void Awake()
    {
        _maxHp = Hp;
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
            if (CheckPlayerInRadius())
            {
                if (Hp > _maxHp / 2)
                {
                    timeShoot = AttackCooldown;
                    SetWeaponPrefab(projectilePrefabs);
                    PerformAttack();
                }
                else
                {
                    timeShoot = meteoriteCooldown;
                    for (var i = 0; i < 9; i++)
                    {
                        Laser(GetRandomPos() + LevelManager.Instance.player.transform.position);
                    }
                    SetWeaponPrefab(protectionsPrefab);
                    PerformProtection();
                }
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.TakeDamage(enemy.dmg, DamageType.Touch, null);
            player.KnockBack(-transform.position + player.transform.position, 20);
            var inVal = 0f;
            DOTween.To(() => inVal, x => inVal = x, 1, 0.3f).OnComplete(() =>
            {
                Laser(LevelManager.Instance.player.transform.position);
            });
        }
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
