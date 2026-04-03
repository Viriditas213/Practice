using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

public class TooltipPanel : BasePanel
{
    RectTransform mainTip;
    CanvasGroup mainCanvasGroup;
    TextMeshProUGUI mainItemName;
    TextMeshProUGUI mainItemInfo;
    TextMeshProUGUI mainItemDiscription;
    RectTransform subTip;
    CanvasGroup subCanvasGroup;
    TextMeshProUGUI subItemName;
    TextMeshProUGUI subItemInfo;
    TextMeshProUGUI subItemDiscription;
    protected override void Awake()
    {
        base.Awake();
        isModel = false;
        mainItemName = GetControlUnit<TextMeshProUGUI>("txtMainItemName");
        mainItemInfo = GetControlUnit<TextMeshProUGUI>("txtMainItemInfo");
        mainItemDiscription = GetControlUnit<TextMeshProUGUI>("txtMainItemDiscription");
        mainTip = mainItemName.transform.parent as RectTransform;
        subItemName = GetControlUnit<TextMeshProUGUI>("txtSubItemName");
        subItemInfo = GetControlUnit<TextMeshProUGUI>("txtSubItemInfo");
        subItemDiscription = GetControlUnit<TextMeshProUGUI>("txtSubItemDiscription");
        subTip = subItemName.transform.parent as RectTransform;
        mainCanvasGroup = mainTip.GetComponent<CanvasGroup>();
        subCanvasGroup = subTip.GetComponent<CanvasGroup>();
        mainCanvasGroup.blocksRaycasts = false;
        subCanvasGroup.blocksRaycasts = false;
    }

    public void UpdatePanel(RectTransform target,
                            string mainName, string mainInfo, string mainDiscription,
                            string subName = null, string subInfo = null, string subDiscription = null)
    {
        bool subShow = !string.IsNullOrEmpty(subName);
        mainCanvasGroup.alpha = 1;
        subCanvasGroup.alpha = subShow ? 1 : 0;
        mainItemName.text = mainName;
        mainItemInfo.text = mainInfo;
        mainItemDiscription.text = mainDiscription;
        LayoutRebuilder.ForceRebuildLayoutImmediate(mainTip);
        if (subShow)
        {
            subItemName.text = subName;
            subItemInfo.text = subInfo;
            subItemDiscription.text = subDiscription;
            LayoutRebuilder.ForceRebuildLayoutImmediate(subTip);
        }
        BoundryDetection(target, subShow);
    }

    void BoundryDetection(RectTransform target, bool subShow)
    {
        RectTransform panelRect = this.transform as RectTransform;
        Vector3 localTarget = panelRect.InverseTransformPoint(target.position);

        float calcWidth = mainTip.rect.width + (subShow ? subTip.rect.width : 0);
        float calcHeight = Mathf.Max(mainTip.rect.height, subShow ? subTip.rect.height : 0);
        bool overflowLeft = localTarget.x + target.rect.xMax + calcWidth > panelRect.rect.xMax;
        bool overflowBottom = localTarget.y + target.rect.yMin - calcHeight < panelRect.rect.yMin;
        mainTip.anchoredPosition = new Vector2(
            localTarget.x + (overflowLeft ? target.rect.xMin - mainTip.rect.width : target.rect.xMax),
            overflowBottom ? panelRect.rect.yMin + calcHeight : localTarget.y + target.rect.yMin);
        if (subShow)
            subTip.anchoredPosition = new Vector2(
                mainTip.anchoredPosition.x + (overflowLeft ? -subTip.rect.width : mainTip.rect.width),
                mainTip.anchoredPosition.y);

    }
    public override void ShowMe(bool fade, float fadeTime = 0.1f, bool canBlock = false)
    {
        base.ShowMe(fade, 0.1f, false);
    }

    public override void HideMe(UnityAction callback, bool fade = true, float fadeTime = 0.1f)
    {
        base.HideMe(callback, fade, 0.1f);
    }
    void OnDestroy()
    {
        Addressables.Release(this.gameObject);
    }
}
