using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabOption : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    public TabGroup tabGroup;
    public E_TabCategory tabCategory;
    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
    public void Selected(Sprite activeSprite)
    {
        image.sprite = activeSprite;
    }
    public void Deselect(Sprite normalSprite)
    {
        image.sprite = normalSprite;
    }
}
