using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbilityManager : Singleton<AbilityManager>
{
    [Header("Items")]
    public List<Items> playerItemsList;
    public List<Items> itemsList;
    public List<Items> defaultList;

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
        defaultList = new List<Items>(itemsList);
    }

    public void ResetItem()
    {
        itemsList = new List<Items>(defaultList);
        playerItemsList = new List<Items>();
        if(transform.childCount == 0) return;
        for (int i = transform.childCount-1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        /*var objs = transform.GetComponentsInChildren<Items>();
        for (var i = 0; i < objs.Length; i++)
        {
            var gameObj = objs[i];
            Destroy(gameObj.gameObject);
        }
        
        var pets = transform.GetComponentsInChildren<Pets>();
        for (var i = 0; i < objs.Length; i++)
        {
            var gameObj = objs[i];
            Destroy(gameObj.gameObject);
        }

        var playerItems = LevelManager.Instance.player.transform.GetComponentsInChildren<Ability>();
        for (var i = 0; i < playerItems.Length; i++)
        {
            var item = playerItems[i];
            Destroy(item.gameObject);
        }*/
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
            case ChestType.Another:
                var a = Random.Range(0,2);
                if(a == 0)
                    GetRareItem();
                else
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
            EventGameManager.Instance.OnGetItem?.Invoke(playerItemsList[playerItemsList.Count-1].itemName);
        }
    }

    [Button("GetCommonItem")]
    private void GetCommonItem()
    {
        EventGameManager.Instance.OnGetItem?.Invoke("Increase base characteristic");
        var pl = LevelManager.Instance;
        pl.player.navMeshAgent.speed += 0.3f;
        
        pl.player.increaseDmg += 3;

        pl.player.maxHp += 5;
        pl.player.GetHp(20);
        
        pl.player.AttackCooldown -= 0.4f;
        
        pl.player.ProtectionCooldown -= 0.2f;
        
        pl.player.increaseSpeedProjectile += 0.5f;
        
        pl.player.increaseLifeTime += 0.4f;
    }
    // private void GetCommonItem()
    // {
    //     var val = Enum.GetValues(typeof(CommonReward));
    //     var r = (CommonReward)val.GetValue(Random.Range(0, val.Length));
    //     var pl = LevelManager.Instance;
    //     EventGameManager.Instance.OnGetItem?.Invoke(r.ToString());
    //     switch (r)
    //     {
    //         case CommonReward.Speed:
    //             pl.player.navMeshAgent.speed+=0.5f;
    //             break;
    //         case CommonReward.Dmg:
    //             pl.player.increaseDmg += 5;
    //             break;
    //         case CommonReward.Hp:
    //             pl.player.maxHp += 10;
    //             pl.player.GetHp(100);
    //             break;
    //         case CommonReward.HpRegen:
    //             break;
    //         case CommonReward.AttackCooldown:
    //             pl.player.AttackCooldown -= 0.3f;
    //             break;
    //         case CommonReward.ProtectionCooldown:
    //             pl.player.ProtectionCooldown -= 0.3f;
    //             break;
    //         case CommonReward.SpeedProjectile:
    //             pl.player.increaseSpeedProjectile += 0.3f;
    //             break;
    //         case CommonReward.LifeTimeProtect:
    //             pl.player.increaseLifeTime += 0.3f;
    //             break;
    //         case CommonReward.RadExplosion:
    //             break;
    //     }
    // }
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
