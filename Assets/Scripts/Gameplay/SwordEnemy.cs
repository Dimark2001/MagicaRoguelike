using Gameplay.Character;
using UnityEngine;

public class SwordEnemy : MeleeWeapon
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("Player"))
        {
            if(other.TryGetComponent(out Player player))
            {
                
            }
        }
    }
}
