using UnityEngine;

public enum E_StatType { HP, MP, Attack, Defense, DodgeRate, CritRate, HitRate, CritResist, FootWork, MaxHP, MaxMP}
[System.Serializable]
public struct ItemStatModifier
{
    public E_StatType stat;
    public int value;
    public bool isPercent;
    public void Deconstruct(out E_StatType statType, out int value, out bool isPercent) => (statType, value, isPercent) = (stat, this.value, this.isPercent);
}
[System.Serializable]
public struct ItemDataStatModifier
{
    public E_StatType stat;
    public int maxValue;
    public int minValue;
    public bool isPercent;

    public void Deconstruct(out E_StatType statType, out int value, out bool isPercent) 
        => (statType, value, isPercent) = (stat, Random.Range(minValue, maxValue), this.isPercent);
}
