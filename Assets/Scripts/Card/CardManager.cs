using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class CardManager {
    public List<CardSaveInfor> ownedCards = new List<CardSaveInfor>();
    public List<CardSaveInfor> ownedChars = new List<CardSaveInfor>();

    float financeRate;
    public List<int> ownerCardIAP_ID = new List<int>();
    public int bonusFinanceIAP;
    public int bonusOfflineTimeIAP;

    public void LoadData() {
        Debug.Log("CardManager LoadData");
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData)) {
            CardManager dataSave = JsonUtility.FromJson<CardManager>(jsonData);
            ownedCards = dataSave.ownedCards;
            ownedChars = dataSave.ownedChars;
            ownerCardIAP_ID = dataSave.ownerCardIAP_ID;
            bonusFinanceIAP = dataSave.bonusFinanceIAP;
            bonusOfflineTimeIAP = dataSave.bonusOfflineTimeIAP;
        }
        GetFinanceRate();
    }
    bool IsChangeData;
    public void SaveData() {
        if (!IsChangeData) return;
        IsChangeData = false;
        PlayerPrefs.SetString("CardManager", JsonUtility.ToJson(this).ToString());
    }
    string GetJsonData() {
        return PlayerPrefs.GetString("CardManager");
    }

    /// <summary>
    ///  Get Finance Rate From Finance Card IAP + Card Normal
    /// </summary>
    /// <returns></returns>
    public float GetFinanceRate() {
        float financeRate = 0f;
        bonusFinanceIAP = 0;
        financeRate += GetFinanceRate_CardIAP();
        financeRate += GetFinanceRate_CardNormal();
        if (ProfileManager.PlayerData.ResourceSave.isBoughtV2Pack) {
            bonusFinanceIAP += 100;
        }
        if (ProfileManager.PlayerData.ResourceSave.isBoughtV3Pack) {
            bonusFinanceIAP += 150;
        }
        financeRate += bonusFinanceIAP / 100f;
        return financeRate;
    }
   
    #region CARD IAP ============================================================
    /// <summary>
    ///  Get Extra Hour From Offine Time Card IAP. Default Value is 1 Hour
    /// </summary>
    /// <returns></returns>
    public int GetExtraHour_OfflineTimeCardIAP() {
        /// default value is 1 hour
        int value = 1;
        for (int i = 0; i < ownerCardIAP_ID.Count; i++) {
            CardIAP card = GetCardIAPByID(ownerCardIAP_ID[i]);
            if (card.type == CardIAPType.OFFLINE_TIME) value += card.extraValue;
        }
        value += bonusOfflineTimeIAP;
        return value;
    }

    /// <summary>
    ///  Calculate Finance Rate From Finance Card IAP.
    ///  Update value when buy a new card
    /// </summary>
    public float GetFinanceRate_CardIAP() {
        float rate = 100f;
        for (int i = 0; i < ownerCardIAP_ID.Count; i++) {
            CardIAP card = GetCardIAPByID(ownerCardIAP_ID[i]);
            if (card.type == CardIAPType.FINANCIAL_MANAGER) rate += card.extraValue;
        }
        return rate /= 100f;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public void AddCardIAP(int id) {
        IsChangeData = true;
        ownerCardIAP_ID.Add(id);
    }
    public void AddCardIAPOneTime(int id) {
        IsChangeData = true;
        if (!ownerCardIAP_ID.Contains(id)) ownerCardIAP_ID.Add(id);
    }
    /// <summary>
    /// Check cardIAP bought?
    /// </summary>
    /// <param name="carId"></param>
    /// <returns></returns>
    public bool IsOwnedCardIAP(int cardId) {
        if (ownerCardIAP_ID.IndexOf(cardId) == -1)
            return false;
        return true;
    }
    CardIAP GetCardIAPByID(int id) {
        return ProfileManager.Instance.dataConfig.shopConfig.GetCardByID(id);
    }

    /// <summary>
    ///  Get Finance Rate From Finance Card IAP
    /// </summary>
    /// <returns></returns>
    public float GetFinanceRate_FinanceCardIAP() {
        if (financeRate == 0) Calculate_FinanceRate_FinanceCardIAP();
        return financeRate;
    }
    /// <summary>
    ///  Calculate Finance Rate From Finance Card IAP.
    ///  Update value when buy a new card
    /// </summary>
    public void Calculate_FinanceRate_FinanceCardIAP() {
        financeRate = 100f;
        for (int i = 0; i < ownerCardIAP_ID.Count; i++) {
            CardIAP card = GetCardIAPByID(ownerCardIAP_ID[i]);
            if (card.type == CardIAPType.FINANCIAL_MANAGER) financeRate += card.extraValue;
        }
        financeRate /= 100f;
    }
    #endregion CARD IAP ============================================================
    #region CARD NORMAL ============================================================
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cardID"></param>
    /// <returns></returns>
    public CardSaveInfor GetOwnerCardByID(int cardID) {
        foreach (CardSaveInfor ownedCard in ownedCards) {
            if (ownedCard.ID == cardID)
                return ownedCard;
        }
        return null;
    }
    /// <summary>
    /// Upgrade level card
    /// UpgradeCard
    /// </summary>
    /// <param name="cardID"></param>
    public void UpgradeCard(int cardID) {
        foreach (CardSaveInfor card in ownedCards) {
            if (card.ID == cardID) {
                CardInfo cardinfo = ProfileManager.Instance.dataConfig.cardData.GetCard(card.ID);
                card.currentAmount = card.currentAmount - cardinfo.cardAmountLevel[card.level];
                //card.timeBlock = 15 * 60f * card.level + 5 * 60f;
                card.level += 1;
                card.level = Mathf.Clamp(card.level, 0, cardinfo.cardAmountLevel.Count);
                IsChangeData = true;
                return;
            }
        }
    }
    /// <summary>
    /// return true if add card success
    /// return false if add card fail ==> return player Gem.
    /// </summary>
    /// <param name="cardID"></param>
    /// <returns></returns>
    public void AddCard(int cardID, int amount = 1) {
        bool haveCard = false;
        CardSaveInfor cardInfor = null;
        foreach (CardSaveInfor card in ownedCards) {
            if (cardID == card.ID) {
                card.currentAmount += amount;
                haveCard = true;
                break;
            }
        }
        if (!haveCard) {
            cardInfor = new CardSaveInfor();
            cardInfor.ID = cardID;
            cardInfor.currentAmount = amount;
            cardInfor.level = 1;
            ownedCards.Add(cardInfor);
        }
        IsChangeData = true;
    }
    public float GetWaiterSpeedRate() {
        float rate = 100f;
        rate += GetCardValueByID(1);
        return rate / 100f;
    }
    public float GetCustomerSpeedRate() {
        float rate = 100f;
        rate += GetCardValueByID(2);
        return rate / 100f;
    }
    public float GetCleanerSpeedRate() {
        float rate = 100f;
        rate += GetCardValueByID(3);
        return rate / 100f;
    }
    public float GetChefCookingTimeRate() {
        float rate = 100f;
        rate -= GetCardValueByID(4);
        return rate / 100f;
    }
    public float GetChefSalaryRate() {
        float rate = 100f;
        rate -= GetCardValueByID(5);
        return rate / 100f;
    }
    public float GetWaiterSalaryRate() {
        float rate = 100f;
        rate -= GetCardValueByID(6);
        return rate / 100f;
    }
    public float GetCleanerSalaryRate() {
        float rate = 100f;
        rate -= GetCardValueByID(7);
        return rate / 100f;
    }
    public float GetCustomerWaitingTimeRate() {
        float rate = 100f;
        rate += GetCardValueByID(8);
        return rate / 100f;
    }
    public float GetCustomerWCTimeRate() {
        float rate = 100f;
        rate -= GetCardValueByID(9);
        return rate / 100f;
    }
    public float GetCustomerEatingTimeRate() {
        float rate = 100f;
        rate -= GetCardValueByID(10);
        return rate / 100f;
    }
    public float GetDeliciousFoodRate() {
        float rate = 100f;
        rate += GetCardValueByID(11);
        return rate / 100f;
    }
    public float GetReceptionistOrderTimeRate() {
        float rate = 100f;
        rate -= GetCardValueByID(12);
        return rate / 100f;
    }
    public float GetCustomerSatisfactionRate() {
        float rate = 100f;
        rate += GetCardValueByID(13);
        return rate / 100f;
    }
    public float GetRestaurantPopularityRate() {
        float rate = 0f;
        rate += GetCardValueByID(14);
        return rate;
    }
    public float GetCleanerWorkingTimeRate() {
        float rate = 100f;
        rate -= GetCardValueByID(15);
        return rate / 100f;
    }
    public float GetTipWaiterRate() {
        float rate = 100f;
        rate += GetCardValueByID(16);
        return rate / 100f;
    }
    public float GetTipReceptionRate() {
        float rate = 100f;
        rate += GetCardValueByID(17);
        return rate / 100f;
    }
    public float GetTipChefRate() {
        float rate = 100f;
        rate += GetCardValueByID(18);
        return rate / 100f;
    }
    public float GetTipCleanerRate() {
        float rate = 100f;
        rate += GetCardValueByID(19);
        return rate / 100f;
    }
    float GetFinanceRate_CardNormal() {
        float rate = 0f;
        rate += GetCardValueByID(20);
        return rate / 100f;
    }

    float GetCardValueByID(int cardID) {
        CardSaveInfor cardSave = GetOwnerCardByID(cardID);
        if (GetOwnerCardByID(cardID) != null) {
            CardInfo card = ProfileManager.Instance.dataConfig.cardData.GetCard(cardID);
            return card.cardValues[cardSave.level - 1];
        }
        return 0;
    }
    public int GetCardLevelByID(int cardID) {
        foreach (var cardSaved in ownedCards) {
            if (cardSaved.ID == cardID) return cardSaved.level;
        }
        return 0;
    }
    public int GetCardAmountByID(int cardID) {
        foreach (var cardSaved in ownedCards) {
            if (cardSaved.ID == cardID) return cardSaved.currentAmount;
        }
        return 0;
    }
    #endregion CARD NORMAL ============================================================
  
}

