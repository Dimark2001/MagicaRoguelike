using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Character;
using Gameplay.Weapon;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossWizard : EnemyController
{
    [SerializeField] private float timeToCastRain;
    [SerializeField] private int meteoriteRainDmg;
    [SerializeField] private float meteoriteRainRad;
    [SerializeField] private float meteoriteRainDuration;
    [SerializeField] private float meteoriteCooldown;

    protected override void Awake()
    {
        base.Awake();
        EventGameManager.Instance.OnBossHpChange?.Invoke(this);
    }

    protected override void Update()
    {
        if(IsMoveBlock != 0) return;
        transform.LookAt(LevelManager.Instance.GetPlayerPos());
        
        if (timeShoot > 0)
        {
            timeShoot -= Time.deltaTime;
        }
        else
        {
            if (CheckPlayerInRadius())
            {
                if (Hp > maxHp / 1.5f)
                {
                    timeShoot = AttackCooldown;
                    SetWeaponPrefab(projectilePrefabs);
                    PerformAttack();
                }
                else
                {
                    timeShoot = meteoriteCooldown;
                    for (var i = 0; i < 3; i++)
                    {
                        MeteoriteRain(GetRandomPos() + LevelManager.Instance.player.transform.position);
                    }
                    SetWeaponPrefab(protectionsPrefab);
                    PerformProtection();
                }
            }
            else
            {
                MoveToRandomPoint();
            }
        }
    }
    
    private void MoveToRandomPoint()
    {
        var a = new Vector3(Random.insideUnitCircle.x * 2, LevelManager.Instance.player.transform.position.y-1, Random.insideUnitCircle.y * 2);
        characterMovement.MovementOnDirection(a, navMeshAgent);
        transform.LookAt(a);
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
                MeteoriteRain(LevelManager.Instance.player.transform.position);
            });
        }
    }

    protected override void PerformAttack()
    {
        attackController.PerformAttack();
    }

    private Vector3 GetRandomPos()
    {
        return new Vector3(Random.insideUnitCircle.x * 11, LevelManager.Instance.player.transform.position.y-1, Random.insideUnitCircle.y * 11);
    }

    private void MeteoriteRain(Vector3 pos)
    {
        var playerPos = pos;
        var attack = Instantiate(AbilityManager.Instance.meteoriteRain);
        attack.transform.position = playerPos;
        attack.GetComponent<MeteoriteRain>().CreateMeteoriteRain(meteoriteRainDmg, meteoriteRainRad, meteoriteRainDuration);
        var vfx = Instantiate(AbilityManager.Instance.vfxMeteoriteRain);
        vfx.transform.position = playerPos;
        vfx.transform.localScale = new Vector3(1, 1, 1) * meteoriteRainRad / 4;
    }

    public override void TakeDamage(int amount, DamageType type, Weapon source)
    {
        base.TakeDamage(amount, type, source);
        EventGameManager.Instance.OnBossHpChange?.Invoke(this);
    }
}
