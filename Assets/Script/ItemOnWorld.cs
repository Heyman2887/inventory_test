using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Inventory[] inventory;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AddNewItemToInventory();
            Destroy(gameObject);
        }
    }

    private void AddNewItemToInventory()
    {
        switch (thisItem.itemType)
        {
            case 1:
                if (!inventory[thisItem.itemType - 1].itemList.Contains(thisItem))
                {
                    inventory[thisItem.itemType - 1].itemList.Add(thisItem);
                    thisItem.itemCount++;
                    UIManager.CreatNewItem(thisItem, thisItem.itemType - 1);
                }
                else
                {
                    thisItem.itemCount++;
                    UIManager.RefreshItem(thisItem, thisItem.itemType - 1);
                }
                break;

            case 2:
                if (!inventory[thisItem.itemType - 1].itemList.Contains(thisItem))
                {
                    inventory[thisItem.itemType - 1].itemList.Add(thisItem);
                    thisItem.itemCount++;
                    UIManager.CreatNewItem(thisItem, thisItem.itemType - 1);
                }
                else
                {
                    thisItem.itemCount++;
                    UIManager.RefreshItem(thisItem, thisItem.itemType - 1);
                }
                break;

            case 3:
                if (!inventory[thisItem.itemType - 1].itemList.Contains(thisItem))
                {
                    inventory[thisItem.itemType - 1].itemList.Add(thisItem);
                    thisItem.itemCount++;
                    UIManager.CreatNewItem(thisItem, thisItem.itemType - 1);
                }
                else
                {
                    thisItem.itemCount++;
                    UIManager.RefreshItem(thisItem, thisItem.itemType - 1);
                }
                break;

            case 4:
                if (!inventory[thisItem.itemType - 1].itemList.Contains(thisItem))
                {
                    inventory[thisItem.itemType - 1].itemList.Add(thisItem);
                    thisItem.itemCount++;
                    UIManager.CreatNewItem(thisItem, thisItem.itemType - 1);
                }
                else
                {
                    thisItem.itemCount++;
                    UIManager.RefreshItem(thisItem, thisItem.itemType - 1);
                }
                break;
        }
    }
}
