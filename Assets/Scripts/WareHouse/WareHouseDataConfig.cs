using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WareHouseReward {
    public ItemReward reward;
    public float persent;
}
[CreateAssetMenu(fileName = "WareHouseData", menuName = "ScriptableObjects/WareHouseData")]
public class WareHouseDataConfig : ScriptableObject
{
    public List<WareHouseMaterial> cheese;
    public List<WareHouseMaterial> pepper;
    public List<WareHouseMaterial> sugar;
    public List<WareHouseMaterial> carot;
    public List<WareHouseReward> wareHouseRewards = new List<WareHouseReward>();
    public WareHouseMaterial GetWareHouseMaterial(WareHouseMaterialType wareHouseMaterialType, int level) {
        switch (wareHouseMaterialType)
        {
            case WareHouseMaterialType.Cheese:
                return GetCheese(level);
            case WareHouseMaterialType.Pepper:
                return GetPepper(level);
            case WareHouseMaterialType.Sugar:
                return GetSugar(level);
            case WareHouseMaterialType.Carot:
                return GetCarot(level);
            default:
                return null;
        }
    }
    public WareHouseMaterial GetCheese(int level) { return cheese[level - 1]; }
    public WareHouseMaterial GetPepper(int level) { return pepper[level - 1]; }
    public WareHouseMaterial GetSugar(int level) { return sugar[level - 1]; }
    public WareHouseMaterial GetCarot(int level) { return carot[level - 1]; }


    float index;
    public ItemReward GetWareHouseReward()
    {
        index = Random.Range(0, 100);
        for (int i = 0; i < wareHouseRewards.Count; i++)
        {
            if (index < wareHouseRewards[i].persent)
                return wareHouseRewards[i].reward;
            else index -= wareHouseRewards[i].persent;
        }
        return wareHouseRewards[wareHouseRewards.Count - 1].reward;
    }
}
