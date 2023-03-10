using System;
using Gameplay.Character;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : Singleton<LevelManager>
{
    public Player player;
    public Transform dynamicContainer;

    public Vector3 GetPlayerPos()
    {
        return player == null ? Vector3.zero : player.transform.position;
    }
}
