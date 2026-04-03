using UnityEngine;

public class CombatEffectData : ScriptableObject
{
    public int priority = 0;
    [TextArea(3, 5)]
    public string discription;
    public virtual float ModifyMiss(float miss) => miss;
}
[CreateAssetMenu(fileName = "EnhanceMiss", menuName = "CombatEffect/EnhanceMiss")]
public class EnhanceMiss : CombatEffectData
{
    public float value;
    public override float ModifyMiss(float miss)
    {
        return miss * (1 + value);
    }
}
