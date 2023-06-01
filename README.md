# inventory_test

1.这是一个简易的背包项目，实现了添加物品数据到数据库，对物品进行分类，UI实时更新物品信息等

2.该项目整体为MVC架构，使用ScriptableObject类对数据进行存储，ScriptableObject类型数据只存在于单例模式中，UIManager中对外提供数据的读写方法

3.数据结构类有Item，Inventory，其中Item中存储有
    public int itemGlobalID;
    public int itemPartID;
    public int itemType;
    public int itemUsageTime;
    public int itemCount;
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
    
  Inventory中有存储Item的list

4.数据到ui的更新过程：player触碰到物体 -> 获取到物品上的item信息，对item进行判断，如果item不存在于inventory中则添加到list，如果存在则item.count++，同时调用
  RefreshItem()对ui进行更新，RefreshItem()通过销毁当前分类面板的子集slot，再重新生成该面板的子集slot，达到对UI的动态更新



