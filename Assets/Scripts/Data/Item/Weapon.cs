using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Weapon : Equipment
{
    public List<ItemStatModifier> statList;
    [JsonConstructor] protected Weapon(string uid, string id, string name, E_QualityType quality, List<ItemStatModifier> itemStatModifiers) : 
                                        base(uid, id, name, quality, itemStatModifiers) { }
    public Weapon(WeaponData weapon) : base(weapon)
    {
        
    }
    public List<CombatEffectData> combatEffects = new List<CombatEffectData>();
}
