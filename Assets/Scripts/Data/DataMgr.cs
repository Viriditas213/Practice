using Newtonsoft.Json;
using UnityEngine;

public class DataMgr : SingleBase<DataMgr>
{
    [JsonProperty]private PlayerModel playerData;
    [JsonIgnore]public PlayerModel PlayerData => playerData;
    [JsonProperty]private ItemModel bagData;
    [JsonIgnore]public ItemModel BagData => bagData;

    public DataMgr()
    {
        playerData = PlayerModel.Load();
        bagData = ItemModel.Load();
    }
}
