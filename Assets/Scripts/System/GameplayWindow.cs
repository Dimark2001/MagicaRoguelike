using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayWindow : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        canvas.worldCamera = Camera.main;
    }
}
