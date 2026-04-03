using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Image backgroundImage;
    public Image itemImage;
    public GameObject hightlightFrame;
    public TextMeshProUGUI txtNum;
    private Item currentItem;
    private static Dictionary<string, AsyncOperationHandle<Sprite>> iconPool = new Dictionary<string, AsyncOperationHandle<Sprite>>();
    public async void UpdateSlot(Item item)
    {
        currentItem = item;
        txtNum.gameObject.SetActive(false);
        if (item == null)
        {
            currentItem = null;
            itemImage.gameObject.SetActive(false);
            return;
        }
        itemImage.gameObject.SetActive(true);
        if (!iconPool.TryGetValue(item.ID, out AsyncOperationHandle<Sprite> handle))
        {
            handle = ItemDatabase.Instance.GetItemDataByDefId(item.ID).icon.LoadAssetAsync();
            iconPool.Add(item.ID, handle);
        }
        if (!handle.IsDone)
        {
            await handle.Task;
        }
        if (currentItem == item && handle.Status == AsyncOperationStatus.Succeeded)
            itemImage.sprite = handle.Result;

        if (item is IStackable stackItem)
        {
            txtNum.gameObject.SetActive(true);
            txtNum.text = stackItem.Amount.ToString();
        }
    }

    public static void ReleaseSprite(List<Item> allItems)
    {
        if (iconPool.Count == 0) return;
        HashSet<string> currentItemIDs = new HashSet<string>(allItems.Select(item => item.ID));
        foreach (string key in iconPool.Keys.ToList())
        {
            if (!currentItemIDs.Contains(key))
            {
                Addressables.Release(iconPool[key]);
                iconPool.Remove(key);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    void ShowTooltip()
    {
        if (currentItem == null) return;
        TooltipPanelController.Instance.ShowItemInfo(transform as RectTransform, currentItem);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (hightlightFrame != null)
            hightlightFrame.SetActive(true);
        EventCenter.Instance.BroadEvent<RectTransform>(E_EventType.SlotFocused, transform as RectTransform);
        ShowTooltip();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (hightlightFrame != null)
            hightlightFrame.SetActive(false);
        if (currentItem != null)
            TooltipPanelController.Instance.HidePanel();
    }

    void OnDisable()
    {
        if (hightlightFrame != null)
            hightlightFrame.SetActive(false);
        if (currentItem != null)
            TooltipPanelController.Instance.HidePanel();
    }
    void OnDestroy()
    {
        Addressables.ReleaseInstance(gameObject);
    }
}
