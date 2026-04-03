using DG.Tweening;
using UnityEngine;

public class RolePanelController : SingleBase<RolePanelController>
{
    RolePanel view;
    PlayerModel model = DataMgr.Instance.PlayerData;

    void InitView()
    {
        AddListener();
        if (view != null)
        {
            model.OnSixDimChange -= view.UpdatePanel;
            model.OnSixDimChange += view.UpdatePanel;
            view.UpdatePanel(model.SixDim, model.Level);
        }
    }

    void AddListener()
    {
        if (view != null)
        {
            view.OnClickUpgrade -= model.Upgrade;
            view.OnClickUpgrade += model.Upgrade;
            view.OnClickClose -= HidePanel;
            view.OnClickClose += HidePanel;
        }
    }
    void RemoveListener()
    {
        if (view != null)
        {
            view.OnClickUpgrade -= model.Upgrade;
            view.OnClickClose -= HidePanel;
        }
    }

    public void ShowPanel()
    {
        UIMgr.Instance.ShowPanel<RolePanel>(E_PanelLayer.Mid, (panel) =>
        {
            view = panel;
            Transform target = MainPanelController.Instance.RoleButtonPos();
            if (target != null)
            {
                view.transform.DOMove(target.position, 0.3f).From(); ;
                view.transform.DOScale(0f, 0.3f).From();
            }
            InitView();
        });
    }
    public void HidePanel()
    {
        if (view != null)
        {
            RectTransform target = MainPanelController.Instance.RoleButtonPos();
            if (target != null)
                view.transform.DOMove(target.position, 0.3f);
            view.transform.DOScale(0f, 0.3f);
            UIMgr.Instance.HidePanel<RolePanel>(() =>
            {
                model.OnSixDimChange -= view.UpdatePanel;
                RemoveListener();
                view = null;
            });
        }
    }
}
