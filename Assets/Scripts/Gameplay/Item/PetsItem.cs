using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PetsItem : Items
{
    [SerializeField] private GameObject pet;
    protected override void Start()
    {
        ActivateItem(pet);
    }

    protected override void OnDestroy()
    {
        
    }

    protected override void ActivateItem(GameObject obj)
    {
        Instantiate(pet, LevelManager.Instance.player.transform.position, Quaternion.identity);
    }
}
