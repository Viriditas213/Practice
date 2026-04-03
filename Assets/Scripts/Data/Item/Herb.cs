using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Herb : MaterialItem
{
    public Herb(HerbData herb, int initalAmount = 1) : base(herb, initalAmount) { }
    [JsonConstructor] protected Herb(string uid, string id, string name, E_QualityType quality, List<ItemStatModifier> itemStatModifiers, int amount) : 
                                            base(uid, id, name, quality, itemStatModifiers, amount) { }
}
