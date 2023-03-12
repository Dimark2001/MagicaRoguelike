using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EventGameManager : Singleton<EventGameManager>
{
    public Action<GameObject> OnProjectileCollision;
    public Action<GameObject> OnProjectileSpawn;
    // public Action<GameObject> OnPickUpItem;
    public Action<GameObject> OnProtected;
    public Action<String> OnGetItem;
    public Action OnCoinChange;
    public Action OnPlayerHpChange;
    public Action OnBossSpawn;
    public Action<EnemyController> OnBossHpChange;
    public Action OnBossDead;
}
