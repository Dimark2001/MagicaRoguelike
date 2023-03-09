public class ShieldProtection : ProtectionWeapon
{
    protected override void Awake()
    {
        base.Awake();
        EventGameManager.Instance.OnProtected?.Invoke(gameObject);
    }
}
