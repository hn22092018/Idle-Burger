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
    public List<Sprite> sprConfig;
    public List<WareHouseMaterial> biscuit;
    public List<WareHouseMaterial> melon;
    public List<WareHouseMaterial> candy;
    public List<WareHouseMaterial> sushi;
    public List<WareHouseMaterial> potato;
    public List<WareHouseReward> wareHouseRewards = new List<WareHouseReward>();
    public WareHouseMaterial GetWareHouseMaterial(WareHouseMaterialType wareHouseMaterialType, int level) {
        switch (wareHouseMaterialType)
        {
            case WareHouseMaterialType.Biscuit:
                return Getbiscuit(level);
            case WareHouseMaterialType.Melon:
                return Getmelon(level);
            case WareHouseMaterialType.Candy:
                return Getcandy(level);
            case WareHouseMaterialType.Sushi:
                return Getsushi(level);
            case WareHouseMaterialType.Potato:
                return Getpotato(level);
            default:
                return null;
        }
    }
    public WareHouseMaterial Getbiscuit(int level) { return biscuit[level - 1]; }
    public WareHouseMaterial Getmelon(int level) { return melon[level - 1]; }
    public WareHouseMaterial Getcandy(int level) { return candy[level - 1]; }
    public WareHouseMaterial Getsushi(int level) { return sushi[level - 1]; }
    public WareHouseMaterial Getpotato(int level) { return potato[level - 1]; }

    private void OnEnable()
    {
        Addbiscuit();
        Addmelon();
        Addcandy();
        Addsushi();
        Addpotato();
    }
    void Addbiscuit() {
        biscuit.Clear();
        WareHouseMaterial biscuitLevel1 = new WareHouseMaterial();
        biscuitLevel1.priceSale = 1;
        biscuitLevel1.wareHouseMaterialType = WareHouseMaterialType.Biscuit;
        biscuitLevel1.icon = GetSpriteByName("Biscuit1");
        WareHouseMaterial biscuitLevel2 = new WareHouseMaterial();
        biscuitLevel2.priceSale = 2;
        biscuitLevel2.wareHouseMaterialType = WareHouseMaterialType.Biscuit;
        biscuitLevel2.icon = GetSpriteByName("Biscuit2");
        WareHouseMaterial biscuitLevel3 = new WareHouseMaterial();
        biscuitLevel3.priceSale = 4;
        biscuitLevel3.wareHouseMaterialType = WareHouseMaterialType.Biscuit;
        biscuitLevel3.icon = GetSpriteByName("Biscuit3");
        WareHouseMaterial biscuitLevel4 = new WareHouseMaterial();
        biscuitLevel4.priceSale = 8;
        biscuitLevel4.wareHouseMaterialType = WareHouseMaterialType.Biscuit;
        biscuitLevel4.icon = GetSpriteByName("Biscuit4");
        WareHouseMaterial biscuitLevel5 = new WareHouseMaterial();
        biscuitLevel5.priceSale = 16;
        biscuitLevel5.wareHouseMaterialType = WareHouseMaterialType.Biscuit;
        biscuitLevel5.icon = GetSpriteByName("Biscuit5");
        biscuit.Add(biscuitLevel1);
        biscuit.Add(biscuitLevel2);
        biscuit.Add(biscuitLevel3);
        biscuit.Add(biscuitLevel4);
        biscuit.Add(biscuitLevel5);
    }
    void Addmelon()
    {
        melon.Clear();
        WareHouseMaterial melonLevel1 = new WareHouseMaterial();
        melonLevel1.priceSale = 1;
        melonLevel1.wareHouseMaterialType = WareHouseMaterialType.Melon;
        melonLevel1.icon = GetSpriteByName("Melon1");
        WareHouseMaterial melonLevel2 = new WareHouseMaterial();
        melonLevel2.priceSale = 2;
        melonLevel2.wareHouseMaterialType = WareHouseMaterialType.Melon;
        melonLevel2.icon = GetSpriteByName("Melon2");
        WareHouseMaterial melonLevel3 = new WareHouseMaterial();
        melonLevel3.priceSale = 4;
        melonLevel3.wareHouseMaterialType = WareHouseMaterialType.Melon;
        melonLevel3.icon = GetSpriteByName("Melon3");
        WareHouseMaterial melonLevel4 = new WareHouseMaterial();
        melonLevel4.priceSale = 8;
        melonLevel4.wareHouseMaterialType = WareHouseMaterialType.Melon;
        melonLevel4.icon = GetSpriteByName("Melon4");
        WareHouseMaterial melonLevel5 = new WareHouseMaterial();
        melonLevel5.priceSale = 16;
        melonLevel5.wareHouseMaterialType = WareHouseMaterialType.Melon;
        melonLevel5.icon = GetSpriteByName("Melon5");
        melon.Add(melonLevel1);
        melon.Add(melonLevel2);
        melon.Add(melonLevel3);
        melon.Add(melonLevel4);
        melon.Add(melonLevel5);
    }
    void Addcandy()
    {
        candy.Clear();
        WareHouseMaterial candyLevel1 = new WareHouseMaterial();
        candyLevel1.priceSale = 1;
        candyLevel1.wareHouseMaterialType = WareHouseMaterialType.Candy;
        candyLevel1.icon = GetSpriteByName("Candy1");
        WareHouseMaterial candyLevel2 = new WareHouseMaterial();
        candyLevel2.priceSale = 2;
        candyLevel2.wareHouseMaterialType = WareHouseMaterialType.Candy;
        candyLevel2.icon = GetSpriteByName("Candy2");
        WareHouseMaterial candyLevel3 = new WareHouseMaterial();
        candyLevel3.priceSale = 4;
        candyLevel3.wareHouseMaterialType = WareHouseMaterialType.Candy;
        candyLevel3.icon = GetSpriteByName("Candy3");
        WareHouseMaterial candyLevel4 = new WareHouseMaterial();
        candyLevel4.priceSale = 8;
        candyLevel4.wareHouseMaterialType = WareHouseMaterialType.Candy;
        candyLevel4.icon = GetSpriteByName("Candy4");
        WareHouseMaterial candyLevel5 = new WareHouseMaterial();
        candyLevel5.priceSale = 16;
        candyLevel5.wareHouseMaterialType = WareHouseMaterialType.Candy;
        candyLevel5.icon = GetSpriteByName("Candy5");
        candy.Add(candyLevel1);
        candy.Add(candyLevel2);
        candy.Add(candyLevel3);
        candy.Add(candyLevel4);
        candy.Add(candyLevel5);
    }
    void Addsushi()
    {
        sushi.Clear();
        WareHouseMaterial sushiLevel1 = new WareHouseMaterial();
        sushiLevel1.priceSale = 1;
        sushiLevel1.wareHouseMaterialType = WareHouseMaterialType.Sushi;
        sushiLevel1.icon = GetSpriteByName("Sushi1");
        WareHouseMaterial sushiLevel2 = new WareHouseMaterial();
        sushiLevel2.priceSale = 2;
        sushiLevel2.wareHouseMaterialType = WareHouseMaterialType.Sushi;
        sushiLevel2.icon = GetSpriteByName("Sushi2");
        WareHouseMaterial sushiLevel3 = new WareHouseMaterial();
        sushiLevel3.priceSale = 4;
        sushiLevel3.wareHouseMaterialType = WareHouseMaterialType.Sushi;
        sushiLevel3.icon = GetSpriteByName("Sushi3");
        WareHouseMaterial sushiLevel4 = new WareHouseMaterial();
        sushiLevel4.priceSale = 8;
        sushiLevel4.wareHouseMaterialType = WareHouseMaterialType.Sushi;
        sushiLevel4.icon = GetSpriteByName("Sushi4");
        WareHouseMaterial sushiLevel5 = new WareHouseMaterial();
        sushiLevel5.priceSale = 16;
        sushiLevel5.wareHouseMaterialType = WareHouseMaterialType.Sushi;
        sushiLevel5.icon = GetSpriteByName("Sushi5");
        sushi.Add(sushiLevel1);
        sushi.Add(sushiLevel2);
        sushi.Add(sushiLevel3);
        sushi.Add(sushiLevel4);
        sushi.Add(sushiLevel5);
    }
    void Addpotato()
    {
        potato.Clear();
        WareHouseMaterial potatoLevel1 = new WareHouseMaterial();
        potatoLevel1.priceSale = 1;
        potatoLevel1.wareHouseMaterialType = WareHouseMaterialType.Potato;
        potatoLevel1.icon = GetSpriteByName("Potato1");
        WareHouseMaterial potatoLevel2 = new WareHouseMaterial();
        potatoLevel2.priceSale = 2;
        potatoLevel2.wareHouseMaterialType = WareHouseMaterialType.Potato;
        potatoLevel2.icon = GetSpriteByName("Potato2");
        WareHouseMaterial potatoLevel3 = new WareHouseMaterial();
        potatoLevel3.priceSale = 4;
        potatoLevel3.wareHouseMaterialType = WareHouseMaterialType.Potato;
        potatoLevel3.icon = GetSpriteByName("Potato3");
        WareHouseMaterial potatoLevel4 = new WareHouseMaterial();
        potatoLevel4.priceSale = 8;
        potatoLevel4.wareHouseMaterialType = WareHouseMaterialType.Potato;
        potatoLevel4.icon = GetSpriteByName("Potato4");
        WareHouseMaterial potatoLevel5 = new WareHouseMaterial();
        potatoLevel5.priceSale = 16;
        potatoLevel5.wareHouseMaterialType = WareHouseMaterialType.Potato;
        potatoLevel5.icon = GetSpriteByName("Potato5");
        potato.Add(potatoLevel1);
        potato.Add(potatoLevel2);
        potato.Add(potatoLevel3);
        potato.Add(potatoLevel4);
        potato.Add(potatoLevel5);
    }
    public Sprite GetSpriteByName(string name)
    {
        foreach (Sprite spr in sprConfig)
        {
            if (spr.name == name) return spr;
        }
        return null;
    }

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
