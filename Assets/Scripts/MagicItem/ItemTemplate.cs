using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "ItemTemplate", order = 1)]
public class ItemTemplate : ScriptableObject
{
    
    // [System.Serializable]
    // public struct Ability
    // {
    //     public string name;
    //     public int value;
    [SerializeField]
    
    public string itemName;
    public Image iconItem;
    public string description;
    public string type;
    // public List<Ability> Abilities;
}

