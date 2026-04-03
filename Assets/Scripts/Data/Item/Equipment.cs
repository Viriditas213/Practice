using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public abstract class Equipment : Item
{
    public Equipment(EquipmentData equipment) : base(equipment.defId, equipment.itemName, equipment.quality, equipment.statList) { }
    [JsonConstructor] protected Equipment(string uid, string id, string name, E_QualityType quality, List<ItemStatModifier> itemStatModifiers) : 
                                            base(uid, id, name, quality, itemStatModifiers) { }
}
