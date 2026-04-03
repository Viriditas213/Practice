using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Ores : MaterialItem
{
    public Ores(OresData ores, int initalAmount = 1) : base(ores, initalAmount) { }
    [JsonConstructor] protected Ores(string uid, string id, string name, E_QualityType quality, List<ItemStatModifier> itemStatModifiers, int amount) : 
                                            base(uid, id, name, quality, itemStatModifiers, amount) { }
}
