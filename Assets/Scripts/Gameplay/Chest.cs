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
    
    private bool isOpen = false;
    private bool isActive = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = false;
        }
    }

    private void Update()
    {
        if (!isOpen && isActive )
        {
            if (priceType == PriceType.Gold && price <= LevelManager.Instance.Coins)
            {
                LevelManager.Instance.Coins -= price;
                OpenChest();
            }

            if (priceType == PriceType.Blood)
            {
                LevelManager.Instance.player.TakeDamage(price, DamageType.Ability, null);
                OpenChest();
            }
        }
    }

    void OpenChest()
    {
        AbilityManager.Instance.GiveRandomItem(chestType);     
        isActive = false;
        isOpen = true;
        EventGameManager.Instance.OnCoinChange?.Invoke();
    }
}
