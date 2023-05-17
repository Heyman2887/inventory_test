using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject bagPanel;
    public Toggle[] toggle;
    private bool isBagOpen;
    static UIManager instance;

    public Inventory[] Bag;

    public GameObject[] slotGrid;
    public Slot slotPrefab;
    public Text itemName;
    public Text itemInfromation;
    public Image itemImage;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private void Start()
    {
        bagPanel.SetActive(false);
        slotGrid[0].SetActive(true);
        for(int i = 0; i < Bag.Length; i++)
        {
            for (int j = 0; j < instance.Bag[i].itemList.Count; j++)
            {
                CreatNewItem(instance.Bag[i].itemList[j], i);
            }
        }    
    }

    public static void OnClickBagHandler()
    {
        instance.isBagOpen = true;
    }

    public static void OnClickToggleHandler()
    {
        for (int i = 0; i < instance.toggle.Length; i++)
        {
            instance.slotGrid[i].SetActive(false);
            if (instance.toggle[i].isOn == true)
            {
                instance.slotGrid[i].SetActive(true);
            }
        }
    }

    private void Update()
    {
        OpenBag();
    }
    private void OpenBag()
    {
        if (isBagOpen || Input.GetKeyDown(KeyCode.I))
        {
            bagPanel.SetActive(!bagPanel.activeSelf);
            isBagOpen = false;
        }
    }

    private void OnEnable()
    {
        instance.itemInfromation.text = "";
        instance.itemImage.sprite = null;
        instance.itemName.text = "";
    }

    public static void UpdateItemInfo(Item item)
    {
        instance.itemName.text = item.itemName;
        instance.itemInfromation.text = item.itemDescription;
        instance.itemImage.sprite = item.itemImage;
    }

    public static void CreatNewItem(Item item, int InventoryType)
    {
        Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid[InventoryType].transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemCount.ToString();
    }

    public static void RefreshItem(Item item, int InventoryType)
    {
        for (int i = 0; i < instance.slotGrid[InventoryType].transform.childCount; i++)
        {
            if (instance.slotGrid[InventoryType].transform.GetChild(i).GetComponent<Slot>().slotItem.itemGlobalID == item.itemGlobalID)
            {
                instance.slotGrid[InventoryType].transform.GetChild(i).GetComponent<Slot>().slotNum.text = item.itemCount.ToString();
            }
        }
    }
}
