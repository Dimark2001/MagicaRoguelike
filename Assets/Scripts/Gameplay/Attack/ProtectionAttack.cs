using Gameplay.Weapon;
using UnityEngine;

public class ProtectionAttack : AttackController
{
    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }

    protected override void PProtection()
    {
        var parent = LevelManager.Instance.dynamicContainer;
        var angle = TargetPointer.GetAttackAngle();
        Instantiate(BaseCharacter.GetWeaponPrefab<Weapon>(), BaseCharacter.transform.position, Quaternion.Euler(0, angle, 0), parent);
    }
}
