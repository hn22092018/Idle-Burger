using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[System.Serializable]
public class CardManagerSave {
    public ManagerStaffID staffID;
    public Rarity rarity;
    public int level;
    public int cardAmount;
    public bool isSelected;
}
[System.Serializable]
public class CardNormalSave {
    public int ID;
    public int level;
    public int currentAmount;
}

[System.Serializable]
public class CardManager {
    public List<CardNormalSave> cardNormals = new List<CardNormalSave>();
    public List<CardManagerSave> cardManagers = new List<CardManagerSave>();
    float financeRate;
    public List<int> ownerCardIAP_ID = new List<int>();
    public int bonusFinanceIAP;
    public int bonusOfflineTimeIAP;

    public void LoadData() {
        Debug.Log("CardManager LoadData");
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData)) {
            CardManager dataSave = JsonUtility.FromJson<CardManager>(jsonData);
            cardNormals = dataSave.cardNormals;
            cardManagers = dataSave.cardManagers;
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
            CardIAPConfig card = GetCardIAPByID(ownerCardIAP_ID[i]);
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
            CardIAPConfig card = GetCardIAPByID(ownerCardIAP_ID[i]);
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
    CardIAPConfig GetCardIAPByID(int id) {
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
            CardIAPConfig card = GetCardIAPByID(ownerCardIAP_ID[i]);
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
    public CardNormalSave GetOwnerCardByID(int cardID) {
        foreach (CardNormalSave ownedCard in cardNormals) {
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
        foreach (CardNormalSave card in cardNormals) {
            if (card.ID == cardID) {
                CardNormalConfig cardinfo = ProfileManager.Instance.dataConfig.cardData.GetCard(card.ID);
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
        CardNormalSave cardInfor = null;
        foreach (CardNormalSave card in cardNormals) {
            if (cardID == card.ID) {
                card.currentAmount += amount;
                haveCard = true;
                break;
            }
        }
        if (!haveCard) {
            cardInfor = new CardNormalSave();
            cardInfor.ID = cardID;
            cardInfor.currentAmount = amount;
            cardInfor.level = 1;
            cardNormals.Add(cardInfor);
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
        CardNormalSave cardSave = GetOwnerCardByID(cardID);
        if (GetOwnerCardByID(cardID) != null) {
            CardNormalConfig card = ProfileManager.Instance.dataConfig.cardData.GetCard(cardID);
            return card.cardValues[cardSave.level - 1];
        }
        return 0;
    }
    public int GetCardLevelByID(int cardID) {
        foreach (var cardSaved in cardNormals) {
            if (cardSaved.ID == cardID) return cardSaved.level;
        }
        return 0;
    }
    public int GetCardAmountByID(int cardID) {
        foreach (var cardSaved in cardNormals) {
            if (cardSaved.ID == cardID) return cardSaved.currentAmount;
        }
        return 0;
    }
    #endregion CARD NORMAL ============================================================
    #region CARD MANAGER =========
    public CardManagerSave GetCardManager(ManagerStaffID id) {
        CardManagerSave card = cardManagers.Where(x => x.isSelected && x.staffID == id).FirstOrDefault();
        if (card == null) {
            card = new CardManagerSave() {
                staffID = id,
                level = 0,
                isSelected = true,
                rarity = Rarity.Rare
            };
            cardManagers.Add(card);
            IsChangeData = true;
        }
        return card;
    }
    public void AddCardManager(int amount, ManagerStaffID id, Rarity rarity) {
        CardManagerSave card = cardManagers.Where(x => x.rarity == rarity && x.staffID == id).FirstOrDefault();
        card.cardAmount += amount;
        IsChangeData = true;
        SaveData();
    }
    public void ConsumeCardManager(int amount, ManagerStaffID id, Rarity rarity) {
        CardManagerSave card = cardManagers.Where(x => x.rarity == rarity && x.staffID == id).FirstOrDefault();
        card.cardAmount -= amount;
        IsChangeData = true;
        SaveData();
    }
    public void LevelUpCardManager(ManagerStaffID id, Rarity rarity) {
        CardManagerSave card = cardManagers.Where(x => x.rarity == rarity && x.staffID == id).FirstOrDefault();
        card.level++;
        if (card.level == 1) EventManager.TriggerEvent(EventName.UpdateCardManager.ToString(), (int)id);
        IsChangeData = true;
        SaveData();
    }
    #endregion
}

