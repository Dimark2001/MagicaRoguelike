using System.Linq;
using Gameplay.Weapon;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseCharacter : MonoBehaviour
{
    public Rigidbody rb;
    public NavMeshAgent navMeshAgent;
    public Renderer characterRenderer;
    public Weapon[] projectilePrefabs;
    public Weapon[] meleeWeaponPrefabs;
    public Weapon[] protectionsPrefab;
    
    public int hp;    
    public float attackCooldown;
    public float protectionCooldown;
    public bool isKnockBack = false;
    [HideInInspector] public float timeShoot = 0;


    private Weapon _weaponPrefab;

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