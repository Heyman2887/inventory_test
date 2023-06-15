using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearData : MonoBehaviour
{
    public Inventory[] Bag;
    public Item[] Items;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        ClearAllData();
    }

    private void ClearAllData()
    {
        for(int i = 0; i < 4; i++)
        {
            Bag[i].itemList.Clear();
        }

        for(int i = 0; i < 8; i++)
        {
            Items[i].itemCount = 0;
        }
    }
}
