using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Items : MonoBehaviour
{
    public string itemName;
    public Image iconItem;
    public string description;
    public string type;

    protected abstract void Start();
    protected abstract void OnDisable();

    protected abstract void ActivateItem();
}
