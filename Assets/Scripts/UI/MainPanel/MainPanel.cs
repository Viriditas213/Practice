using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    Button roleButton, skillButton;
    Text level, money, crys, power;
    private RectTransform roleButtonPos;
    private RectTransform skillButtonPos;
    public RectTransform RoleButtonPos => roleButtonPos;
    public RectTransform SkillButtonPos => skillButtonPos;
    public event Action ClickRoleButton;
    public event Action ClickSkillButton;
    protected override void Awake()
    {
        base.Awake();
        roleButton = GetControlUnit<Button>("btnRole");
        skillButton = GetControlUnit<Button>("btnSkill");
        roleButtonPos = roleButton.GetComponent<RectTransform>();
        skillButtonPos = skillButton.GetComponent<RectTransform>();
        level = GetControlUnit<Text>("txtLev");
        money = GetControlUnit<Text>("txtMoney");
        crys = GetControlUnit<Text>("txtGem");
        power = GetControlUnit<Text>("txtPower");
        roleButton.onClick.AddListener(() =>
        {
            ClickRoleButton?.Invoke();
        });
        skillButton.onClick.AddListener(() =>
        {
            ClickSkillButton?.Invoke();
        });
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        ClickRoleButton = null;
        ClickSkillButton = null;
    }
    public void UpdatePanel(int level, int money, int crys, int power)
    {
        this.level.text = $"LV.{level}";
        this.money.text = money.ToString();
        this.crys.text = crys.ToString();
        this.power.text = power.ToString();
    }
    public void UpdateLevel(int level)
    {
        this.level.text = $"LV.{level}";
    }
    public void UpdateMoney(int money)
    {
        this.money.text = money.ToString();
    }
    public void UpdateCrys(int crys)
    {
        this.crys.text = crys.ToString();
    }
    public void UpdatePower(int power)
    {
        this.power.text = power.ToString();
    }

    void OnDestroy()
    {
        Addressables.ReleaseInstance(gameObject);
    }
}
