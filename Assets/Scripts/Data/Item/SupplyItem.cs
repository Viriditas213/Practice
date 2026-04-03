using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public abstract class SupplyItem : BulkItem
{
    protected SupplyItem(SupplyData supply, int initalAmount = 1) : base(supply, initalAmount) { }
    [JsonConstructor] protected SupplyItem(string uid, string id, string name, E_QualityType quality, List<ItemStatModifier> itemStatModifiers, int amount) : 
                                            base(uid, id, name, quality, itemStatModifiers, amount) { }
}
