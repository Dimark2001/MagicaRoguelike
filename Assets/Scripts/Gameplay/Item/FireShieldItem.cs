using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShieldItem : Items
{
    [SerializeField] private GameObject vfxEffect;

    protected override void Start()
    {
        EventGameManager.Instance.OnProjectileSpawn += ActivateItem;
    }

    protected override void OnDisable()
    {
        EventGameManager.Instance.OnProjectileSpawn -= ActivateItem;
    }

    protected override void ActivateItem(GameObject obj)
    {
        Instantiate(vfxEffect, obj.transform);
    }
}
