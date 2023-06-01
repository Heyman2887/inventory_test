using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;

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
                UIManager.AddNewItem(thisItem.itemType - 1, thisItem);
                break;

            case 2:
                UIManager.AddNewItem(thisItem.itemType - 1, thisItem);
                break;

            case 3:
                UIManager.AddNewItem(thisItem.itemType - 1, thisItem);
                break;

            case 4:
                UIManager.AddNewItem(thisItem.itemType - 1, thisItem);
                break;
        }
    }
}
