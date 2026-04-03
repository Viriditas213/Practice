using System;
using System.Collections.Generic;
using UnityEngine;
public enum E_EventType{ KillMonster, ItemChange, DialogeNPC, EnterScene, SlotAmountChangeOrSort, SlotContentChange, SlotFocused}
public class EventCenter : SingleBase<EventCenter>
{
    private Dictionary<E_EventType, Delegate> eventDic = new Dictionary<E_EventType, Delegate>();

    public void AddEvent(E_EventType eventType, Action action)
    {
        if (!eventDic.TryGetValue(eventType, out Delegate d))
            eventDic.Add(eventType, action);
        else
            eventDic[eventType] = Delegate.Combine(d, action);
    }

    public void RemoveEvent( E_EventType eventType, Action action)
    {
        if (!eventDic.TryGetValue(eventType, out Delegate d))
            return;
        eventDic[eventType] = Delegate.Remove(d, action);
    }

    public void BroadEvent( E_EventType eventType)
    {
        if (eventDic.TryGetValue(eventType, out Delegate d))
            (d as Action)?.Invoke();
    }

    public void AddEvent<T>(E_EventType eventType, Action<T> action)
    {
        if (!eventDic.TryGetValue(eventType, out Delegate d))
            eventDic.Add(eventType, action);
        else
            eventDic[eventType] = Delegate.Combine(d, action);
    }

    public void RemoveEvent<T>( E_EventType eventType, Action<T> action)
    {
        if (!eventDic.TryGetValue(eventType, out Delegate d))
            return;
        eventDic[eventType] = Delegate.Remove(d, action);
    }

    public void BroadEvent<T>( E_EventType eventType, T info)
    {
        if (eventDic.TryGetValue(eventType, out Delegate d))
            (d as Action<T>)?.Invoke(info);
    }

    public void AddEvent<T0, T1>(E_EventType eventType, Action<T0, T1> action)
    {
        if (!eventDic.TryGetValue(eventType, out Delegate d))
            eventDic.Add(eventType, action);
        else
            eventDic[eventType] = Delegate.Combine(d, action);
    }

    public void RemoveEvent<T0, T1>( E_EventType eventType, Action<T0, T1> action)
    {
        if (!eventDic.TryGetValue(eventType, out Delegate d))
            return;
        eventDic[eventType] = Delegate.Remove(d, action);
    }

    public void BroadEvent<T0, T1>( E_EventType eventType, T0 info0, T1 info1)
    {
        if (eventDic.TryGetValue(eventType, out Delegate d))
            (d as Action<T0, T1>)?.Invoke(info0, info1);
    }
}
