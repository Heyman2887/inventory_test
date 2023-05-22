using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item slotItem;
    public Image slotImage;
    public Text slotNum;
    public Text newItem;

    public void ItemOnClicked()
    {
        UIManager.UpdateItemInfo(slotItem);
        UIManager.OnClickItemHandler();
        slotItem.isNewItem = false;
        for (int i = 0; i < 4; i++)
        {
            UIManager.RefreshItem(i);
        }
    }
}
