using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : Singleton<DontDestroy>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
