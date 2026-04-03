using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Leather : MaterialItem
{
    public Leather(LeatherData leather, int initalAmount = 1) : base(leather, initalAmount) { }
    [JsonConstructor] protected Leather(string uid, string id, string name, E_QualityType quality, List<ItemStatModifier> itemStatModifiers, int amount) : 
                                            base(uid, id, name, quality, itemStatModifiers, amount) { }
}
