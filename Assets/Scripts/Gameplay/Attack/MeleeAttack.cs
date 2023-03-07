using Gameplay.Character;
using Gameplay.Weapon;
using UnityEngine;

public class MeleeAttack : AttackController
{
    protected override void Attack()
    {
        var angle = TargetPointer.GetAttackAngle();
        var weapon = BaseCharacter.GetWeaponPrefab<Weapon>().gameObject;
        weapon.transform.rotation = Quaternion.Euler(0, angle, 0);
        weapon.SetActive(true);
    }

    protected override void PProtection()
    {
        
    }
}