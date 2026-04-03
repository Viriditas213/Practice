using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public abstract class MaterialItem : BulkItem
{
    protected MaterialItem(MaterialData material, int initalAmount = 1) : base(material, initalAmount) { }
    [JsonConstructor] protected MaterialItem(string uid, string id, string name, E_QualityType quality, List<ItemStatModifier> itemStatModifiers, int amount) : 
                                            base(uid, id, name, quality, itemStatModifiers, amount) { }
}
