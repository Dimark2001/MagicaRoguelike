using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Character;
using UnityEngine;

public class Skeleton : EnemyController
{
    [SerializeField] private int explotanoDmg;
    [SerializeField] private float explotanoRad;
    [SerializeField] private float forceExp;

    private void OnDestroy()
    {
        print("explosion");
        var abilityManager = AbilityManager.Instance;
        Instantiate(abilityManager.vfxSkeletonExplotano, transform);
        var circle = Instantiate(abilityManager.vfxCircle, transform);
        circle.transform.localScale = new Vector3(explotanoRad, explotanoRad, explotanoRad);
        var ex = Instantiate(abilityManager.explotano, transform);
        ex.GetComponent<Explotano>().CreateExplotano(explotanoDmg, explotanoRad, forceExp);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyEnemy();
        }
    }
}
