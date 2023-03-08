using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EventGameManager : Singleton<EventGameManager>
{
    public Action<Transform> OnProjectileCollision;
    public Action<GameObject> OnProjectileSpawn;
}
