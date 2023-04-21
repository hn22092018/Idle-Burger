using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CardDataConfig", menuName = "ScriptableObjects/CardCreater/CardDataConfig", order = 1)]
public class CardDataConfig : ScriptableObject
{
    public List<CardInfo> cardList;
    public CardInfo GetCard(int cardID) {
        foreach (CardInfo card in cardList)
        {
            if (card.ID == cardID)
                return card;
        }
        return null;
    }
}

[System.Serializable] 
public class CardAmount 
{
    public CardInfo card;
    public int amount = 0;
}

public enum CardType
{
    NormalCard,
    CharacterCard
}