using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityShieldItem : Items
{
    protected override void Start()
    {
        EventGameManager.Instance.OnProtected += ActivateItem;
    }

    protected override void OnDestroy()
    {
        EventGameManager.Instance.OnProtected -= ActivateItem;
    }

    protected override void ActivateItem(GameObject gameO)
    {
        Instantiate(AbilityManager.Instance.gravityShield, gameO.transform);
    }
}
