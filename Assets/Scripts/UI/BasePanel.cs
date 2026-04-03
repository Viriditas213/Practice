using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BasePanel : MonoBehaviour
{
    bool isShow = false;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;
    private Dictionary<string, Dictionary<string, UIBehaviour>> controlUnits;
    protected bool isModel = true;
    public bool IsModel => isModel;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        controlUnits = new Dictionary<string, Dictionary<string, UIBehaviour>>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        FindControlUnits();
    }
    protected virtual void OnEnable()
    {

    }
    protected virtual void OnDisable()
    {

    }
    public void SetInteractable(bool interactable)
    {
        canvasGroup.interactable = interactable;
    }
    public virtual void ShowMe(bool fade,float fadeTime = 0.3f, bool canBlock = true)
    {
        if (isShow)
            return;
        transform.localScale = Vector3.one;
        if (rectTransform != null)
            rectTransform.anchoredPosition = Vector2.zero;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        isShow = true;
        if (fade)
        {
            canvasGroup.DOKill(true);
            canvasGroup.DOFade(1f, fadeTime).OnComplete(() =>
            {
                canvasGroup.interactable = canBlock;
                canvasGroup.blocksRaycasts = canBlock;
            });
        }
        else
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = canBlock;
            canvasGroup.blocksRaycasts = canBlock;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fade">true有淡出效果，false没有</param>
    /// <param name="callback">放置界面关闭后需要执行的操作</param>
    public virtual void HideMe(UnityAction callback, bool fade = true, float fadeTime = 0.3f)
    {
        if (fade)
            {
                canvasGroup.DOKill(true);
                canvasGroup.DOFade(0f, fadeTime).OnComplete(() => { callback?.Invoke(); });}
        else
            callback?.Invoke();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        isShow = false;
    }

    protected T GetControlUnit<T>(string controlName) where T : UIBehaviour
    {
        if (!controlUnits.ContainsKey(controlName) || !controlUnits[controlName].ContainsKey(typeof(T).Name))
            return null;
        return controlUnits[controlName][typeof(T).Name] as T;
    }
    private void FindControlUnits()
    {
        UIBehaviour[] units = GetComponentsInChildren<UIBehaviour>();

        foreach (UIBehaviour unit in units)
        {
            string controlName = unit.gameObject.name;
            if (!controlName.StartsWith("btn") && !controlName.StartsWith("txt") && !controlName.StartsWith("img") &&
                !controlName.StartsWith("tog") && !controlName.StartsWith("sld") && !controlName.StartsWith("scr"))
                continue;
            string typeName = unit.GetType().Name;
            if (!controlUnits.ContainsKey(controlName))
                controlUnits.Add(controlName, new Dictionary<string, UIBehaviour>());
            if (!controlUnits[controlName].ContainsKey(typeName))
                controlUnits[controlName].Add(typeName, unit);
            else
            {
                // 1. 组合好警告信息
                string warningMsg = $"在面板 {this.gameObject.name} 中发现了重复的控件命名 [{controlName}]({typeName})！\n\n这会导致 UI 绑定冲突，已自动忽略多余的节点。请立刻检查并修改您的 UI 层级命名！";

                // 2. 依然在控制台打印一份，方便查阅历史日志
                Debug.LogWarning("【UI框架拦截】" + warningMsg);

                // 3. 💡 核心魔法：预处理指令。
                // 这里的代码只有在 Unity 编辑器环境下才会被编译和执行！打包时它会被当成注释直接忽略。
#if UNITY_EDITOR
                // 呼出原生编辑器弹窗：标题，内容，确认按钮文字
                UnityEditor.EditorUtility.DisplayDialog("🚨 UI 框架严重警告", warningMsg, "我马上去改！");
#endif
            }

        }
    }

}
