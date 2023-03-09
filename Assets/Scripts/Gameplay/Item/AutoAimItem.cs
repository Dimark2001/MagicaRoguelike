using System;
using System.Linq;
using UnityEngine;

public class AutoAimItem : Items
{
    protected override void Start()
    {
        EventGameManager.Instance.OnProjectileSpawn += ActivateItem;
    }

    protected override void OnDisable()
    {
        EventGameManager.Instance.OnProjectileSpawn -= ActivateItem;
    }

    protected override void ActivateItem(GameObject proj)
    {
        Instantiate(AbilityManager.Instance.autoAim, proj.transform);
    }
}
