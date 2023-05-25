using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class BoxManager {
    public float timeBlockFreeChest = 0;
    public int freeChestAvaible = 10;
    public int dayReal;
    bool cardListFilled = false;
    BoxDataConfig boxData;
    BoxInfo boxToOpen;
    Dictionary<int, List<CardNormalConfig>> cardListByType = new Dictionary<int, List<CardNormalConfig>>();  // Common, Rare, Epic, Legendary card (in this order)
    List<int> amountOfEachType = new List<int>(); // Amount of all card in the same type Common, Rare, Epic, Legendary (in this order) Eg: 10 card rare, ..
    List<CardNormalConfig> QueueEarnableList = new List<CardNormalConfig>(); // List of the list earned Card, so each list should have different rarity card. // Show each list on scene
    List<CardAmount> amountOfEachCard = new List<CardAmount>(); // Eg: 10 cardA, 5 cardB
    public void LoadData() {
        Debug.Log("BoxManager LoadData");
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData)) {
            BoxManager dataSave = JsonUtility.FromJson<BoxManager>(jsonData);
            timeBlockFreeChest = dataSave.timeBlockFreeChest;
            freeChestAvaible = dataSave.freeChestAvaible;
            dayReal = dataSave.dayReal;
        }
        boxData = ProfileManager.Instance.dataConfig.boxData;
        amountOfEachCard.Clear();
        float offTime = ProfileManager.PlayerData.GetCustomTimeManager().GetOfflineTimeExactly();
        if (timeBlockFreeChest > 0) {
            timeBlockFreeChest -= offTime;
            IsChangeData = true;
        }
        if (freeChestAvaible <= 0) freeChestAvaible = 0;
    }
    bool IsChangeData;
    public void SaveData() {
        if (!IsChangeData) return;
        IsChangeData = false;
        PlayerPrefs.SetString("BoxManager", JsonUtility.ToJson(this).ToString());
    }
    string GetJsonData() {
        return PlayerPrefs.GetString("BoxManager");
    }
    public void Update() {
        if (timeBlockFreeChest > 0) {
            timeBlockFreeChest -= Time.deltaTime;
            IsChangeData = true;
        }
        if (System.DateTime.Now.DayOfYear != dayReal) {
            dayReal = System.DateTime.Now.DayOfYear;
            freeChestAvaible = 10;
            timeBlockFreeChest = 0;
            IsChangeData = true;
        }
    }
    public bool IsHasFreeChest() {
        return freeChestAvaible > 0 && timeBlockFreeChest <= 0;
    }
    public void ClaimFreeChest() {
        freeChestAvaible--;
        if (freeChestAvaible <= 0) freeChestAvaible = 0;
        timeBlockFreeChest = 2 * 60 * 60;
        IsChangeData = true;
    }
    public void ForceSkipBlockTimeFreeChest() {
        timeBlockFreeChest = 0;
        IsChangeData = true;
    }
    void FillListByType() {
        cardListFilled = true;
        List<CardNormalConfig> cardList = ProfileManager.Instance.dataConfig.cardData.cardList;
        List<CardNormalConfig> commons = new List<CardNormalConfig>();
        List<CardNormalConfig> rares = new List<CardNormalConfig>();
        List<CardNormalConfig> epics = new List<CardNormalConfig>();
        List<CardNormalConfig> lengends = new List<CardNormalConfig>();
        foreach (var card in cardList) {
            switch (card.cardRarity) {
                case Rarity.Common:
                    commons.Add(card);
                    break;
                case Rarity.Rare:
                    rares.Add(card);
                    break;
                case Rarity.Epic:
                    epics.Add(card);
                    break;
                case Rarity.Legendary:
                    lengends.Add(card);
                    break;
            }
        }
        cardListByType.Add(0, commons);
        cardListByType.Add(1, rares);
        cardListByType.Add(2, epics);
        cardListByType.Add(3, lengends);

    }

    public BoxInfo GetBoxInfo(ItemType boxType) //
    {
        Debug.Log("Find Box");
        for (int i = 0; i < boxData.boxs.Count; i++) {
            if (boxData.boxs[i].boxType == boxType) {
                return boxData.boxs[i];
            }
        }
        return null;
    }

    public void OpenBox(ItemType boxOpenType) {
        //cardEarnList.Clear();
        if(!cardListFilled)
        {
            FillListByType();
        }
        amountOfEachType = new List<int>();
        boxToOpen = GetBoxInfo(boxOpenType);
        QueueEarnableList.Clear();
        amountOfEachCard.Clear();
        RandomCard();
        AddToCardManager();
        UIManager.instance.ShowPanelReward(boxOpenType);
    }
    void RandomCard() {
        float totalChanceToOpen = 0f;
        int ensureAmount = 0;
        for (int i = 0; i < boxToOpen.cardEarnable.Count; i++) {
            amountOfEachType.Add(boxToOpen.cardEarnable[i].preEarn);
            ensureAmount += boxToOpen.cardEarnable[i].preEarn;
            totalChanceToOpen += boxToOpen.cardEarnable[i].earnableChance;
        }
        // generate type by chance.
        for (int j = 0; j < boxToOpen.cardEarnAmount - ensureAmount; j++) {
            float rand = Random.Range(0f, totalChanceToOpen);
            float top = 0f;
            for (int i = 0; i < boxToOpen.cardEarnable.Count; i++) {
                top += boxToOpen.cardEarnable[i].earnableChance;
                if (rand < top) {
                    amountOfEachType[i] += 1;
                    break;
                }
            }
        }
        // generate card by card type
        for (int i = 0; i < boxToOpen.cardEarnable.Count; i++) {
            for (int j = 0; j < amountOfEachType[i]; j++) {
                int type =(int) boxToOpen.cardEarnable[i].cardRarity;
                int cardIndex = Random.Range(0, cardListByType[type].Count);
                QueueEarnableList.Add(cardListByType[i][cardIndex]);
                AddToCountList(cardListByType[i][cardIndex]);
            }
        }
        //if(amountOfEachType[boxToOpen.cardEarnable.Count] > 0 )
        //{
        //    for(int i = 0; i < amountOfEachType[boxToOpen.cardEarnable.Count]; i++)
        //    {
        //        int cardIndex = Random.Range(0, cardListByType[4].Count);
        //        QueueEarnableList.Add(cardListByType[4][cardIndex]);
        //        AddToCountList(cardListByType[4][cardIndex]);
        //    }
        //}
    }

    void AddToCountList(CardNormalConfig cardToCount) {
        for (int i = 0; i < amountOfEachCard.Count; i++) {
            if (amountOfEachCard[i].card == cardToCount) {
                amountOfEachCard[i].amount++;
                return;
            }
        }
        CardAmount newCard = new CardAmount();
        newCard.card = cardToCount;
        newCard.amount = 1;
        amountOfEachCard.Add(newCard);
    }

    public void AddToCardManager() {
        for (int i = 0; i < amountOfEachCard.Count; i++) {
            switch (amountOfEachCard[i].card.cardType)
            {
                case CardType.NormalCard:
                    ProfileManager.PlayerData.GetCardManager().AddCard(amountOfEachCard[i].card.ID, amountOfEachCard[i].amount);
                    break;

                default:
                    break;
            }

        }
    }

    public List<CardAmount> GetEarnedCardList() {
        return amountOfEachCard;
    }

}
