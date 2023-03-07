using UnityEngine;

public abstract class TargetPointer : MonoBehaviour
{
    public abstract float GetAttackAngle();
    public abstract Vector3 GetTargetPosition();
}
