using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    public int itemGlobalID;
    public int itemPartID;
    public int itemType;
    public int itemUsageTime;
    public int itemCount;
    public string itemName;
    [TextArea]
    public string itemDescription;
    public Sprite itemImage;
}
