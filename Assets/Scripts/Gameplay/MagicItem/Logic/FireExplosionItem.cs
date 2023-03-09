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
        EventGameManager.Instance.OnProjectileCollision += Activate;
    }

    protected override void OnDisable()
    {
        EventGameManager.Instance.OnProjectileCollision -= Activate;
    }

    protected override void ActivateItem()
    {
        print("hmm");
    }

    void Activate(Transform t)
    {
        var abilityManager = AbilityManager.Instance;
        Instantiate(abilityManager.vfxExplotano, t);
        var circle = Instantiate(abilityManager.vfxCircle, t);
        circle.transform.localScale = new Vector3(rad, rad, rad);
        var ex = Instantiate(abilityManager.explotano, t);
        ex.GetComponent<Explotano>().CreateExplotano(dmg, rad, forceExp);
    }
}
