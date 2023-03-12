using DG.Tweening;
using UnityEngine;

public class Slime : Pets
{
    [SerializeField] private float rad;

    protected override void Update()
    {
        if(LevelManager.Instance.player == null) return;
        
        characterMovement.MovementToTheSelectionPosition(LevelManager.Instance.player.transform.position, navMeshAgent.stoppingDistance, navMeshAgent);
        if (_isCanAttack)
        {
            UseAbility(transform);
        }
    }
    
    protected override void UseAbility(Transform enemy)
    {
        base.UseAbility(enemy);
        var puddle = Instantiate(AbilityManager.Instance.toxicPuddle, transform.position, Quaternion.identity);
        puddle.GetComponent<ToxicPuddle>().CreateToxicPuddle(20, rad, AttackCooldown*4);
        var vfx = Instantiate(AbilityManager.Instance.vfxPoisonPuddle);
        vfx.transform.position = transform.position;
        vfx.transform.localScale = new Vector3(rad, rad, rad)/4;
        var inVal = 0f;
        DOTween.To(() => inVal, x => inVal = x, 1, AttackCooldown*8).OnComplete(() =>
        {
            Destroy(puddle);
            Destroy(vfx);
        });
    }
}
