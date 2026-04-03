using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Quest/QuestData")]
public class QuestData : ScriptableObject
{
    public string questId;
    public string questName;
    [TextArea(3,6)]
    public string questDescription;
    public List<QuestObjective> objectives;
}
