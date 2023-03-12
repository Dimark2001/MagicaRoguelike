using System;
using UnityEngine;

public class AuraNegativeGravityItem : Items
{
    protected override void Start()
    {
        ActivateItem(LevelManager.Instance.player.gameObject);
    }

    protected override void OnDestroy()
    {
        
    }

    protected override void ActivateItem(GameObject a)
    {
        Instantiate(AbilityManager.Instance.auraNegativeGravity, LevelManager.Instance.player.transform);
    }
}
