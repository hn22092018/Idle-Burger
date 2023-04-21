using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoxInfo 
{
    public ItemType boxType;
    public int gemPrice;
    public int cardEarnAmount;
    public List<CardEarnable> cardEarnable;
    public float characterCardRate;
}

[System.Serializable]
public class CardEarnable
{
    public Rarity cardRarity;
    public float earnableChance; 
    public int preEarn;
}


[CreateAssetMenu(fileName = "BoxDataConfig", menuName = "ScriptableObjects/BoxDataConfig", order = 1)]
public class BoxDataConfig : ScriptableObject
{
    public List<BoxInfo> boxs;

    private void OnEnable()
    {
        boxs.Clear();
        AddBox1();
        AddBox2();
        AddBox3();
    }

    void AddBox1()
    {
        BoxInfo box = new BoxInfo();
        box.boxType = ItemType.FreeChest;
        box.gemPrice = 0;
        box.cardEarnAmount = 10;
        box.characterCardRate = 0;
        box.cardEarnable = new List<CardEarnable>();
        CardEarnable earn1 = new CardEarnable();
        earn1.cardRarity = Rarity.Common;
        earn1.preEarn = 0;
        earn1.earnableChance = 60;
        CardEarnable earn2 = new CardEarnable();
        earn2.cardRarity = Rarity.Rare;
        earn2.preEarn = 0;
        earn2.earnableChance = 40;
        box.cardEarnable.Add(earn1);
        box.cardEarnable.Add(earn2);
        boxs.Add(box);
    }

    void AddBox2()
    {
        BoxInfo box = new BoxInfo();
        box.boxType = ItemType.NormalChest;
        box.gemPrice = 200;
        box.cardEarnAmount = 40;
        box.characterCardRate = 0;
        box.cardEarnable = new List<CardEarnable>();
        CardEarnable earn1 = new CardEarnable();
        earn1.cardRarity = Rarity.Common;
        earn1.preEarn = 0;
        earn1.earnableChance = 50;
        CardEarnable earn2 = new CardEarnable();
        earn2.cardRarity = Rarity.Rare;
        earn2.preEarn = 0;
        earn2.earnableChance = 30;
        CardEarnable earn3 = new CardEarnable();
        earn3.cardRarity = Rarity.Epic;
        earn3.preEarn = 0;
        earn3.earnableChance = 20;
        box.cardEarnable.Add(earn1);
        box.cardEarnable.Add(earn2);
        box.cardEarnable.Add(earn3);
        boxs.Add(box);
    }

    void AddBox3()
    {
        BoxInfo box = new BoxInfo();
        box.boxType = ItemType.AdvancedChest;
        box.gemPrice = 500;
        box.cardEarnAmount = 120;
        box.characterCardRate = 0;
        box.cardEarnable = new List<CardEarnable>();
        CardEarnable earn1 = new CardEarnable();
        earn1.cardRarity = Rarity.Common;
        earn1.preEarn = 0;
        earn1.earnableChance = 40;
        CardEarnable earn2 = new CardEarnable();
        earn2.cardRarity = Rarity.Rare;
        earn2.preEarn = 0;
        earn2.earnableChance = 30;
        CardEarnable earn3 = new CardEarnable();
        earn3.cardRarity = Rarity.Epic;
        earn3.preEarn = 0;
        earn3.earnableChance = 20;
        CardEarnable earn4 = new CardEarnable();
        earn4.cardRarity = Rarity.Legendary;
        earn4.preEarn = 0;
        earn4.earnableChance = 10;
        box.cardEarnable.Add(earn1);
        box.cardEarnable.Add(earn2);
        box.cardEarnable.Add(earn3);
        box.cardEarnable.Add(earn4);
        boxs.Add(box);
    }
}
