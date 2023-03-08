using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShield : Items
{
    protected override void ActivateItem()
    {
        //сжигает снаряды
        EventGameManager.Instance.SampleEvent += A;
        print(this.name);
    }

    void A(int a)
    {
        
    }
}
