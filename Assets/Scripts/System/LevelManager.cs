using System;
using Gameplay.Character;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : Singleton<LevelManager>
{
    public Player player;
    public Transform dynamicContainer;
    public GameObject portal;
    public int currentLevel = 0;

    [SerializeField] private int _coins;

    public int Coins
    {
        get
        {
            return _coins;
        }
        set
        {
            if (value >= 0) _coins = value;
            else _coins = 0;
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
