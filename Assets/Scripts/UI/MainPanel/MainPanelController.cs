using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum E_DataUpdate { Level, Money, Crys, Power }
public class MainPanelController : SingleBase<MainPanelController>
{
    MainPanel view;
    PlayerModel model;
    public MainPanelController()
    {
        model = DataMgr.Instance.PlayerData;
    }
    private void InitView()
    {
        if (view != null)
        {
            AddListener();
            view.UpdatePanel(model.Level, model.Money, model.Crys, model.Power);
            if (model != null)
            {
                model.OnLevelChange -= view.UpdateLevel;
                model.OnMoneyChange -= view.UpdateMoney;
                model.OnCrysChange -= view.UpdateCrys;
                model.OnPowerChange -= view.UpdatePower;
                model.OnLevelChange += view.UpdateLevel;
                model.OnMoneyChange += view.UpdateMoney;
                model.OnCrysChange += view.UpdateCrys;
                model.OnPowerChange += view.UpdatePower;
            }
        }
    }
    private void AddListener()
    {
        if (view != null)
        {
            view.ClickRoleButton -= OpenRolePanel;
            view.ClickSkillButton -= OpenSkillPanel;
            view.ClickRoleButton += OpenRolePanel;
            view.ClickSkillButton += OpenSkillPanel;
        }
    }
    public void RemoveListener()
    {
        if (view != null)
        {
            view.ClickRoleButton -= OpenRolePanel;
            view.ClickSkillButton -= OpenSkillPanel;
        }
    }
    public void PreLoadPanel()
    {
        if (view != null) return;
        UIMgr.Instance.ShowPanel<MainPanel>(E_PanelLayer.Bot, fade: false, callback: (panel) =>
        {
            view = panel;
            view.GetComponent<CanvasGroup>().alpha = 0;
            Test.Instance.StartCoroutine(HidePanelNextFrame());

            IEnumerator HidePanelNextFrame()
            {
                yield return null;
                UIMgr.Instance.HidePanel<MainPanel>(null, false);
            }
        });
    }
    public void ShowPanel()
    {
        UIMgr.Instance.ShowPanel<MainPanel>(E_PanelLayer.Bot, (panel) =>
        {
            view = panel;
            InitView();
        });
    }

    public void HidePanel()
    {
        RolePanel panel = UIMgr.Instance.GetPanel<RolePanel>();
        if (panel != null)
            return;
        UIMgr.Instance.HidePanel<MainPanel>(() =>
        {
            if (model != null)
            {
                model.OnLevelChange -= view.UpdateLevel;
                model.OnMoneyChange -= view.UpdateMoney;
                model.OnCrysChange -= view.UpdateCrys;
                model.OnPowerChange -= view.UpdatePower;
            }
            RemoveListener();
            view = null;
        });
    }

    public void ModelChenge(E_DataUpdate type, int value)
    {
        switch (type)
        {
            case E_DataUpdate.Level:
                model.ChangeLevel(value);
                break;
            case E_DataUpdate.Money:
                model.ChangeMoney(value);
                break;
            case E_DataUpdate.Crys:
                model.ChangeCrys(value);
                break;
            case E_DataUpdate.Power:
                model.ChangePower(value);
                break;
        }
    }

    public RectTransform RoleButtonPos()
    {
        if (view == null)
            return null;
        return view.RoleButtonPos;
    }
    public RectTransform SkillButtonPos()
    {
        if (view == null)
            return null;
        return view.SkillButtonPos;
    }


    private void OpenRolePanel()
    {
        BagAndRolePanelController.Instance.ShowPanel();
    }

    private void OpenSkillPanel()
    {

    }

}
