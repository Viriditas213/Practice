using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Food : SupplyItem
{
    public Food(FoodData food, int initalAmount = 1) : base(food, initalAmount) { }
    [JsonConstructor] protected Food(string uid, string id, string name, E_QualityType quality, List<ItemStatModifier> itemStatModifiers, int amount) : 
                                            base(uid, id, name, quality, itemStatModifiers, amount) { }
}
