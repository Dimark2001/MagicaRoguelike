using System.Linq;
using Gameplay.Weapon;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseCharacter : MonoBehaviour
{
    [SerializeField] public Rigidbody rb;
    [SerializeField] public NavMeshAgent navMeshAgent;
    [SerializeField] public Renderer characterRenderer;
    [SerializeField] public Weapon[] projectilePrefabs;
    [SerializeField] public Weapon[] meleeWeaponPrefabs;
    [SerializeField] public Weapon[] protectionsPrefab;
    private Weapon _weaponPrefab;

    [SerializeField] public int hp;    
    [HideInInspector] public float timeLastShoot;
    [HideInInspector] public float timeShoot = 0;

    public bool isKnockBack = false;

    public void SetWeaponPrefab(Weapon[] weapons, int index)
    {
        _weaponPrefab = weapons[index];
    }

    public void SetWeaponPrefab(Weapon[] weapons)
    {
        _weaponPrefab = weapons.First();
    }

    public T GetWeaponPrefab<T>() where T: Weapon
    {
        if (_weaponPrefab == null)
            return null;
    
        return (T)_weaponPrefab;
    }
}