using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EventGameManager : Singleton<EventGameManager>
{
    public bool switchBool;
    // public static Action OnPlayerDead;
    // public delegate void SampleEventHandler(object sender);
    public Action<int> SampleEvent;
    public Action<Transform> OnProjectileCollision;

    void Update()
    {
        if (switchBool)
        {
            SampleEvent?.Invoke(1);
            switchBool = false;
        }
    }
        
}
