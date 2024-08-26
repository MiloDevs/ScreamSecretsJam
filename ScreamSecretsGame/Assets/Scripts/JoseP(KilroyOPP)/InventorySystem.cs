using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventorySystem : MonoBehaviour
{

    private List<Item> Items = new List<Item>();
    [SerializeField] List<ItemInInventory> itemPlaceHolders;

    [SerializeField] Transform inventoryContent;
    [SerializeField] GameObject itemPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TakeItem(Item itemToTake)
    {
       Items.Add(itemToTake);
       itemPlaceHolders = inventoryContent.gameObject.GetComponentsInChildren<ItemInInventory>().ToList<ItemInInventory>();
    }

    public void RemoveItem(Item itemToRemove)
    { 
        SetInventoryItems(itemToRemove);
        Items.Remove(itemToRemove);
        
    }

    void SetInventoryItems(Item itemInventory)
    {
       itemPlaceHolders = inventoryContent.gameObject.GetComponentsInChildren<ItemInInventory>().ToList<ItemInInventory>();

        foreach (ItemInInventory item in itemPlaceHolders)
        {
            if (item.item == itemInventory)
            { 
                item.gameObject.SetActive(false);
            }
        }
    }

    public void ListItems()
    {
        foreach (var item in Items)
        {
            
            GameObject obj = Instantiate(itemPrefab, inventoryContent);
            
            obj.GetComponent<Image>().sprite = item.sprite;
            obj.GetComponent<ItemInInventory>().addItem(item);

        }
    }

    public bool playerHasThisItem(Item item)
    { 
        return Items.Contains(item);
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
