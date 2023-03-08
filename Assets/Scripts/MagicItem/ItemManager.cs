using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public List<Items> playerItemsList;
    public List<Items> itemsList;

    public void GiveRandomItem()
    {
        
        var l = ItemManager.Instance.itemsList.Count;
        if (l > 0)
        {
            var a = Random.Range(0, l);
            ItemManager.Instance.playerItemsList.Add(ItemManager.Instance.itemsList[a]);
            ItemManager.Instance.itemsList.Remove(ItemManager.Instance.itemsList[a]);
            print(a);
            GameObject.Instantiate(playerItemsList[playerItemsList.Count() - 1], transform);
        }
    }
}
