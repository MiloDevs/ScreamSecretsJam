using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInInventory : MonoBehaviour
{
    public Item item;

    public void addItem(Item itemToAdd)
    { 
        item = itemToAdd;
    }

    public Item getItem()
    { 
        return item;
    }
}
