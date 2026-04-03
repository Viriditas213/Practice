using UnityEngine;

public interface IStackable
{
    public int MaxStack { get; }
    public int Amount { get; }
    public int AddAmount(int amount);
    public bool RemoveAmount(int amount);
}
