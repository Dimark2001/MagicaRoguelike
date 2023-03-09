using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AuraNegativeGravity : MonoBehaviour
{
    [SerializeField] private float rad;
    [SerializeField] private float power;
    private void FixedUpdate()
    {
        var hitColliders = Physics.OverlapSphere(transform.position, rad);
        var findAllRb = hitColliders.ToList()
            .FindAll(col =>  col.transform.CompareTag("EnemyProjectile"))
            .Select(rb => rb.GetComponent<Rigidbody>()).ToList();
        foreach (var rb in findAllRb)
        {
            //rb.AddForce((rb.transform.position - transform.position) * power, ForceMode.Force);
            rb.transform.rotation = Quaternion.Euler(rb.transform.rotation.x, Random.Range(-2, 1) * 90, rb.transform.rotation.z);
        }
    }
}
