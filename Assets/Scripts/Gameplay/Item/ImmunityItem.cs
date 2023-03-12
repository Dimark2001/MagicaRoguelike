
using UnityEngine;

public class ImmunityItem : Items
{
    [SerializeField] public ImmunityType immunityType;
    protected override void Start()
    {
        ActivateItem(null);
    }

    protected override void OnDestroy()
    {
    }

    protected override void ActivateItem(GameObject obj)
    {
        LevelManager.Instance.player.immunityList.Add(immunityType);
    }
}
