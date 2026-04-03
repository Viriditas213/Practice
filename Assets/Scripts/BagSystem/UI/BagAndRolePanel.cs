using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BagAndRolePanel : BasePanel
{
    Button closeButton, tidyButton;
    TextMeshProUGUI moneyText;
    ScrollRect scrollRect;
    List<Slot> slotPool = new List<Slot>();
    Dictionary<string, Slot> slotDic = new Dictionary<string, Slot>();
    bool isCreat = false;
    public TabGroup tabGroup;
    public InputActionReference navigateAction;
    public E_TabCategory currentTab = E_TabCategory.All;
    public event Action OnClose, OnTidy;
    protected override void Awake()
    {
        base.Awake();
        closeButton = GetControlUnit<Button>("btnClose");
        tidyButton = GetControlUnit<Button>("btnTidy");
        moneyText = GetControlUnit<TextMeshProUGUI>("txtMoney");
        scrollRect = GetControlUnit<ScrollRect>("scrScrollRect");
        tabGroup = GetComponentInChildren<TabGroup>();
        slotPool = scrollRect.content.GetComponentsInChildren<Slot>().ToList();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (navigateAction != null)
        {
            navigateAction.action.Enable();
            navigateAction.action.performed += OnNavigateTriggered;
        }
    }
    void Start()
    {
        EventCenter.Instance.AddEvent<List<Item>>(E_EventType.SlotAmountChangeOrSort, CheckAndExpandSlot);
        EventCenter.Instance.AddEvent<List<Item>>(E_EventType.SlotAmountChangeOrSort, Slot.ReleaseSprite);
        closeButton.onClick.AddListener(() =>
        {
            OnClose?.Invoke();
        });
        tidyButton.onClick.AddListener(() =>
        {
            OnTidy?.Invoke();
        });
    }

    public void RefreshSlot(Item item)
    {
        if (slotDic.TryGetValue(item.UID, out Slot slot))
            slot.UpdateSlot(item);
    }

    public async void RefreshBag(List<Item> allItems)
    {
        slotDic.Clear();
        List<Item> items;
        switch (currentTab)
        {
            case E_TabCategory.Equipment:
                items = allItems.Where(item => item is Equipment).ToList();
                break;
            case E_TabCategory.Supply:
                items = allItems.Where(item => item is SupplyItem).ToList();
                break;
            case E_TabCategory.Material:
                items = allItems.Where(item => item is MaterialItem).ToList();
                break;
            case E_TabCategory.ImportantItem:
                items = allItems.Where(item => item is ImportantItem).ToList();
                break;
            default:
                items = allItems;
                break;
        }
        int displayNum = Mathf.Max(items.Count, 50);
        if (slotPool.Count < displayNum)
        {
            int needNum = displayNum - slotPool.Count;
            for (int i = 0; i < needNum; i++)
            {
                GameObject slotObj = await Addressables.InstantiateAsync("Slot", scrollRect.content, false).Task;
                slotPool.Add(slotObj.GetComponent<Slot>());

                await Task.Yield();
            }
        }
        for (int i = 0; i < displayNum; i++)
        {
            slotPool[i].gameObject.SetActive(true);
            if (i < items.Count)
            {
                slotPool[i].UpdateSlot(items[i]);
                slotDic[items[i].UID] = slotPool[i];
            }
            else
                slotPool[i].UpdateSlot(null);

        }
        for (int i = displayNum; i < slotPool.Count; i++)
        {
            slotPool[i].gameObject.SetActive(false);
        }
        scrollRect.verticalNormalizedPosition = 1f;
    }

    public async void CheckAndExpandSlot(List<Item> items)
    {
        int count = Mathf.Max(items.Count + 10, 50);
        if (count < slotPool.Count) return;
        int requestNum = count - slotPool.Count;
        if (!isCreat)
        {
            isCreat = true;
            for (int i = 0; i < requestNum; i++)
            {
                GameObject slotObj = await Addressables.InstantiateAsync("Slot", scrollRect.content, false).Task;
                slotPool.Add(slotObj.GetComponent<Slot>());
                slotObj.SetActive(false);
                await Task.Yield();
            }
            isCreat = false;
        }

    }

    private void OnSlotFocused(RectTransform target)
    {
        if (scrollRect.content.rect.height <= scrollRect.viewport.rect.height) return;
        float contentHeight = scrollRect.content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;
        float deltaHeight = contentHeight - viewportHeight;
        float viewTop = -scrollRect.content.anchoredPosition.y;
        float viewBotton = viewTop - viewportHeight;
        float targetTop = scrollRect.content.InverseTransformPoint(target.position).y + target.rect.yMax + 20;
        float targetButton = scrollRect.content.InverseTransformPoint(target.position).y + target.rect.yMin - 20;
        if (targetTop > viewTop)
            scrollRect.verticalNormalizedPosition = 1 + targetTop / deltaHeight;
        if (targetButton < viewBotton)
            scrollRect.verticalNormalizedPosition = (contentHeight + targetButton) / deltaHeight;
    }
    private void OnNavigateTriggered(InputAction.CallbackContext context)
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            StartCoroutine(ActiveNavigateEndOfThisFrame());
        }

        IEnumerator ActiveNavigateEndOfThisFrame()
        {
            yield return new WaitForEndOfFrame();
            if (slotPool.Count > 0)
            {
                EventSystem.current.SetSelectedGameObject(slotPool[0].gameObject);
            }
        }
    }

    public override void ShowMe(bool fade, float fadeTime = 0.3f, bool canBlock = true)
    {
        base.ShowMe(fade);
        EventSystem.current.SetSelectedGameObject(null);
        if (scrollRect != null && scrollRect.verticalScrollbar != null)
            scrollRect.verticalNormalizedPosition = 1;
        EventCenter.Instance.RemoveEvent<Item>(E_EventType.SlotContentChange, RefreshSlot);
        EventCenter.Instance.AddEvent<Item>(E_EventType.SlotContentChange, RefreshSlot);
        EventCenter.Instance.RemoveEvent<List<Item>>(E_EventType.SlotAmountChangeOrSort, RefreshBag);
        EventCenter.Instance.AddEvent<List<Item>>(E_EventType.SlotAmountChangeOrSort, RefreshBag);
        EventCenter.Instance.RemoveEvent<RectTransform>(E_EventType.SlotFocused, OnSlotFocused);
        EventCenter.Instance.AddEvent<RectTransform>(E_EventType.SlotFocused, OnSlotFocused);
    }
    public override void HideMe(UnityAction callback, bool fade = true, float fadeTime = 0.3f)
    {
        base.HideMe(callback, fade);
        EventCenter.Instance.RemoveEvent<Item>(E_EventType.SlotContentChange, RefreshSlot);
        EventCenter.Instance.RemoveEvent<List<Item>>(E_EventType.SlotAmountChangeOrSort, RefreshBag);
        EventCenter.Instance.RemoveEvent<RectTransform>(E_EventType.SlotFocused, OnSlotFocused);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        navigateAction.action.performed -= OnNavigateTriggered;
        navigateAction.action.Disable();
    }
    void OnDestroy()
    {
        Addressables.ReleaseInstance(gameObject);
        EventCenter.Instance.RemoveEvent<List<Item>>(E_EventType.SlotAmountChangeOrSort, CheckAndExpandSlot);
        EventCenter.Instance.RemoveEvent<List<Item>>(E_EventType.SlotAmountChangeOrSort, Slot.ReleaseSprite);
    }
}
