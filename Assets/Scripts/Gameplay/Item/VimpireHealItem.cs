using UnityEngine;

public class VimpireHealItem : Items
{
    protected override void Start()
    {
        ActivateItem(null);
    }

    protected override void OnDisable(){}

    protected override void ActivateItem(GameObject obj)
    {
        LevelManager.Instance.player.isVampireAbility = true;
    }
}
