using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MeteoriteRain : Ability
{
    private Sequence _sequence;
    
    private void OnDestroy()
    {
        _sequence.Kill();
    }

    public void CreateMeteoriteRain(int dmg, float rad, float duration)
    {
        _sequence = DOTween.Sequence();
        var inVal = 0f;
        var count = 1;
        _sequence.Append(DOTween.To(() => inVal, x => inVal = x, dmg, duration).OnUpdate(() =>
        {
            var perSecond = dmg / duration;
            if ((perSecond*count) <= (int)inVal)
            {
                count++;
                var others = Physics.OverlapSphere(transform.position, rad);
                foreach (var other in others)
                {
                    if (other.TryGetComponent(out BaseCharacter baseCharacter))
                    {
                        baseCharacter.TakeDamage((int)perSecond, DamageType.Explosion, null);
                    }
                }
            }
        }));
    }
}
