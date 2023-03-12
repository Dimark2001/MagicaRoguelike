using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ToxicPuddle : Ability
{
    public void CreateToxicPuddle(int dmg, float rad, float duration)
    {
        var inVal = 0f;
        var count = 1;
        DOTween.To(() => inVal, x => inVal = x, dmg, duration).OnUpdate(() =>
        {
            var perSecond = dmg / duration;
            if ((perSecond*count) <= (int)inVal)
            {
                count++;
                print(1);
                var others = Physics.OverlapSphere(transform.position, rad);
                foreach (var other in others)
                {
                    if (other.TryGetComponent(out BaseCharacter baseCharacter))
                    {
                        baseCharacter.TakeDamage((int)perSecond, DamageType.Poison, null);
                    }
                }
            }
        });
    }
}
