using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Silk : MaterialItem
{
    public Silk(SilkData silk, int initalAmount = 1) : base(silk, initalAmount) { }
    [JsonConstructor] protected Silk(string uid, string id, string name, E_QualityType quality, List<ItemStatModifier> itemStatModifiers, int amount) : 
                                            base(uid, id, name, quality, itemStatModifiers, amount) { }
}
