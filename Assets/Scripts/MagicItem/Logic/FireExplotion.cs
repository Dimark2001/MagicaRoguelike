using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplotion : Items
{
    private void Start()
    {
        EventGameManager.Instance.OnProjectileCollision += Activate;
    }

    private void OnDisable()
    {
        EventGameManager.Instance.OnProjectileCollision -= Activate;
    }

    protected override void ActivateItem()
    {
        print("hmm");
    }

    void Activate(Transform t)
    {
        print("EXPOTION");
        print(t.position);
    }
}
