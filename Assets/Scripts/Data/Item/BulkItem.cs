using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public abstract class BulkItem : Item, IStackable
{
    [JsonProperty] private int _amount;
    [JsonIgnore] public int Amount => _amount;
    [JsonIgnore] public int MaxStack => 99;
    public BulkItem(BulkItemData bulkItem, int initalAmount = 1) : base(bulkItem.defId, bulkItem.itemName, bulkItem.quality, bulkItem.statList) { _amount = initalAmount; }
    [JsonConstructor] protected BulkItem(string uid, string id, string name, E_QualityType quality, List<ItemStatModifier> itemStatModifiers, int amount) : 
                                            base(uid, id, name, quality, itemStatModifiers) { _amount = amount; }
    public int AddAmount(int amount)
    {
        if (_amount + amount > MaxStack)
        {
            int overflow = _amount + amount - MaxStack;
            _amount = MaxStack;
            return overflow;
        }
        else
        {
            _amount += amount;
            return 0;
        }
    }

    public bool RemoveAmount(int amount)
    {
        if (amount > _amount || amount < 0)
            return false;
        _amount -= amount;
        return true;
    }
}
