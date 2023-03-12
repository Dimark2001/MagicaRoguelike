using System.Linq;
using Gameplay.Weapon;
using UnityEngine;
using Random = UnityEngine.Random;

public class AuraNegativeGravity : MonoBehaviour
{
    [SerializeField] private float rad;
    [SerializeField] private float power;
    private void FixedUpdate()
    {
        var hitColliders = Physics.OverlapSphere(transform.position, rad);
        if (!hitColliders.ToList().Any(col => col.GetComponent<EnemyProjectile>())) return;
        
        var findAddProjectile = hitColliders.ToList().FindAll(col => col.transform.CompareTag("EnemyProjectile"));
        if(findAddProjectile.Count == 0) return;
        var selectedRb = findAddProjectile.Select(rb => rb.GetComponent<Rigidbody>()).ToList();
        foreach (var rb in selectedRb)
        {
            rb.AddForce((rb.transform.position - transform.position) * power, ForceMode.Force);
            rb.transform.rotation = Quaternion.Euler(rb.transform.rotation.x, Random.Range(-2, 1) * 90, rb.transform.rotation.z);
        }
    }
}
