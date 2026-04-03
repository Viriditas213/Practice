using System;
using UnityEngine;
using UnityEngine.UI;

public class RolePanel : BasePanel
{
    Text hp, atk, def, crit, miss, luck, level;
    Button upgradeButton, closeButton;
    public event Action OnClickUpgrade;
    public event Action OnClickClose;
    protected override void Awake()
    {
        base.Awake();
        hp = GetControlUnit<Text>("txtHp");
        atk = GetControlUnit<Text>("txtAtk");
        def = GetControlUnit<Text>("txtDef");
        crit = GetControlUnit<Text>("txtCrit");
        miss = GetControlUnit<Text>("txtMiss");
        luck = GetControlUnit<Text>("txtLuck");
        level = GetControlUnit<Text>("txtLev");
        upgradeButton = GetControlUnit<Button>("btnLevUp");
        closeButton = GetControlUnit<Button>("btnClose");
        upgradeButton.onClick.AddListener(() =>
        {
            OnClickUpgrade?.Invoke();
        });
        closeButton.onClick.AddListener(() =>
        {
            OnClickClose?.Invoke();
        });
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        OnClickUpgrade = null;
        OnClickClose = null;
    }

    public void UpdatePanel(SixDimensions sixDim, int level)
    {
        this.hp.text = sixDim.hp.ToString();
        this.atk.text = sixDim.atk.ToString();
        this.def.text = sixDim.def.ToString();
        this.crit.text = sixDim.crit.ToString();
        this.miss.text = sixDim.miss.ToString();
        this.luck.text = sixDim.luck.ToString();
        this.level.text = $"LV.{level}";
    }
}
