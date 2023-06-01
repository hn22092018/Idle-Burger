using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoxInfo 
{
    public ItemType boxType;
    public int cardEarnAmount;
    public List<CardEarnable> cardEarnable;
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
}
