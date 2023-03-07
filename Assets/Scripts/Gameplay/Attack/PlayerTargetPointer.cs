using UnityEngine;

public class PlayerTargetPointer : TargetPointer
{
    public override float GetAttackAngle()
    {
        var player = LevelManager.Instance.player;
        
        var playerPos = player.transform.position;
        var mousePos = player.GetMouseAngle();
        var angle = Mathf.Atan2(mousePos.x - playerPos.x, mousePos.z - playerPos.z) * Mathf.Rad2Deg;
        
        return angle;
    }

    public override Vector3 GetTargetPosition()
    {
        throw new System.NotImplementedException();
    }
}