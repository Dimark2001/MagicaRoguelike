using System;
using Gameplay.Character;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : Singleton<LevelManager>
{
    public Player player;
    public Transform dynamicContainer;
    public int currentLevel = 0;

    [SerializeField] private int coins;

    public int Coins
    {
        get
        {
            return coins;
        }
        set
        {
            EventGameManager.Instance.OnCoinChange?.Invoke();
            if (value >= 0) coins = value;
            else coins = 0;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
    }

    public Vector3 GetPlayerPos()
    {
        return player == null ? Vector3.zero : player.transform.position;
    }
}
