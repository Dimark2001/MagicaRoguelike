using System;
using DG.Tweening;
using Gameplay.Weapon;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] private float angleRot;
    
    private float rotY;
    private void OnEnable()
    {
        rotY = transform.rotation.y;
        transform.Rotate(new Vector3(0, rotY - angleRot,0), Space.Self);
        SwordRotate();
    }

    private void SwordRotate()
    { 
        transform.DORotate(new Vector3(0, rotY + angleRot * 2, 0), duration, RotateMode.WorldAxisAdd).OnComplete(
            () => { Invoke(nameof(DestroyWeapon), 0.01f); });
    }

    protected virtual void OnTriggerEnter(Collider other) { }
    
    private void DestroyWeapon()
    {
        gameObject.SetActive(false);
        transform.rotation = Quaternion.identity;
    }
}
