using UnityEngine;

public abstract class QuestObjective : ScriptableObject
{
    public bool isComplete { get; protected set; }
    public abstract void Init();
    public abstract void CheckProgress();
    public abstract void CleanUp();
}
[CreateAssetMenu(fileName = "KillMonster", menuName = "Quest/QuestObjective/KillMonster", order = 1)]
public class KillMonster : QuestObjective
{
    public string targetId;
    public int targetCount;
    public int nowCount;
    public override void Init()
    {
        nowCount = 0;
        isComplete = false;
        EventCenter.Instance.AddEvent<string>(E_EventType.KillMonster, Kill);
    }
    private void Kill(string monsterId)
    {
        if (monsterId == targetId)
        {
            nowCount++;
            CheckProgress();
        }
    }

    public override void CheckProgress()
    {
        if (nowCount >= targetCount)
        {
            isComplete = true;
            CleanUp();
        }
    }

    public override void CleanUp()
    {
        EventCenter.Instance.RemoveEvent<string>(E_EventType.KillMonster, Kill);
    }
}

[CreateAssetMenu(fileName = "CollectItem", menuName = "Quest/QuestObjective/CollectItem", order = 2)]
public class CollectItem : QuestObjective
{
    public string targetId;
    public int targetCount;
    public int nowCount;

    public override void Init()
    {
        nowCount = 0;
        EventCenter.Instance.AddEvent<string, int>(E_EventType.ItemChange, ItemChange);
        //检查背包，是否有这玩意，有的话更新数量，然后检查是否完成
        nowCount = DataMgr.Instance.BagData.QueryItemAmount(targetId);
        CheckProgress();
    }

    private void ItemChange(string itemId, int num)
    {
        if (itemId == targetId)
        {
            nowCount = num;
            CheckProgress();
        }
    }
    public override void CheckProgress()
    {
        isComplete = nowCount >= targetCount;
    }

    public override void CleanUp()
    {
        EventCenter.Instance.RemoveEvent<string, int>(E_EventType.ItemChange, ItemChange);
        if (isComplete)
            DataMgr.Instance.BagData.RemoveItemByDefId(targetId, targetCount);
    }
}


public class DialogeNPC : QuestObjective
{
    public string targetId;

    public override void Init()
    {
        throw new System.NotImplementedException();
    }
    public override void CheckProgress()
    {
        throw new System.NotImplementedException();
    }

    public override void CleanUp()
    {
        throw new System.NotImplementedException();
    }
}