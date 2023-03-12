using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Aim : Ability
{
    [SerializeField] private float radAim;

    private Transform _target;
    
    void FixedUpdate()
    {
        if(_target == null)
            FindTarget();
        else
            transform.parent.LookAt(_target);
    }

    private void FindTarget()
    {
        var hitColliders = Physics.OverlapSphere(transform.position, radAim);
        _target = hitColliders.ToList().Find(col => col.TryGetComponent(out Enemy enemyController))?.transform;
    }
}
