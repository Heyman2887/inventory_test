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

    //������ɵ�slot
    private List<Slot> SlotList = new();

    //��ȡ���
    public Player player;
    //��ȡ��������
    public GameObject normalButton;
    public GameObject specialButton;
    public GameObject limitedButton;

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
        //DoesBagHaveSpecialItem();
    }

    private void Update()
    {
        OpenBag();
        DoesBagHaveSpecialItem();
        DoesBagHaveLimitedItem();
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
            DestroySlotInslotGrid();
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
                DestroySlotInslotGrid();
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
        DestroySlotInslotGrid();
        RefreshItem(instance.currentInventoryType);
    }

    //���౳����ǰ��ҳ
    public static void OnClickPreviousHandler()
    {
        //��һҳ��ֹ��ҳ
        if (instance.pageIndex == 1)
            return;
        instance.pageIndex--;
        DestroySlotInslotGrid();
        RefreshItem(instance.currentInventoryType);
    }

    public static void OnClickNormalHandler()
    {
        Collider collider = instance.player.DetectItem();
        if (collider != null)
        {
            //�ж�Tag,�Բ�ͬ�Ŀɽ���������в���
            if(collider.tag == "Door")
            {
                Interactable interactable = collider.GetComponent<Interactable>();
                interactable.OnClick();
            }
        }

        //TODO:����������ж�ʹ��ͨ�õİ������ԶԶ�����Ʒ���н���
    }

    public static void OnClickSpecialHandler()
    {
        if(instance.Bag[1].itemList.Exists(i => i.itemName == "����"))
        {
            Debug.Log("ʹ���˻���");
        }
    }

    public static void OnClickLimitedHandler()
    {
        Collider collider = instance.player.DetectItem();
        if (collider != null)
        {
            //�ж�Tag,�Բ�ͬ�Ŀɽ���������в���
            if (collider.tag == "Limited Box" && instance.Bag[3].itemList.Exists(i => i.itemName == "��ʱ����"))
            {
                Interactable interactable = collider.GetComponent<Interactable>();
                interactable.OnClick();
                instance.limitedButton.SetActive(false);
                Item item = instance.Bag[3].itemList.Find(i => i.itemName == "��ʱ����");
                item.itemCount--;
                instance.Bag[3].itemList.Remove(item);
                DestroySlotInslotGrid();
            }
        }
    }

    //���ұ������Ƿ���������ߣ����Ѱ�ťͼƬ����Ϊ�������ͼƬ
    public static void DoesBagHaveSpecialItem()
    {
        Item item = instance.Bag[1].itemList.Find(i => i.itemName == "����");
        if (item != null)
        {
            //���õ�ǰ��ť��ͼ��Ϊ���������ͼ��
            instance.specialButton.GetComponent<Image>().sprite = item.itemImage;
        }
    }

    //���ұ������Ƿ�����ʱ���ߣ��������������ʱ��ť�����Ѱ�ťͼƬ����Ϊ��ʱ����ͼƬ
    public static void DoesBagHaveLimitedItem()
    {
        //�������������Ƿ��и���ʱ����
        Item item = instance.Bag[3].itemList.Find(i => i.itemName == "��ʱ����");
        //���õ�ǰ��ť��ͼ��Ϊ���������ͼ��
        if (item != null)
        {
            instance.limitedButton.GetComponent<Image>().sprite = item.itemImage;
            instance.limitedButton.SetActive(true);
        }
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
            item.isNewItem = true;
            if (item.itemCount < item.itemMaxCount)
            {
                instance.Bag[inventoryType].itemList.Add(item);
                item.itemCount++;
                //���Ӵ��������������ȫ���뱳��ʱ��������ٺ������¼���
                ItemOnWorld.DestoryItem += DestroyItemOnWorld;
            }

            else
            {
                instance.Bag[inventoryType].itemList.Add(item);
                item.itemCount = item.itemMaxCount;
                Debug.Log("�����������������������������޷�ʰȡ��");
            }
            //����ͬʱ����UI����õ�����Ʒʵʱ��ʾ��UI��
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
                //���Ӵ��������������ȫ���뱳��ʱ��������ٺ������¼���
                ItemOnWorld.DestoryItem += DestroyItemOnWorld;

                if (instance.currentInventoryType == inventoryType)
                {
                    DestroySlotInslotGrid();
                    RefreshItem(inventoryType);
                }
            }
            else 
            {
                Debug.Log("��������������������޷�ʰȡ��");
            }
        }
    }

    //���ݴ����item��Ϣ������slot
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
    
    //���ݱ������ͣ�ѭ������CreatItem����slot������ӵ�slotList
    public static void RefreshItem(int inventoryType)
    {
        //�������Ϊ�գ�ֱ������
        if (instance.Bag[inventoryType].itemList.Count <= 0)
            return;

        //������ǰ����
        for (int i = 0; i < instance.Bag[inventoryType].itemList.Count; i++)
        {
            //��¼item��itemCount
            int itemCount = instance.Bag[inventoryType].itemList[i].itemCount;

            //������item��itemMaxCountInSlot����slot�е������ʾ����,��������CreatItem
            for (int j = 0; j < ((int)Math.Ceiling((double)instance.Bag[inventoryType].itemList[i].itemCount / instance.Bag[inventoryType].itemList[i].itemMaxCountInSlot)); j++)
            {
                instance.SlotList.Add(CreatNewItem(instance.Bag[inventoryType].itemList[i],itemCount));
                itemCount -= instance.Bag[inventoryType].itemList[i].itemMaxCountInSlot;
            }
        }

        //���㵱ǰ�����������ҳ��
        instance.pageCount = (int)Math.Ceiling((double)instance.SlotList.Count / 28);

        //���ݵ�ǰ��pageIndex������slotList[i]�ĸ�����slotGrid
        for (int i = (instance.pageIndex - 1) * 28; i < (((instance.pageIndex - 1) * 28 + 28) > instance.SlotList.Count ? instance.SlotList.Count : ((instance.pageIndex - 1) * 28 + 28)); i++)
        {
            instance.SlotList[i].transform.SetParent(instance.slotGrid.transform);
        }

        //ҳ����ʾ
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

    //���ٴ��������
    public static void DestroyItemOnWorld(GameObject gameObject)
    { 
        Destroy(gameObject);
    }
}
