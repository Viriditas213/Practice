using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
public enum E_PanelLayer { Bot, Mid, Top, System }

public class UIMgr : SingleBase<UIMgr>
{
    Transform canvas, bot, mid, top, system;
    Dictionary<string, BasePanel> existPanel;
    List<BasePanel> panelStack;
    Dictionary<string, BasePanel> panelPool;
    Dictionary<string, List<Action<BasePanel>>> waitRoom;
    public UIMgr()
    {
        Addressables.InstantiateAsync("Canvas").Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                canvas = handle.Result.transform;
                bot = canvas.Find("Bot");
                mid = canvas.Find("Mid");
                top = canvas.Find("Top");
                system = canvas.Find("System");
                existPanel = new Dictionary<string, BasePanel>();
                waitRoom = new Dictionary<string, List<Action<BasePanel>>>();
                panelPool = new Dictionary<string, BasePanel>();
                panelStack = new List<BasePanel>();
                GameObject.DontDestroyOnLoad(canvas.gameObject);
                MainPanelController.Instance.PreLoadPanel();
                BagAndRolePanelController.Instance.PreLoadPanel();
                TooltipPanelController.Instance.PreLoadPanel();
            }
        };
    }

    public void ShowPanel<T>(E_PanelLayer layer, Action<T> callback, bool fade = true) where T : BasePanel
    {
        if (canvas == null)
        {
            Debug.LogWarning("画布尚未初始化完成，请稍后再试");
            return;
        }
        string panelName = typeof(T).Name;
        if (existPanel.ContainsKey(panelName))
        {
            callback?.Invoke(existPanel[panelName] as T);
            return;
        }
        Transform parent;
        switch (layer)
        {
            case E_PanelLayer.Mid:
                parent = mid;
                break;
            case E_PanelLayer.Top:
                parent = top;
                break;
            case E_PanelLayer.System:
                parent = system;
                break;
            default:
                parent = bot;
                break;
        }
        T panel = null;
        if (panelPool.TryGetValue(panelName, out BasePanel panelInPool))
        {
            panel = panelInPool as T;
            panel.transform.SetParent(parent, false);
            panelPool.Remove(panelName);
            if (panel.IsModel)
            {
                foreach (BasePanel bp in existPanel.Values)
                    bp.SetInteractable(false);
                panelStack.Add(panel);
            }
            existPanel.Add(panelName, panel);
            panel.ShowMe(fade);
            callback?.Invoke(panel);
            return;
        }
        if (waitRoom.TryGetValue(panelName, out List<Action<BasePanel>> waitList))
        {
            if (callback != null)
                waitList.Add((panel) => callback?.Invoke(panel as T));
            return;
        }
        waitRoom.Add(panelName, new List<Action<BasePanel>>() { (panel) => callback?.Invoke(panel as T) });

        DoLoadPanelAsync<T>(panelName, parent, fade);
    }
    void DoLoadPanelAsync<T>(string panelName, Transform parent, bool fade) where T : BasePanel
    {
        Addressables.InstantiateAsync(panelName, parent, false).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject panelObj = handle.Result;
                panelObj.name = panelName;
                T panel = panelObj.GetComponent<T>();
                panel.ShowMe(fade);
                foreach (var cb in waitRoom[panelName])
                {
                    cb?.Invoke(panel);
                }
                waitRoom.Remove(panelName);
                if (panel.IsModel)
                {
                    foreach (BasePanel bp in existPanel.Values)
                        bp.SetInteractable(false);
                    panelStack.Add(panel);
                }
                existPanel.Add(panelName, panel);
            }
            else
                Debug.LogError($"找不到名为{panelName}的可寻址文件");
        };
    }

    public void HidePanel<T>(UnityAction callback, bool fade = true) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (existPanel.TryGetValue(panelName, out BasePanel panel))
        {
            panelPool.Add(panelName, panel);
            existPanel.Remove(panelName);
            panelStack.Remove(panel);
            if (panelStack.Count > 0)
                panelStack[panelStack.Count - 1].SetInteractable(true);
            else
                EventSystem.current.SetSelectedGameObject(null);
            panel.HideMe(() =>
            {
                callback?.Invoke();
            }, fade);
            return;
        }
    }

    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (existPanel.ContainsKey(panelName))
            return existPanel[panelName] as T;
        return null;
    }
}
