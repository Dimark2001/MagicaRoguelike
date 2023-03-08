using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShield : Items
{
    [SerializeField] private GameObject vfxEffect;

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
        print("зачем это вообще нужно???");
    }

    void Activate(GameObject obj)
    {
        Instantiate(vfxEffect, obj.transform);
    }
}
