using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CircularSaw : Items
{
    private void Start()
    {
        EventGameManager.Instance.OnProjectileSpawn += Activate;
    }

    private void OnDisable()
    {
        EventGameManager.Instance.OnProjectileSpawn -= Activate;
    }

    protected override void ActivateItem()
    {
        print("hmm");
    }

    void Activate(GameObject projectile)
    {
        var abilityManager = AbilityManager.Instance;
        var a = new Vector3(0, Random.Range(0f, 360f), 0);
        Instantiate(abilityManager.circularSaw, projectile.transform.position,Quaternion.Euler(a), LevelManager.Instance.dynamicContainer);
    }
}