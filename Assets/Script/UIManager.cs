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
        //��Ʒ��Ϣ��ʾ�ÿ�
        instance.itemInfromation.text = "";
        instance.itemImage.sprite = null;
        instance.itemName.text = "";
    }

    //���������ť�򿪱���
    public static void OnClickBagHandler()
    {
        instance.isBagOpen = true;
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

    //��Ʒ��Ϣ��ʾ
    public static void UpdateItemInfo(Item item)
    {
        instance.itemName.text = item.itemName;
        instance.itemInfromation.text = item.itemDescription;
        instance.itemImage.sprite = item.itemImage;
    }

    public static void AddNewItem(int inventoryType,Item item)
    {
        //�ж��Ƿ���list�У����������add��list
        if (!instance.Bag[inventoryType].itemList.Contains(item))
        {
            instance.Bag[inventoryType].itemList.Add(item);
            item.itemCount++;
            item.isNewItem = true;
            //����ͬʱ����UI����õ�����Ʒʵʱ��ʾ��UI��
            instance.Bag[inventoryType].itemList.Sort();
            UIManager.RefreshItem(inventoryType);
        }
        else
        {
            item.itemCount++;
            UIManager.RefreshItem(inventoryType);
        }
    }

    //���slot��grid
    public static void CreatNewItem(Item item, int InventoryType)
    {
        //�ж���ӵ�grid�е�slot�е�Item�����Ƿ����99������ݹ鵽��һ�㣬����һ���µ�slot
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

            //��¡һ��item����Ϊ�������ݵ���һ��ݹ�
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

    //ˢ��grid
    public static void RefreshItem(int InventoryType) 
    {
        int sum = 0;
        for (int i = 0; i < instance.slotGrid[InventoryType].transform.childCount; i++)
        {
            Destroy(instance.slotGrid[InventoryType].transform.GetChild(i).gameObject);
        }

        //�жϵ�ǰ������UI�е�slot�����Ƿ���ڱ�����grid������������
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
