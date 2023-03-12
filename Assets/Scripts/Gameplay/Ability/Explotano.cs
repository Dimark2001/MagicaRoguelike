using UnityEngine;

public class Explotano : Ability
{
    public void CreateExplotano(int dmg, float rad, float force)
    {
        var others = Physics.OverlapSphere(transform.position, rad);
        
        foreach (var other in others)
        {
            if (other.TryGetComponent(out BaseCharacter baseCharacter))
            {
                baseCharacter.KnockBack(baseCharacter.transform.position - transform.position, force);
                baseCharacter.TakeDamage(dmg, DamageType.Explosion, null);
            }
        }
    }
}
