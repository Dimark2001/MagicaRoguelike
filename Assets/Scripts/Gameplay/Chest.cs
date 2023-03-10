using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    [SerializeField] private ChestType chestType;
    [SerializeField] private PriceType priceType;
    [SerializeField] private int price;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AbilityManager.Instance.GiveRandomItem(chestType);
        }
    }
}
