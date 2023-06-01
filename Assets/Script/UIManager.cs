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

    //��ȡUI���
    public GameObject bagPanel;
    public GameObject descriptionPanel;
    public GameObject slotGrid;
    public Slot slotPrefab;
    public Toggle[] toggle;
    public Text pageInfo;

    //��ȡ��������
    public Inventory[] Bag;

    //��ǰ�������͡�ĳ��������ҳ����ҳ�롢�������ر��
    private int currentInventoryType;
    private int pageCount;
    private int pageIndex;
    private bool isBagOpen;

    //�洢��¡��item
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

    //���������ť�򿪱���
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

    //�����Ʒ�鿴��ϸ��Ϣ
    public static void OnClickItemHandler()
    {
        instance.descriptionPanel.SetActive(true);
    }

    //�����Ʒ���ఴť���л���ͬ������Ʒҳ��
    public static void OnClickToggleHandler()
    {
        instance.descriptionPanel.SetActive(false);

        //�ж��л���ť��isON����currentInventory��Ǵ򿪵�����һ���������࣬ͬʱˢ�µ�ǰ���౳��
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
    
    //���౳�����ҳ
    public static void OnClickNextHandler()
    {
        //���һҳ��ֹ��ҳ
        if (instance.pageIndex == instance.pageCount)
            return;
        if (instance.pageIndex >= instance.pageCount)
            instance.pageIndex = instance.pageCount;
        instance.pageIndex++;
        RefreshItem(instance.currentInventoryType);
    }

    //���౳����ǰ��ҳ
    public static void OnClickPreviousHandler()
    {
        //��һҳ��ֹ��ҳ
        if (instance.pageIndex == 1)
            return;
        instance.pageIndex--;
        RefreshItem(instance.currentInventoryType);
    }

    //��Ʒ��Ϣ��ʾ
    public static void UpdateItemInfo(Item item)
    {
        instance.descriptionPanel.transform.GetChild(0).GetComponent<Text>().text = item.itemDescription;
        instance.descriptionPanel.transform.GetChild(1).GetComponent<Image>().sprite = item.itemImage;
        instance.descriptionPanel.transform.GetChild(2).GetComponent<Text>().text = item.itemName;
    }

    public static void AddNewItem(int inventoryType, Item item)
    {
        //�ж��Ƿ���list�У����������add��list
        if (!instance.Bag[inventoryType].itemList.Contains(item))
        {
            instance.Bag[inventoryType].itemList.Add(item);
            item.itemCount++;
            //����ͬʱ����UI����õ�����Ʒʵʱ��ʾ��UI��
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

    //���ݴ����item��Ϣ��ʵ����һ���µ�slot
    public static void CreatNewItem(Item item)
    {
        Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemCount.ToString();
    }

    //ˢ��grid
    public static void RefreshItem(int inventoryType)
    {
        //����ʱɾ��slotGrid�µ�����������
        DestroyItem();

        //�������Ϊ�գ�ֱ������
        if (instance.Bag[inventoryType].itemList.Count <= 0)
            return;

        //������ǰ����
        for (int i = 0; i < instance.Bag[inventoryType].itemList.Count; i++)
        {
            //��¡һ��item
            Item _item = ScriptableObject.CreateInstance<Item>();

            _item.itemGlobalID = instance.Bag[inventoryType].itemList[i].itemGlobalID;
            _item.itemPartID = instance.Bag[inventoryType].itemList[i].itemPartID;
            _item.itemType = instance.Bag[inventoryType].itemList[i].itemType;
            _item.itemUsageTime = instance.Bag[inventoryType].itemList[i].itemUsageTime;
            _item.itemCount = instance.Bag[inventoryType].itemList[i].itemCount;
            _item.itemName = instance.Bag[inventoryType].itemList[i].itemName;
            _item.itemDescription = instance.Bag[inventoryType].itemList[i].itemDescription;
            _item.itemImage = instance.Bag[inventoryType].itemList[i].itemImage;

            //�жϵ�ǰitem����Ŀ�Ƿ����99������ǣ�������һ������Ϊ99����item���洢����ſ�¡item��list��
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

        //���㵱ǰ�����������ҳ��
        instance.pageCount = (int)Math.Ceiling((double)instance._itemList.Count / 28);

        //���ݵ�ǰ��pageIndex������slot
        for (int i = (instance.pageIndex - 1) * 28; i < (((instance.pageIndex - 1) * 28 + 28) > instance._itemList.Count ? instance._itemList.Count : ((instance.pageIndex - 1) * 28 + 28)); i++)
        {
            CreatNewItem(instance._itemList[i]);
        }

        //ҳ����ʾ
        instance.pageInfo.text = string.Format("{0}/{1}", instance.pageIndex.ToString(), instance.pageCount.ToString());

        instance._itemList.Clear();
    }

    //ɾ��slotGrid�µ�����������
    public static void DestroyItem()
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
        }
    }
}
