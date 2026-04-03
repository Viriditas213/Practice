using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
public enum E_QualityType { Tier1 = 1, Tier2 = 2, Tier3 = 3, Tier4 = 4, Tier5 = 5 }
public abstract class Item
{
    [JsonProperty] private readonly string _uid;
    [JsonIgnore] public string UID => _uid;
    [JsonProperty] private readonly string _id;
    [JsonIgnore] public string ID => _id;
    [JsonProperty] private readonly string _name;
    [JsonIgnore] public string Name => _name;
    [JsonProperty] private readonly E_QualityType _quality;
    [JsonIgnore] public E_QualityType Quality => _quality;
    [JsonProperty]private readonly List<ItemStatModifier> _statModifiers;
    [JsonIgnore]public List<ItemStatModifier> StatModifiers => _statModifiers;
    public Item(string id, string name, E_QualityType quality, List<ItemDataStatModifier> statList)
    {
        _uid = Guid.NewGuid().ToString();
        _id = id;
        _name = name;
        _quality = quality;
        _statModifiers = new List<ItemStatModifier>();
        foreach(ItemDataStatModifier statData in statList)
        {
            ItemStatModifier statModifier = new ItemStatModifier();
            (statModifier.stat, statModifier.value, statModifier.isPercent) = statData;
            _statModifiers.Add(statModifier);
        }
    }
    [JsonConstructor]
    protected Item(string uid, string id, string name, E_QualityType quality, List<ItemStatModifier> statModifiers)
    {
        _uid = uid;
        _id = id;
        _name = name;
        _quality = quality;
        _statModifiers = statModifiers;
    }
}