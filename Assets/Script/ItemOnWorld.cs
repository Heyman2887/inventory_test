using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
                AddNewItem(thisItem.itemType - 1);
                break;

            case 2:
                AddNewItem(thisItem.itemType - 1);
                break;

            case 3:
                AddNewItem(thisItem.itemType - 1);
                break;

            case 4:
                AddNewItem(thisItem.itemType - 1);
                break;
        }
    }

    private void AddNewItem(int inventoryType)
    {
        //判断是否在list中，如果不在则add到list
        if (!inventory[inventoryType].itemList.Contains(thisItem))
        {
            inventory[inventoryType].itemList.Add(thisItem);
            thisItem.itemCount++;
            thisItem.isNewItem = true;
            //排序，同时更新UI，获得的新物品实时显示在UI中
            inventory[inventoryType].itemList.Sort();
            UIManager.RefreshItem(inventoryType);
        }
        else
        {
            thisItem.itemCount++;
            UIManager.RefreshItem(inventoryType);
        }
    }
}
