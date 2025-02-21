using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    private List<Item> inventory;

    public List<Item> PlayerInventory { get { return inventory; } }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = new List<Item>();
    }

    public void AddItem(Item newItem)
    {
        inventory.Add(newItem);
        InventoryUIManager.Instance.ItemAdded(newItem);
    }

    public void  RemoveItem(Item itemToRemove)
    {
        // remove item from inventory
        int index = inventory.IndexOf(itemToRemove);
        inventory.RemoveAt(index);

        // remove item from ui
        InventoryUIManager.Instance.ItemRemoved(index);
    }
}
