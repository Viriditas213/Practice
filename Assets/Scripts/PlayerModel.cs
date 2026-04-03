using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
public class PlayerModel : SingleBase<PlayerModel>
{
    private const string savePath = "D:\\R\\Documents\\Unity\\Practice\\Assets\\TestData\\MainData.json";
    public event Action<int> OnLevelChange;
    public event Action<int> OnMoneyChange;
    public event Action<int> OnCrysChange;
    public event Action<int> OnPowerChange;
    public event Action<SixDimensions, int> OnSixDimChange;
    [JsonProperty]private int level = 1;
    [JsonIgnore]public int Level => level;
    [JsonProperty]private int money = 10;
    [JsonIgnore]public int Money => money;
    [JsonProperty]private int crys = 10;
    [JsonIgnore]public int Crys => crys;
    [JsonProperty]private int power = 10;
    [JsonIgnore]public int Power => power;
    [JsonProperty]private SixDimensions sixDim = new SixDimensions();
    [JsonIgnore]public SixDimensions SixDim => sixDim;
    
    public void ChangeLevel(int value)
    {
        level = value;
        OnLevelChange?.Invoke(value);
    }

    public void ChangeMoney(int value)
    {
        money = value;
        OnMoneyChange?.Invoke(value);
    }

    public void ChangeCrys(int value)
    {
        crys = value;
        OnCrysChange?.Invoke(value);
    }

    public void ChangePower(int value)
    {
        power = value;
        OnPowerChange?.Invoke(value);
    }

    public void Upgrade()
    {
        level++;
        OnLevelChange?.Invoke(level);
        sixDim.hp += level;
        sixDim.atk += level;
        sixDim.def += level;
        sixDim.crit += level;
        sixDim.miss += level;
        sixDim.luck += level;
        OnSixDimChange?.Invoke(sixDim, level);
        Save();
    }


    public void Save()
    {
        string json = JsonConvert.SerializeObject(this);
        File.WriteAllText(savePath, json);
    }

    public static PlayerModel Load()
    {
        if (!File.Exists(savePath))
            return new PlayerModel();
        string json = File.ReadAllText(savePath);
        return JsonConvert.DeserializeObject<PlayerModel>(json);
    }
}
