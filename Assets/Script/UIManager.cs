using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIManager : MonoBehaviour
{
    static UIManager instance;

    //获取UI组件
    public GameObject bagPanel;
    public GameObject descriptionPanel;
    public GameObject slotGrid;
    public Slot slotPrefab;
    public Toggle[] toggle;
    public Text pageInfo;

    //获取背包数据
    public Inventory[] Bag;

    //当前背包类型、某个背包总页数、页码、背包开关标记
    private int currentInventoryType;
    private int pageCount;
    private int pageIndex;
    private bool isBagOpen;

    //存储克隆的item
    private List<Item> _itemList = new();


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
        currentInventoryType = 0;
        pageCount = 0;
        pageIndex = 1;
    }

    private void Update()
    {
        OpenBag();
    }

    //点击背包按钮打开背包
    public static void OnClickBagHandler()
    {
        instance.isBagOpen = true;
    }
    private static void OpenBag()
    {
        if (instance.isBagOpen || Input.GetKeyDown(KeyCode.I))
        {
            instance.bagPanel.SetActive(!instance.bagPanel.activeSelf);
            RefreshItem(0);
            instance.isBagOpen = false;
        }
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

        //判断切换按钮的isON，用currentInventory标记打开的是哪一个背包分类，同时刷新当前分类背包
        for (int i = 0; i < instance.toggle.Length; i++)
        {
            if (instance.toggle[i].isOn)
            {
                instance.currentInventoryType = i;
                instance.pageIndex = 1;
                RefreshItem(instance.currentInventoryType);
            }
        }
    }
    
    //分类背包向后翻页
    public static void OnClickNextHandler()
    {
        //最后一页禁止翻页
        if (instance.pageIndex == instance.pageCount)
            return;
        if (instance.pageIndex >= instance.pageCount)
            instance.pageIndex = instance.pageCount;
        instance.pageIndex++;
        RefreshItem(instance.currentInventoryType);
    }

    //分类背包向前翻页
    public static void OnClickPreviousHandler()
    {
        //第一页禁止翻页
        if (instance.pageIndex == 1)
            return;
        instance.pageIndex--;
        RefreshItem(instance.currentInventoryType);
    }

    //物品信息显示
    public static void UpdateItemInfo(Item item)
    {
        instance.descriptionPanel.transform.GetChild(0).GetComponent<Text>().text = item.itemDescription;
        instance.descriptionPanel.transform.GetChild(1).GetComponent<Image>().sprite = item.itemImage;
        instance.descriptionPanel.transform.GetChild(2).GetComponent<Text>().text = item.itemName;
    }

    public static void AddNewItem(int inventoryType, Item item)
    {
        //判断是否在list中，如果不在则add到list
        if (!instance.Bag[inventoryType].itemList.Contains(item))
        {
            instance.Bag[inventoryType].itemList.Add(item);
            item.itemCount++;
            //排序，同时更新UI，获得的新物品实时显示在UI中
            instance.Bag[inventoryType].itemList.Sort();
            if (instance.currentInventoryType == inventoryType)
                RefreshItem(inventoryType);
        }
        else
        {
            item.itemCount++;
            if (instance.currentInventoryType == inventoryType)
                RefreshItem(inventoryType);
        }
    }

    //根据传入的item信息，实例化一个新的slot
    public static void CreatNewItem(Item item)
    {
        Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemCount.ToString();
    }

    //刷新grid
    public static void RefreshItem(int inventoryType)
    {
        //调用时删除slotGrid下的所有子物体
        DestroyItem();

        //如果背包为空，直接跳出
        if (instance.Bag[inventoryType].itemList.Count <= 0)
            return;

        //遍历当前背包
        for (int i = 0; i < instance.Bag[inventoryType].itemList.Count; i++)
        {
            //克隆一份item
            Item _item = ScriptableObject.CreateInstance<Item>();

            _item.itemGlobalID = instance.Bag[inventoryType].itemList[i].itemGlobalID;
            _item.itemPartID = instance.Bag[inventoryType].itemList[i].itemPartID;
            _item.itemType = instance.Bag[inventoryType].itemList[i].itemType;
            _item.itemUsageTime = instance.Bag[inventoryType].itemList[i].itemUsageTime;
            _item.itemCount = instance.Bag[inventoryType].itemList[i].itemCount;
            _item.itemName = instance.Bag[inventoryType].itemList[i].itemName;
            _item.itemDescription = instance.Bag[inventoryType].itemList[i].itemDescription;
            _item.itemImage = instance.Bag[inventoryType].itemList[i].itemImage;

            //判断当前item的数目是否大于99，如果是，则生成一个数量为99的新item，存储到存放克隆item的list中
            for (int j = 0; j < ((int)Math.Ceiling((double)instance.Bag[inventoryType].itemList[i].itemCount / 99)); j++)
            {
                if (_item.itemCount > 99)
                {
                    Item _item1 = ScriptableObject.CreateInstance<Item>();

                    _item1.itemGlobalID = _item.itemGlobalID;
                    _item1.itemPartID = _item.itemPartID;
                    _item1.itemType = _item.itemType;
                    _item1.itemUsageTime = _item.itemUsageTime;
                    _item1.itemCount = 99;
                    _item1.itemName = _item.itemName;
                    _item1.itemDescription = _item.itemDescription;
                    _item1.itemImage = _item.itemImage;

                    instance._itemList.Add(_item1);
                    _item.itemCount -= 99;
                }
                else
                {
                    instance._itemList.Add(_item);
                }
            }
        }

        //计算当前背包分类的总页码
        instance.pageCount = (int)Math.Ceiling((double)instance._itemList.Count / 28);

        //根据当前的pageIndex来创建slot
        for (int i = (instance.pageIndex - 1) * 28; i < (((instance.pageIndex - 1) * 28 + 28) > instance._itemList.Count ? instance._itemList.Count : ((instance.pageIndex - 1) * 28 + 28)); i++)
        {
            CreatNewItem(instance._itemList[i]);
        }

        //页码显示
        instance.pageInfo.text = string.Format("{0}/{1}", instance.pageIndex.ToString(), instance.pageCount.ToString());

        instance._itemList.Clear();
    }

    //删除slotGrid下的所有子物体
    public static void DestroyItem()
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
        }
    }
}
