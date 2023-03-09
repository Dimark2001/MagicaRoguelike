using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Aim : MonoBehaviour
{
    [SerializeField] private float radAim;

    private Transform _target;

    private void Awake()
    {
        var hitColliders = Physics.OverlapSphere(transform.position, radAim);
        _target = hitColliders.ToList().Find(col => col.TryGetComponent(out Enemy enemyController))?.transform;
    }

    void Update()
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
