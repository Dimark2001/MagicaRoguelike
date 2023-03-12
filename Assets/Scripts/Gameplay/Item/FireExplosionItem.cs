using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosionItem : Items
{
    public int dmg;
    public float rad;
    public float forceExp;

    protected override void Start()
    {
        EventGameManager.Instance.OnProjectileCollision += ActivateItem;
    }

    protected override void OnDestroy()
    {
        EventGameManager.Instance.OnProjectileCollision -= ActivateItem;
    }

    protected override void ActivateItem(GameObject GOob)
    {
        var abilityManager = AbilityManager.Instance;
        Instantiate(abilityManager.vfxExplotano, GOob.transform);
        var circle = Instantiate(abilityManager.vfxCircle, GOob.transform);
        circle.transform.localScale = new Vector3(rad, rad, rad);
        var ex = Instantiate(abilityManager.explotano, GOob.transform);
        ex.GetComponent<Explotano>().CreateExplotano(dmg, rad, forceExp);
    }
}
