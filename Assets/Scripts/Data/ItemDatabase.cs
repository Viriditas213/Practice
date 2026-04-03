using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObject/ItemDatabase", order = 1)]
public class ItemDatabase : SingleToScriptable<ItemDatabase>
{
    public List<ItemData> itemList = new List<ItemData>();
    private Dictionary<string, ItemData> itemDic;
    public void Init()
    {
        itemDic = new Dictionary<string, ItemData>();
        foreach (ItemData itemData in itemList)
        {
            if (!itemDic.TryAdd(itemData.defId, itemData))
            {
                string warningMsg = $"当前物品中存在重复id[{itemData.defId}]!({itemData.itemName})和({itemDic[itemData.defId].itemName}), 请检查";
                Debug.LogWarning("[物品信息拦截]"+warningMsg);

#if UNITY_EDITOR
                UnityEditor.EditorUtility.DisplayDialog("物品数据验证警告！！！",warningMsg,"马上纠正");
#endif
                return;
            }
        }
    }
    public ItemData GetItemDataByDefId(string defId)
    {
        if (itemDic.TryGetValue(defId, out ItemData itemData))
        {
            return itemData;
        }
        return null;
    }
}
