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

        RefreshItem(0);
    }

    private void Update()
    {
        OpenBag();
    }

    private void OnEnable()
    {
        //物品信息显示置空
        instance.itemInfromation.text = "";
        instance.itemImage.sprite = null;
        instance.itemName.text = "";
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
            RefreshItem(i);
        }
    }
    private void OpenBag()
    {
        if (isBagOpen || Input.GetKeyDown(KeyCode.I))
        {
            bagPanel.SetActive(!bagPanel.activeSelf);
            isBagOpen = false;
        }
    }

    //物品信息显示
    public static void UpdateItemInfo(Item item)
    {
        instance.itemName.text = item.itemName;
        instance.itemInfromation.text = item.itemDescription;
        instance.itemImage.sprite = item.itemImage;
    }

    //添加slot到grid
    public static void CreatNewItem(Item item, int InventoryType)
    {
        Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid[InventoryType].transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemCount.ToString();
    }

    //增加相同的物品数量
    public static void DuplicateItem(Item item, int InventoryType)
    {
        for (int i = 0; i < instance.slotGrid[InventoryType].transform.childCount; i++)
        {
            if (instance.slotGrid[InventoryType].transform.GetChild(i).GetComponent<Slot>().slotItem.itemGlobalID == item.itemGlobalID)
            {
                instance.slotGrid[InventoryType].transform.GetChild(i).GetComponent<Slot>().slotNum.text = item.itemCount.ToString();
            }
        }
    }

    //刷新grid
    public static void RefreshItem(int InventoryType) 
    {
        for (int i = 0; i < instance.slotGrid[InventoryType].transform.childCount; i++)
        {
            Destroy(instance.slotGrid[InventoryType].transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < instance.Bag[InventoryType].itemList.Count; i++)
        {
            CreatNewItem(instance.Bag[InventoryType].itemList[i], InventoryType);
        }
    }
}
