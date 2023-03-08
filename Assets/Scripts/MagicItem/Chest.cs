using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ItemManager.Instance.GiveRandomItem();
        }
    }
}
