using UnityEngine;

public class BagMgr : SingleBase<BagMgr>
{
    ItemModel model = DataMgr.Instance.BagData;

    public void GetItemFromPickUp(Item item)
    {
        model.AddItem(item);
    }

    public void GetItemFromBuy(Item item)
    {
        model.AddItem(item);
    }

    public void GetItemFromCraft(Item item)
    {
        model.AddItem(item);
    }
}
