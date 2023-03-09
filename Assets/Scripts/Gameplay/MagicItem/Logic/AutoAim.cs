using System;
using System.Linq;
using UnityEngine;

public class AutoAim : Items
{
    protected override void Start()
    {
        EventGameManager.Instance.OnProjectileSpawn += AddAutoAim;
    }

    protected override void OnDisable()
    {
        EventGameManager.Instance.OnProjectileSpawn -= AddAutoAim;
    }

    protected override void ActivateItem()
    {
        
    }

    private void AddAutoAim(GameObject proj)
    {
        Instantiate(AbilityManager.Instance.autoAim, proj.transform);
    }

    private void Update()
    {
        
    }
}
