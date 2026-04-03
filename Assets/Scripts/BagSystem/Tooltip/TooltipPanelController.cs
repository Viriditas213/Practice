using System.Collections;
using UnityEngine;

public class TooltipPanelController : SingleBase<TooltipPanelController>
{
    TooltipPanel view;

    public void PreLoadPanel()
    {
        UIMgr.Instance.ShowPanel<TooltipPanel>(E_PanelLayer.Top, fade: false, callback: (panel) =>
        {
            view = panel;
            if (view != null)
            {
                view.GetComponent<CanvasGroup>().alpha = 0;
                Test.Instance.StartCoroutine(HidePanelNextFrame());
            }

            IEnumerator HidePanelNextFrame()
            {
                yield return null;
                UIMgr.Instance.HidePanel<TooltipPanel>(null, false);
            }
        });
    }
    public void ShowItemInfo(RectTransform target, Item item1, Item item2 = null)
    {
        ShowPanel();
        if (view == null) return;
        string itemName1 = $"【{item1.GetQualityName()}】{item1.Name}";
        string itemInfo1 = item1.GetStatInfo();
        string itemDiscription1 = ItemDatabase.Instance.GetItemDataByDefId(item1.ID).discription;
        string itemName2 = null, itemInfo2 = null, itemDiscription2 = null;
        if (item2 != null)
        {
            itemName2 = $"【{item2.GetQualityName()}】{item2.Name}";
            itemInfo2 = item2.GetStatInfo();
            itemDiscription2 = ItemDatabase.Instance.GetItemDataByDefId(item2.ID).discription;
        }
        view.UpdatePanel(target, itemName1, itemInfo1, itemDiscription1, itemName2, itemInfo2, itemDiscription2);
    }

    public void ShowPanel()
    {
        UIMgr.Instance.ShowPanel<TooltipPanel>(E_PanelLayer.Top, (panel) =>
        {
            view = panel;
        });
    }

    public void HidePanel()
    {
        UIMgr.Instance.HidePanel<TooltipPanel>(() =>
        {

        });
    }
}
