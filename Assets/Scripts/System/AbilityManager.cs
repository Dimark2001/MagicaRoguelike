using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

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

    public void GiveRandomItem(ChestType chestType)
    {
        switch (chestType)
        {
            case ChestType.Rare:
                GetRareItem();
                break;
            default:
                print("предметы кончились, ожидайте доставку");
                break;
        }
    }

    void GetRareItem()
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
}
