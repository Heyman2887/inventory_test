using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class Item : ScriptableObject,IComparable<Item>
{
    public int itemGlobalID;
    public int itemPartID;
    public int itemType;
    public int itemUsageTime;
    public int itemCount;
    public int itemMaxCount;
    public int itemMaxCountInSlot;
    public string itemName;
    [TextArea]
    public string itemDescription;
    public Sprite itemImage;
    public bool isNewItem;

    public int CompareTo(Item other)
    {
        if (this.itemGlobalID > other.itemGlobalID) return 1;
        else return -1;
    }
}
