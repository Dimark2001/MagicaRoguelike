using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGameManager : Singleton<EventGameManager>
{
    public Action OnPlayerDead;
}
