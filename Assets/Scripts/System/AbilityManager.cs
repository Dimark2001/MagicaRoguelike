using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NaughtyAttributes;
using UnityEditorInternal;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbilityManager : Singleton<AbilityManager>
{
    [Header("Items")]
    public List<Items> playerItemsList;
    public List<Items> itemsList;
    public List<Pets> petsList;

    [Header("AbilityEffect")]
    public GameObject explotano;
    public GameObject meteoriteRain;
    public GameObject circularSaw;
    public GameObject autoAim;
    public GameObject auraNegativeGravity;
    public GameObject gravityShield;
    public GameObject vampireTouch;
    public GameObject toxicPuddle;

    [Header("VFX")] 
    public GameObject vfxExplotano;
    public GameObject vfxSkeletonExplotano;
    public GameObject vfxCircle;
    public GameObject vfxMeteoriteRain;
    public GameObject vfxPoisonPuddle;
    public GameObject vfxLightningAura;
    public GameObject vfxLightningTouch;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

    }

    public void GiveRandomItem(ChestType chestType)
    {
        switch (chestType)
        {
            case ChestType.Rare:
                GetRareItem();
                break;
            case ChestType.Common:
                GetCommonItem();
                break;
            default:
                print("предметы кончились, ожидайте доставку");
                break;
        }
    }
    [Button("GetRareItem")]
    private void GetRareItem()
    {
        var l = itemsList.Count;
        if (l > 0)
        {
            var a = Random.Range(0, l);
            playerItemsList.Add(itemsList[a]);
            itemsList.Remove(itemsList[a]);
            Instantiate(playerItemsList[playerItemsList.Count() - 1], transform);
        }
    }
    [Button("GetCommonItem")]
    private void GetCommonItem()
    {
        var val = Enum.GetValues(typeof(CommonReward));
        var r = (CommonReward)val.GetValue(Random.Range(0, val.Length));
        var pl = LevelManager.Instance;
        print(r);
        switch (r)
        {
            case CommonReward.Speed:
                pl.player.navMeshAgent.speed+=0.5f;
                break;
            case CommonReward.Dmg:
                pl.player.increaseDmg += 5;
                break;
            case CommonReward.Hp:
                // pl.player.hp
                break;
            case CommonReward.HpRegen:
                break;
            case CommonReward.AttackCooldown:
                pl.player.AttackCooldown -= 0.3f;
                break;
            case CommonReward.ProtectionCooldown:
                pl.player.ProtectionCooldown -= 0.3f;
                break;
            case CommonReward.SpeedProjectile:
                pl.player.increaseSpeedProjectile += 0.3f;
                break;
            case CommonReward.LifeTimeProtect:
                pl.player.increaseLifeTime += 0.3f;
                break;
            case CommonReward.RadExplosion:
                break;
        }
    }
}

public enum CommonReward
{
    Speed,
    Dmg,
    Hp,
    HpRegen,
    AttackCooldown,
    ProtectionCooldown,
    SpeedProjectile,
    LifeTimeProtect,
    RadExplosion,
}
