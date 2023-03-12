using Gameplay.Character;
using Gameplay.Weapon;
using UnityEngine;

public class RangeAttack : AttackController
{
    protected override void Attack()
    {
        var parent = LevelManager.Instance.dynamicContainer;
        var angle = TargetPointer.GetAttackAngle();
        Instantiate(BaseCharacter.GetWeaponPrefab<Weapon>(), BaseCharacter.transform.position, Quaternion.Euler(0, angle, 0), parent);
        print(angle);
    }

    protected override void PProtection()
    {
    
    }
}