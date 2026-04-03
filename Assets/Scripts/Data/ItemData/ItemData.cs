using System.Collections.Generic;
using ToolGood.Words;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ItemData : ScriptableObject
{
    public string defId;
    public string itemName;
    public string name_pinyin;
    public E_QualityType quality;
    public AssetReferenceSprite icon;
    public List<ItemDataStatModifier> statList;
    [TextArea(3,5)]
    public string discription;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!string.IsNullOrEmpty(itemName))
            name_pinyin = WordsHelper.GetPinyin(itemName);
    }
#endif
}







