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
    public List<WareHouseMaterial> flour;
    public List<WareHouseReward> wareHouseRewards = new List<WareHouseReward>();
    public WareHouseMaterial GetWareHouseMaterial(WareHouseMaterialType wareHouseMaterialType, int level) {
        switch (wareHouseMaterialType)
        {
            case WareHouseMaterialType.Cheese:
                return Getbiscuit(level);
            case WareHouseMaterialType.Pepper:
                return Getmelon(level);
            case WareHouseMaterialType.Sugar:
                return Getcandy(level);
            case WareHouseMaterialType.Carot:
                return Getsushi(level);
            case WareHouseMaterialType.Flour:
                return Getpotato(level);
            default:
                return null;
        }
    }
    public WareHouseMaterial Getbiscuit(int level) { return cheese[level - 1]; }
    public WareHouseMaterial Getmelon(int level) { return pepper[level - 1]; }
    public WareHouseMaterial Getcandy(int level) { return sugar[level - 1]; }
    public WareHouseMaterial Getsushi(int level) { return carot[level - 1]; }
    public WareHouseMaterial Getpotato(int level) { return flour[level - 1]; }


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
