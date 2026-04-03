using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ToolGood.Words;
using UnityEngine;
public class ItemModel
{
    [JsonProperty] private List<Item> items = new List<Item>();
    [JsonIgnore] public List<Item> Items => items;
    [JsonIgnore] private Dictionary<string, Item> itemDicByUid;
    [JsonIgnore] private Dictionary<string, List<Item>> itemDicByDefId;
    private void Init()
    {
        itemDicByDefId = new Dictionary<string, List<Item>>();
        itemDicByUid = new Dictionary<string, Item>();
        foreach (Item item in items)
        {
            itemDicByUid.Add(item.UID, item);
            if (itemDicByDefId.TryGetValue(item.ID, out List<Item> defIdList))
            {
                defIdList.Add(item);
                continue;
            }
            itemDicByDefId.Add(item.ID, new List<Item>() { item });
        }
    }
    public void AddItem(Item item)
    {
        if (item is IStackable stackableItem)
        {
            if (itemDicByDefId.TryGetValue(item.ID, out List<Item> defIdList))
            {
                foreach (Item existItem in defIdList)
                {
                    if (existItem is IStackable existStackable)
                    {
                        int overflow = existStackable.AddAmount(stackableItem.Amount);
                        EventCenter.Instance.BroadEvent(E_EventType.SlotContentChange, existItem);
                        if (overflow == 0)
                        {
                            EventCenter.Instance.BroadEvent(E_EventType.ItemChange, item.ID, QueryItemAmount(item.ID));
                            return;
                        }
                        else
                        {
                            stackableItem.RemoveAmount(stackableItem.Amount - overflow);
                        }
                    }
                }
            }
        }
        AddNewItemSlot(item);
        EventCenter.Instance.BroadEvent(E_EventType.ItemChange, item.ID, QueryItemAmount(item.ID));
    }
    public bool RemoveItemByUid(string uid, int amount = 1)
    {
        if (itemDicByUid.TryGetValue(uid, out Item item))
        {
            if (item is IStackable stackableItem)
            {
                if (!stackableItem.RemoveAmount(amount))
                    return false;
                if (stackableItem.Amount > 0)
                {
                    EventCenter.Instance.BroadEvent(E_EventType.ItemChange, item.ID, QueryItemAmount(item.ID));
                    EventCenter.Instance.BroadEvent(E_EventType.SlotContentChange, item);
                    return true;
                }
            }
            RemoveItemSlot(uid);
            EventCenter.Instance.BroadEvent(E_EventType.ItemChange, item.ID, QueryItemAmount(item.ID));
            return true;
        }
        return false;
    }
    public bool RemoveItemByDefId(string defId, int amount = 1)
    {
        if (itemDicByDefId.TryGetValue(defId, out List<Item> defIdList))
        {
            int total = QueryItemAmount(defId);
            if (total > amount)
            {
                for (int i = defIdList.Count - 1; i >= 0; i--)
                {
                    Item currentItem = defIdList[i];
                    if (currentItem is IStackable stackableItem)
                    {
                        if (stackableItem.Amount > amount)
                        {
                            stackableItem.RemoveAmount(amount);
                            EventCenter.Instance.BroadEvent(E_EventType.ItemChange, defId, QueryItemAmount(defId));
                            EventCenter.Instance.BroadEvent(E_EventType.SlotContentChange, currentItem);
                            return true;
                        }
                        else
                        {
                            amount -= stackableItem.Amount;
                            RemoveItemSlot(currentItem.UID);
                        }
                    }
                    else
                    {
                        amount -= 1;
                        RemoveItemSlot(currentItem.UID);
                    }
                    if (amount == 0)
                    {
                        EventCenter.Instance.BroadEvent(E_EventType.ItemChange, defId, QueryItemAmount(defId));
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public int QueryItemAmount(string defId)
    {
        if (itemDicByDefId.TryGetValue(defId, out List<Item> defIdList))
        {
            int total = 0;
            foreach (Item item in defIdList)
            {
                if (item is IStackable stackableItem)
                    total += stackableItem.Amount;
                else
                    total += 1;
            }
            return total;
        }
        return 0;
    }
    public void TidyItem()
    {
        foreach (string defId in itemDicByDefId.Keys.ToList())
        {
            List<Item> defIdList = itemDicByDefId[defId];
            if (defIdList.Count > 1 && defIdList[0] is IStackable)
            {
                int index = 0;
                int total = QueryItemAmount(defId);
                for (int i = 0; i < defIdList.Count; i++)
                {
                    IStackable stackItem = defIdList[i] as IStackable;
                    stackItem.RemoveAmount(stackItem.Amount);
                    total = stackItem.AddAmount(total);
                    if (total == 0)
                    {
                        index = i;
                        break;
                    }
                }
                for (int i = defIdList.Count - 1; i > index; i--)
                {
                    RemoveItemSlot(defIdList[i].UID);
                }
            }
        }
        items.Sort((a, b) =>
        {
            int qualityCompare = b.Quality.CompareTo(a.Quality);
            if (qualityCompare != 0)
                return qualityCompare;
            string nameA = ItemDatabase.Instance.GetItemDataByDefId(a.ID).name_pinyin;
            string nameB = ItemDatabase.Instance.GetItemDataByDefId(a.ID).name_pinyin;
            int nameCompare = nameA.CompareTo(nameB);
            if (nameCompare != 0)
                return nameCompare;
            int amountA = a is IStackable stackA ? stackA.Amount : 1;
            int amountB = b is IStackable stackB ? stackB.Amount : 1;
            return amountB.CompareTo(amountA);
        });
        EventCenter.Instance.BroadEvent(E_EventType.SlotAmountChangeOrSort, items);
    }
    private void AddNewItemSlot(Item item)
    {
        items.Add(item);
        itemDicByUid.Add(item.UID, item);
        if (!itemDicByDefId.TryGetValue(item.ID, out List<Item> defIdList))
            itemDicByDefId.Add(item.ID, defIdList = new List<Item>());
        defIdList.Add(item);
        EventCenter.Instance.BroadEvent(E_EventType.SlotAmountChangeOrSort, items);
    }
    private void RemoveItemSlot(string uid)
    {
        if (itemDicByUid.TryGetValue(uid, out Item item))
        {
            items.Remove(item);
            itemDicByUid.Remove(uid);
            var defIdList = itemDicByDefId[item.ID];
            defIdList.Remove(item);
            if (defIdList.Count == 0)
                itemDicByDefId.Remove(item.ID);
            EventCenter.Instance.BroadEvent(E_EventType.SlotAmountChangeOrSort, items);
        }
    }
    public void Save()
    {
        string path = Path.Combine(Application.dataPath, "TestData", "BagData.json");
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        string json = JsonConvert.SerializeObject(this, settings);
        File.WriteAllText(path, json);
    }
    public static ItemModel Load()
    {
        string path = Path.Combine(Application.dataPath, "TestData", "BagData.json");
        ItemModel model = new ItemModel();
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            model = JsonConvert.DeserializeObject<ItemModel>(json, settings);
        }
        model.Init();
        return model;
    }
}
