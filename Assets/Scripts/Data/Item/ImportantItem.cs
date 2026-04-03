using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ImportantItem : Item
{
    protected ImportantItem(ImportantItemData importantItem) : base(importantItem.defId, importantItem.itemName, importantItem.quality, importantItem.statList) { }
    [JsonConstructor] protected ImportantItem(string uid, string id, string name, E_QualityType quality, List<ItemStatModifier> itemStatModifiers) : 
                                        base(uid, id, name, quality, itemStatModifiers) { }
}
