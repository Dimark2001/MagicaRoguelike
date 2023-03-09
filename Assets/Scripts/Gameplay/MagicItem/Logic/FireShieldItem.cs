using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShieldItem : Items
{
    [SerializeField] private GameObject vfxEffect;

    protected override void Start()
    {
        EventGameManager.Instance.OnProjectileSpawn += Activate;
    }

    protected override void OnDisable()
    {
        EventGameManager.Instance.OnProjectileSpawn -= Activate;
    }
    protected override void ActivateItem()
    {
        print("зачем это вообще нужно???");
    }

    void Activate(GameObject obj)
    {
        Instantiate(vfxEffect, obj.transform);
    }
}
