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

    [Header("AbilityEffect")]
    public GameObject explotano;
    public GameObject meteoriteRain;
    public GameObject autoAim;

    [Header("VFX")] 
    public GameObject vfxExplotano;
    public GameObject vfxSkeletonExplotano;
    public GameObject vfxCircle;
    public GameObject vfxMeteoriteRain;

    public void GiveRandomItem()
    {
        var l = itemsList.Count;
        if (l > 0)
        {
            var a = Random.Range(0, l);
            playerItemsList.Add(itemsList[a]);
            itemsList.Remove(itemsList[a]);
            print(a);
            Instantiate(playerItemsList[playerItemsList.Count() - 1], transform);
        }
    }
}
