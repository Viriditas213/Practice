using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : SingleToMono<Test>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _ = UIMgr.Instance;
        ItemDatabase.Instance.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[Key.B].wasPressedThisFrame)
        {
            MainPanelController.Instance.ShowPanel();
        }
        if (Keyboard.current[Key.N].wasPressedThisFrame)
        {
            MainPanelController.Instance.HidePanel();
        }
        if (Keyboard.current[Key.J].wasPressedThisFrame)
        {
            Weapon weapon = new Weapon(ItemDatabase.Instance.GetItemDataByDefId("1") as WeaponData);
            BagMgr.Instance.GetItemFromPickUp(weapon);
        }
        if (Keyboard.current[Key.K].wasPressedThisFrame)
        {
            Pill pill = new Pill(ItemDatabase.Instance.GetItemDataByDefId("2") as PillData);
            BagMgr.Instance.GetItemFromPickUp(pill);
        }
    }
}
