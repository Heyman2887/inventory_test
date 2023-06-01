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
        this.transform.GetChild(1).GetComponent<Text>().color = new Color(0, 0, 0, 0);
    }
}
