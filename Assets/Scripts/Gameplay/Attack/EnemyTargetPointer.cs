using UnityEngine;
using UnityEngine.UIElements;

public class EnemyTargetPointer : TargetPointer
{
    public override float GetAttackAngle()
    {
        var position = transform.position;
        
        var angle = Mathf.Atan2(GetTargetPosition().x - position.x, GetTargetPosition().z - position.z) * Mathf.Rad2Deg;
        
        return angle;
    }

    public override Vector3 GetTargetPosition()
    {
        var targetPosition = LevelManager.Instance.player.transform.position;
        return targetPosition;
    }
}