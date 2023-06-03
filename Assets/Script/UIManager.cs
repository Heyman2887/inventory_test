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

    //存放生成的slot
    private List<Slot> SlotList = new();

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
        DestroySlotInslotGrid();
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
            DestroySlotInslotGrid();
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
                DestroySlotInslotGrid();
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
        DestroySlotInslotGrid();
        RefreshItem(instance.currentInventoryType);
    }

    //分类背包向前翻页
    public static void OnClickPreviousHandler()
    {
        //第一页禁止翻页
        if (instance.pageIndex == 1)
            return;
        instance.pageIndex--;
        DestroySlotInslotGrid();
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
            item.isNewItem = true;
            if (item.itemCount < item.itemMaxCount)
            {
                instance.Bag[inventoryType].itemList.Add(item);
                item.itemCount++;
                //当接触到的物体可以完全放入背包时，添加销毁函数到事件中
                ItemOnWorld.DestoryItem += DestroyItemOnWorld;
            }

            else
            {
                instance.Bag[inventoryType].itemList.Add(item);
                item.itemCount = item.itemMaxCount;
                Debug.Log("超出该物体最大容量，多余的物体无法拾取！");
            }
            //排序，同时更新UI，获得的新物品实时显示在UI中
            instance.Bag[inventoryType].itemList.Sort();
            if (instance.currentInventoryType == inventoryType)
            {
                DestroySlotInslotGrid();
                RefreshItem(inventoryType);
            }
        }
        else
        {
            if (item.itemCount < item.itemMaxCount)
            {
                item.itemCount++;
                item.isNewItem = true;
                //当接触到的物体可以完全放入背包时，添加销毁函数到事件中
                ItemOnWorld.DestoryItem += DestroyItemOnWorld;

                if (instance.currentInventoryType == inventoryType)
                {
                    DestroySlotInslotGrid();
                    RefreshItem(inventoryType);
                }
            }
            else 
            {
                Debug.Log("超出该物体最大容量，无法拾取！");
            }
        }
    }

    //根据传入的item信息，返回slot
    public static Slot CreatNewItem(Item item, int itemCount)
    {
        Slot newItem = Instantiate(instance.slotPrefab);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;

        if (itemCount >= item.itemMaxCountInSlot)
        {
            newItem.slotNum.text = item.itemMaxCountInSlot.ToString();
        }
        else
        {
            newItem.slotNum.text = itemCount.ToString();

            if(item.isNewItem)
                newItem.transform.GetChild(1).GetComponent<Text>().color = new Color(1, 0.7f, 0.2f, 1);
        }

        return newItem;
    }
    
    //根据背包类型，循环调用CreatItem生成slot，并添加到slotList
    public static void RefreshItem(int inventoryType)
    {
        //如果背包为空，直接跳出
        if (instance.Bag[inventoryType].itemList.Count <= 0)
            return;

        //遍历当前背包
        for (int i = 0; i < instance.Bag[inventoryType].itemList.Count; i++)
        {
            //记录item的itemCount
            int itemCount = instance.Bag[inventoryType].itemList[i].itemCount;

            //超过该item的itemMaxCountInSlot即在slot中的最大显示数量,反复调用CreatItem
            for (int j = 0; j < ((int)Math.Ceiling((double)instance.Bag[inventoryType].itemList[i].itemCount / instance.Bag[inventoryType].itemList[i].itemMaxCountInSlot)); j++)
            {
                instance.SlotList.Add(CreatNewItem(instance.Bag[inventoryType].itemList[i],itemCount));
                itemCount -= instance.Bag[inventoryType].itemList[i].itemMaxCountInSlot;
            }
        }

        //计算当前背包分类的总页码
        instance.pageCount = (int)Math.Ceiling((double)instance.SlotList.Count / 28);

        //根据当前的pageIndex来设置slotList[i]的父物体slotGrid
        for (int i = (instance.pageIndex - 1) * 28; i < (((instance.pageIndex - 1) * 28 + 28) > instance.SlotList.Count ? instance.SlotList.Count : ((instance.pageIndex - 1) * 28 + 28)); i++)
        {
            instance.SlotList[i].transform.SetParent(instance.slotGrid.transform);
        }

        //页码显示
        instance.pageInfo.text = string.Format("{0}/{1}", instance.pageIndex.ToString(), instance.pageCount.ToString());
    }
    public static void DestroySlotInslotGrid()
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
        }
        instance.SlotList.Clear();
    }

    //销毁传入的物体
    public static void DestroyItemOnWorld(GameObject gameObject)
    { 
        Destroy(gameObject);
    }
}
