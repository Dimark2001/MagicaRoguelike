﻿
using UnityEngine;

public class ImmunityItem : Items
{
    [SerializeField] public ImmunityType immunityType;
    protected override void Start()
    {
        
    }

    protected override void OnDisable()
    {
    }

    protected override void ActivateItem(GameObject obj)
    {
        LevelManager.Instance.player.immunityList.Add(immunityType);
    }
}