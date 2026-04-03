using System.Collections.Generic;
using UnityEngine;

public enum E_TabCategory { All, Equipment, Supply, Material, ImportantItem }
public class TabGroup : MonoBehaviour
{
    public Sprite activeSprite;
    public Sprite normalSprite;
    public List<TabOption> tabOptions;

    private TabOption selectedOption;
    void Start()
    {
        if (tabOptions.Count > 0)
            OnTabSelected(tabOptions[0]);
    }

    public void OnTabSelected(TabOption clickedOption ,bool forceUpdate = false)
    {
        if (!forceUpdate)
        {
            if (clickedOption == selectedOption) 
                return;
        }
        selectedOption?.Deselect(normalSprite);
        selectedOption = clickedOption;
        selectedOption.Selected(activeSprite);
        BagAndRolePanelController.Instance.ShowCatagory(selectedOption.tabCategory);
    }
}
