using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "CardDataConfig", menuName = "ScriptableObjects/CardCreater/CardDataConfig", order = 1)]
public class CardDataConfig : ScriptableObject {
    public List<CardNormalConfig> cardList;
    public List<CardManagerConfig> cardManagerList;
    public List<int> cardPieceRequireByLevel;
    public List<int> burgerCoinRequireByLevel;
    public List<float> incomeRateByLevel;
    public List<float> processingRateByLevel;
    private void OnEnable() {
        cardPieceRequireByLevel.Clear();
        burgerCoinRequireByLevel.Clear();
        incomeRateByLevel.Clear();
        InitCardManagerData();
    }
    void InitCardManagerData() {
        cardPieceRequireByLevel = new List<int>() { 1, 2, 4, 8, 12, 18, 24, 30, 36, 42 };
        burgerCoinRequireByLevel = new List<int>() { 0, 25, 50, 100, 200, 350, 550, 800, 1200, 1500 };
        incomeRateByLevel = new List<float>() { 1, 1.5f, 2f, 2.5f, 3.5f, 5f, 7f, 9f, 12f, 15f, 20f };
        processingRateByLevel = new List<float>() { 0, 15f, 22f, 30f, 35f, 40f, 43f, 46f, 50f, 53f, 55f };
    }
    public CardNormalConfig GetCard(int cardID) {
        foreach (CardNormalConfig card in cardList) {
            if (card.ID == cardID)
                return card;
        }
        return null;
    }
    public CardManagerConfig GetCardManagerInfo(ManagerStaffID staffID, Rarity rarity) {
        return cardManagerList.Where(x => x.staffType == staffID && x.rarity == rarity).FirstOrDefault();
    }
    public int GetBurgerCoinRequireLevelUp(int level) {
        return burgerCoinRequireByLevel[level];
    }
    public int GetCardManagerRequireLevelUp(int level) {
        return cardPieceRequireByLevel[level];
    }
    public float GetIncomeRateByLevel(int level, Rarity rarity = Rarity.Rare) {
        return rarity switch {
            Rarity.Rare => incomeRateByLevel[level],
            Rarity.Epic => incomeRateByLevel[level] * 1.5f,
            Rarity.Legendary => incomeRateByLevel[level] * 2.5f,
            _ => throw new System.NotImplementedException(),
        };
    }
    public float GetProcessingRateByLevel(int level) {
        return processingRateByLevel[level];
    }
    public int GetGemPriceExchangeCard(int amount, Rarity rarity) {
        if (amount <= 0) return 0;
        return rarity switch {
            Rarity.Rare => amount * 25,
            Rarity.Epic => amount * 60,
            Rarity.Legendary => amount * 100,
            _ => 0,
        };
    }
    public int GetGemPriceExchangeBurgerCoin(int amount) {
        if (amount <= 0) return 0;
        return amount;
    }
}

[System.Serializable]
public class CardAmount {
    public CardNormalConfig card;
    public int amount = 0;
}

public enum CardType {
    NormalCard,
    CharacterCard
}