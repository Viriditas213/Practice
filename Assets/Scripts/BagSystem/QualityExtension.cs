using System.Text;
using UnityEngine;

public static class ItemInfoViewer
{
    public static string GetQualityName(this Item item) => item switch
    {
        Equipment => item.Quality switch
        {
            E_QualityType.Tier1 => "粗制",
            E_QualityType.Tier2 => "良品",
            E_QualityType.Tier3 => "百炼",
            E_QualityType.Tier4 => "名匠",
            E_QualityType.Tier5 => "鬼斧",
            _ => "未知"
        },
        Pill => item.Quality switch
        {
            E_QualityType.Tier1 => "下品",
            E_QualityType.Tier2 => "中品",
            E_QualityType.Tier3 => "上品",
            E_QualityType.Tier4 => "极品",
            E_QualityType.Tier5 => "绝品",
            _ => "未知"
        },
        Food => item.Quality switch
        {
            E_QualityType.Tier1 => "难咽",
            E_QualityType.Tier2 => "果腹",
            E_QualityType.Tier3 => "鲜美",
            E_QualityType.Tier4 => "珍馐",
            E_QualityType.Tier5 => "绝味",
            _ => "未知"
        },
        Ores => item.Quality switch
        {
            E_QualityType.Tier1 => "粗矿",
            E_QualityType.Tier2 => "寻常",
            E_QualityType.Tier3 => "致密",
            E_QualityType.Tier4 => "无暇",
            E_QualityType.Tier5 => "浑然",
            _ => "未知"
        },
        Herb => item.Quality switch
        {
            E_QualityType.Tier1 => "残败",
            E_QualityType.Tier2 => "寻常",
            E_QualityType.Tier3 => "饱满",
            E_QualityType.Tier4 => "名贵",
            E_QualityType.Tier5 => "稀世",
            _ => "未知"
        },
        Leather => item.Quality switch
        {
            E_QualityType.Tier1 => "破损",
            E_QualityType.Tier2 => "完整",
            E_QualityType.Tier3 => "光泽",
            E_QualityType.Tier4 => "无痕",
            E_QualityType.Tier5 => "完美",
            _ => "未知"
        },
        Silk => item.Quality switch
        {
            E_QualityType.Tier1 => "粗糙",
            E_QualityType.Tier2 => "平整",
            E_QualityType.Tier3 => "细密",
            E_QualityType.Tier4 => "流霞",
            E_QualityType.Tier5 => "绝尘",
            _ => "未知"
        },
        _ => item.Quality switch
        {
            E_QualityType.Tier1 => "多得像路边的野狗",
            E_QualityType.Tier2 => "常见",
            E_QualityType.Tier3 => "少见",
            E_QualityType.Tier4 => "珍稀",
            E_QualityType.Tier5 => "孤品",
            _ => "未知"
        },
    };

    public static string GetStatInfo(this Item item)
    {
        StringBuilder sb = new StringBuilder();
        foreach (ItemStatModifier stat in item.StatModifiers)
        {
            string statName = GetStatName(stat);
            string statValue = stat.value.ToString() + (stat.isPercent ? "%" : "");
            sb.AppendLine($"{statName}    {statValue}");
        }
        return sb.ToString();


        string GetStatName(ItemStatModifier statModifier) => statModifier.stat switch
        {
            E_StatType.HP => "恢复生命",
            E_StatType.MP => "恢复内力",
            E_StatType.Attack => "攻击",
            E_StatType.Defense => "防御",
            E_StatType.DodgeRate => "闪避",
            E_StatType.CritRate => "暴击",
            E_StatType.HitRate => "命中",
            E_StatType.CritResist => "防暴",
            E_StatType.FootWork => "身法",
            E_StatType.MaxHP => "生命上限",
            E_StatType.MaxMP => "内力上限",
            _ => ""
        };
    }
}
