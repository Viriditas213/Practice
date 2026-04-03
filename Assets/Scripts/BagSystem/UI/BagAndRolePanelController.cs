using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BagAndRolePanelController : SingleBase<BagAndRolePanelController>
{
    BagAndRolePanel view;
    ItemModel model = DataMgr.Instance.BagData;

    Transform spawnPoint;
    void InitView()
    {
        AddListener();
        view.tabGroup.OnTabSelected(view.tabGroup.tabOptions[(int)E_TabCategory.All], true);
    }

    void AddListener()
    {
        if (view != null)
        {
            view.OnClose -= HidePanel;
            view.OnClose += HidePanel;
            view.OnTidy -= TidyBag;
            view.OnTidy += TidyBag;
        }
    }

    void RemoveListener()
    {
        if (view != null)
        {
            view.OnClose -= HidePanel;
            view.OnTidy -= TidyBag;
        }
    }

    void TidyBag()
    {
        if (view != null && model != null)
        {
            model.TidyItem();
            view.RefreshBag(model.Items);
        }
    }

    public void PreLoadPanel()
    {
        if (view != null)
            return;
        UIMgr.Instance.ShowPanel<BagAndRolePanel>(E_PanelLayer.Mid, fade: false, callback: (panel) =>
        {
            view = panel;
            if (view != null)
            {
                view.GetComponent<CanvasGroup>().alpha = 0;
                Test.Instance.StartCoroutine(HidePanelNextFrame());
                
                IEnumerator HidePanelNextFrame()
                {
                    yield return null;
                    UIMgr.Instance.HidePanel<BagAndRolePanel>(null,false);
                    view.transform.localScale = Vector3.one * 0.01f;
                }
            }
        });
    }

    public void ShowPanel()
    {
        UIMgr.Instance.ShowPanel<BagAndRolePanel>(E_PanelLayer.Mid, (panel) =>
        {
            view = panel;
            if (view == null) return;
            spawnPoint = MainPanelController.Instance.RoleButtonPos();
            if (spawnPoint != null)
            {
                view.transform.DOMove(spawnPoint.position, 0.3f).From();
                view.transform.DOScale(0.01f, 0.3f).From();
            }
            InitView();
        });
    }

    public void HidePanel()
    {
        if (view == null) return;
        UIMgr.Instance.HidePanel<BagAndRolePanel>(null);
        spawnPoint = MainPanelController.Instance.RoleButtonPos();
        if (spawnPoint != null)
        {
            view.transform.DOMove(spawnPoint.position, 0.3f);
            view.transform.DOScale(0.01f, 0.3f);
        }
        RemoveListener();
    }
    public void ShowCatagory(E_TabCategory tabCategory)
    {
        if (view != null)
        {
            view.currentTab = tabCategory;
            view.RefreshBag(model.Items);
        }
    }

}
