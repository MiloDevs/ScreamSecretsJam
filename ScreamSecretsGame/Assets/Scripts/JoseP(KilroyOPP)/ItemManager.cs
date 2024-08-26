using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour
{

    public void HandleItem(Item item)
    {
        GameObject.FindObjectOfType<InventorySystem>().TakeItem(item);
        GameObject.FindObjectOfType<InventorySystem>().ListItems();
    }

   
}
