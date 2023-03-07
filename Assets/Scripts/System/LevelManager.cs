using System;
using Gameplay.Character;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Player player;
    public Transform dynamicContainer;
    void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);
        Instance = this;
    }
}
