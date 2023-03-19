using System;
using System.Linq;
using DG.Tweening;
using Gameplay.Character;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    [SerializeField] private ChestType chestType;
    [SerializeField] private PriceType priceType;
    [SerializeField] private int price;
    [SerializeField] private TextMeshPro priceText;
    
    
    private bool isOpen = false;
    private bool isActive = false;

    private void Start()
    {
        if (priceType == PriceType.Gold)
        {
            priceText.color = Color.yellow;
            price *= LevelManager.Instance.countLevel;
        }
        else
        {
            priceText.color = Color.red;
            if (LevelManager.Instance.countLevel <= 9)
                price = (int)(Player.Instance.maxHp * 0.1f * (LevelManager.Instance.countLevel));
            else
                price = (int)(Player.Instance.maxHp * 0.9f);
        }
        priceText.text = price.ToString();
    }

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
        priceText.transform.rotation = LevelManager.Instance.thisCamera.transform.rotation;
        if (!isOpen && isActive)
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
        transform.DOScale(Vector3.zero, 0.6f).OnComplete((() =>
        {
            Destroy(gameObject);
        }));
    }
}
