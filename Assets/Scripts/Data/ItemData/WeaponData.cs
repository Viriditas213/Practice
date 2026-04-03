using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Equipment/Weaopn", order = 1)]
public class WeaponData : EquipmentData
{
    public List<CombatEffectData> combatEffectDatas;
}
