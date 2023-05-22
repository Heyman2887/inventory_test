using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIManager : MonoBehaviour
{
    public GameObject bagPanel;
    public GameObject descriptionPanel;
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
        descriptionPanel.SetActive(false);
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

    //点击背包按钮打开背包
    public static void OnClickBagHandler()
    {
        instance.isBagOpen = true;
    }

    //点击物品查看详细信息
    public static void OnClickItemHandler()
    {
        instance.descriptionPanel.SetActive(true);
    }

    //点击物品分类按钮，切换不同类型物品页面
    public static void OnClickToggleHandler()
    {
        instance.descriptionPanel.SetActive(false);

        for (int i = 0; i < instance.toggle.Length; i++)
        {
            instance.slotGrid[i].SetActive(false);
            if (instance.toggle[i].isOn)
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

    public static void AddNewItem(int inventoryType,Item item)
    {
        //判断是否在list中，如果不在则add到list
        if (!instance.Bag[inventoryType].itemList.Contains(item))
        {
            instance.Bag[inventoryType].itemList.Add(item);
            item.itemCount++;
            item.isNewItem = true;
            //排序，同时更新UI，获得的新物品实时显示在UI中
            instance.Bag[inventoryType].itemList.Sort();
            UIManager.RefreshItem(inventoryType);
        }
        else
        {
            item.itemCount++;
            UIManager.RefreshItem(inventoryType);
        }
    }

    //添加slot到grid
    public static void CreatNewItem(Item item, int InventoryType)
    {
        //判断添加到grid中的slot中的Item数量是否大于99，是则递归到下一层，生成一个新的slot
        if (item.itemCount > 99)
        {
            Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid[InventoryType].transform);
            newItem.slotItem = item;
            newItem.slotImage.sprite = item.itemImage;
            newItem.slotNum.text = "99";
            if (item.isNewItem == true)
            {
                newItem.transform.GetChild(1).GetComponent<Text>().color = new Color(1, 0.7f, 0.2f, 1);
            }
            else
            {
                newItem.newItem.text = "";
            }

            //克隆一份item，作为参数传递到下一层递归
            Item _item = ScriptableObject.CreateInstance<Item>();

            _item.itemGlobalID = item.itemGlobalID;
            _item.itemPartID = item.itemPartID;
            _item.itemType = item.itemType;
            _item.itemUsageTime = item.itemUsageTime;
            _item.itemCount = item.itemCount;
            _item.itemName = item.itemName;
            _item.itemDescription = item.itemDescription;
            _item.itemImage = item.itemImage;
            _item.isNewItem = false;

            _item.itemCount -= 99;
            CreatNewItem(_item, InventoryType);
        }

        else
        {
            Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid[InventoryType].transform);
            newItem.slotItem = item;
            newItem.slotImage.sprite = item.itemImage;
            newItem.slotNum.text = item.itemCount.ToString();
            if (item.isNewItem == true)
            {
                newItem.transform.GetChild(1).GetComponent<Text>().color = new Color(1, 0.7f, 0.2f, 1);
            }
            else
            {
                newItem.newItem.text = "";
            }
        }
    }

    //刷新grid
    public static void RefreshItem(int InventoryType) 
    {
        int sum = 0;
        for (int i = 0; i < instance.slotGrid[InventoryType].transform.childCount; i++)
        {
            Destroy(instance.slotGrid[InventoryType].transform.GetChild(i).gameObject);
        }

        //判断当前物体在UI中的slot数量是否大于背包该grid面板的数量上限
        for (int i = 0; i < instance.Bag[InventoryType].itemList.Count; i++)
        {
            sum += (int)Math.Ceiling((double)instance.Bag[InventoryType].itemList[i].itemCount / 99);
        }

        for (int i = 0; i < instance.Bag[InventoryType].itemList.Count; i++)
        {
            if (sum > 28)
            {
                Debug.Log("The backpack is full!");
            }
            else
            {
                CreatNewItem(instance.Bag[InventoryType].itemList[i], InventoryType);
            }
        }
    }
}
