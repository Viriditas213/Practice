using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Pill : SupplyItem
{
    public Pill(PillData pill, int initalAmount = 1) : base(pill, initalAmount) { }
    [JsonConstructor] protected Pill(string uid, string id, string name, E_QualityType quality, List<ItemStatModifier> itemStatModifiers, int amount) : 
                                            base(uid, id, name, quality, itemStatModifiers, amount) { }
}
