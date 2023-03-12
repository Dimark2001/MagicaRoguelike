using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CircularSawItem : Items
{
    protected override void Start()
    {
        EventGameManager.Instance.OnProjectileSpawn += ActivateItem;
    }

    protected override void OnDestroy()
    {
        EventGameManager.Instance.OnProjectileSpawn -= ActivateItem;
    }

    protected override void ActivateItem(GameObject projectile)
    {
        
        var abilityManager = AbilityManager.Instance;
        var a = new Vector3(0, Random.Range(0f, 360f), 0);
        Instantiate(abilityManager.circularSaw, projectile.transform.position,Quaternion.Euler(a), LevelManager.Instance.dynamicContainer);
    }
}