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
    
    public Vector3 GetPlayerPos()
    {
        return player == null ? Vector3.zero : player.transform.position;
    }
}
