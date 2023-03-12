using System.Linq;
using UnityEngine;

public class GravityShield : Ability
{
    [SerializeField] private float rad;
    private void Update()
    {
        var hitColliders = Physics.OverlapSphere(transform.position, rad);
        var findAllRb = hitColliders.ToList()
            .FindAll(col =>  col.transform.CompareTag("EnemyProjectile"))
            .Select(rb => rb.GetComponent<Rigidbody>()).ToList();
        foreach (var rb in findAllRb)
        {
            rb.transform.LookAt(transform);
        }
    }
}
