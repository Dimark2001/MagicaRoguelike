using UnityEngine;

public abstract class AttackController : MonoBehaviour
{
    protected BaseCharacter BaseCharacter;
    protected TargetPointer TargetPointer;

    private void Start()
    {
        BaseCharacter = GetComponent<BaseCharacter>();
        TargetPointer = GetComponent<TargetPointer>();
    }

    public void PerformAttack()
    {
        Attack();
    }
    
    public void PerformProtection()
    {
        PProtection();
    }

    protected abstract void Attack();
    protected abstract void PProtection();
}

