using SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerData {
    [Header("=======Resources Data========")]
    public ResourceSave ResourceSave = new ResourceSave();
    [Header("=======Quest Data========")]
    public QuestSave QuestSave = new QuestSave();
    [Header("=======Negotiation Data========")]
    public OrderSave OrderSave = new OrderSave();
    [Header("=======Room Data========")]
    public RoomSave RoomSave_W1 = new RoomSave();
    public RoomSave RoomSave_W2 = new RoomSave();
    public RoomSave RoomSave_W3 = new RoomSave();

    [Header("=======Card=====")]
    public CardManager cardManager = new CardManager();
    [Header("=====LimitOfferManager========")]
    public SaleOfferManager limitOfferManager = new SaleOfferManager();
    [Header("=====BoxManager=====")]
    public BoxManager boxManager = new BoxManager();
    [Header("=======Overviews=====")]
    public MarketingManager marketingManager = new MarketingManager();
    [Header("=======AdBoost=====")]
    public AdBoostManager adBoostManager = new AdBoostManager();
    public CustomTimeManager timeManager = new CustomTimeManager();
    [Header("=====ResearchManager=====")]
    public ResearchManager researchManager = new ResearchManager();
    [Header("=====SkinManager=====")]
    public SkinManager skinManager = new SkinManager();
    [Header("=====DailyGift=====")]
    public DailyRewardsManager dailyRewardManager = new DailyRewardsManager();
    [Header("=====WareHouse=====")]
    public WareHouseManager wareHouseManager = new WareHouseManager();
    [Header("=====RewardRoomManager=====")]
    public RewardRoomManager _RewardRoomManager = new RewardRoomManager();
    public int selectedWorld = 1;
    public int unlockedWorld = 1;
    public void LoadData() {
        Debug.Log("LoadData");
        selectedWorld = GetSelectedWorld();
        unlockedWorld = GetUnlockedWorld();
        ResourceSave.LoadData();
        timeManager.LoadData();
        boxManager.LoadData();
        cardManager.LoadData();
        marketingManager.LoadData();
        adBoostManager.LoadData();
        QuestSave.LoadData();
        OrderSave.LoadData();
        RoomSave_W1.LoadData(1);
        RoomSave_W2.LoadData(2);
        RoomSave_W3.LoadData(3);
        researchManager.LoadData();
        wareHouseManager.LoadData();
        _RewardRoomManager.LoadData();
        skinManager.LoadData();
        dailyRewardManager.LoadData();

    }
    public void ChangeSelectedWorld(int world) {
        selectedWorld = world;
        switch (world) {
            case 1:
                RoomSave_W1.LoadData(selectedWorld);
                break;
            case 2:
                RoomSave_W2.LoadData(selectedWorld);
                break;

            case 3:
                RoomSave_W3.LoadData(selectedWorld);
                break;
        }

        OrderSave.LoadData();
        PlayerPrefs.SetInt("SelectedWorld", selectedWorld);
    }
    public int GetSelectedWorld() {
        return PlayerPrefs.GetInt("SelectedWorld", 1);
    }
    public int GetEventSelected() {
        return PlayerPrefs.GetInt("SelectedEvent", 0);
    }
    public int GetUnlockedWorld() {
        return PlayerPrefs.GetInt("UnlockedWorld", 1);
    }
    public void SaveUnlockedWorld() {
        PlayerPrefs.SetInt("UnlockedWorld", unlockedWorld);
    }

    float capTimeSaving = 10;
    float deltaTimeSaving;
    bool isSaving;
    public void Update() {
        adBoostManager.Update();
        marketingManager.Update();
        boxManager.Update();
        researchManager.Update();
        dailyRewardManager.Update();
        wareHouseManager.Update();
        OrderSave.Update();
        deltaTimeSaving += Time.unscaledDeltaTime;
        if (deltaTimeSaving >= capTimeSaving && !isSaving) {
            isSaving = true;
            SaveData();
        }
    }

    public void SaveData() {
        Debug.Log("SaveData");
        QuestSave.SaveData();
        SaveRoom();
        ResourceSave.SaveData();
        OrderSave.SaveData();
        timeManager.SaveData();
        adBoostManager.SaveData();
        marketingManager.SaveData();
        boxManager.SaveData();
        cardManager.SaveData();
        researchManager.SaveData();
        skinManager.SaveData();
        wareHouseManager.SaveData();
        SaveUnlockedWorld();
        deltaTimeSaving = 0;
        isSaving = false;
    }
    void SaveRoom() {
        switch (selectedWorld) {
            case 1:
                RoomSave_W1.SaveData(selectedWorld);
                break;
            case 2:
                RoomSave_W2.SaveData(selectedWorld);
                break;
            case 3:
                RoomSave_W3.SaveData(selectedWorld);
                break;
        }
    }

    public void IsMarkChangeQuest() {
        QuestSave.IsMarkChangeData();
    }
    #region Room Data
    public BaseRoom<T> GetRoomData<T>(BaseRoom<T> baseRoom) {
        switch (selectedWorld) {
            case 1:
                return RoomSave_W1.GetRoomSaveData().GetRoomData<T>(baseRoom.roomID, baseRoom.GroupID);
            case 2:
                return RoomSave_W2.GetRoomSaveData().GetRoomData<T>(baseRoom.roomID, baseRoom.GroupID);
            case 3:
                return RoomSave_W3.GetRoomSaveData().GetRoomData<T>(baseRoom.roomID, baseRoom.GroupID);
        }
        return RoomSave_W1.GetRoomSaveData().GetRoomData<T>(baseRoom.roomID, baseRoom.GroupID);
    }
    public StaffConfig GetStaffConfigData(StaffConfig staffconfig) {
        switch (selectedWorld) {
            case 1:
                return RoomSave_W1.GetRoomSaveData().GetStaffConfigData(staffconfig);
            case 2:
                return RoomSave_W2.GetRoomSaveData().GetStaffConfigData(staffconfig);
            case 3:
                return RoomSave_W3.GetRoomSaveData().GetStaffConfigData(staffconfig);
        }
        return RoomSave_W1.GetRoomSaveData().GetStaffConfigData(staffconfig);
    }
    public void SaveRoomData<T>(BaseRoom<T> room, bool isOverideData = true) {
        switch (selectedWorld) {
            case 1:
                RoomSave_W1.IsMarkChangeData();
                RoomSave_W1.GetRoomSaveData().SaveRoomData(room, isOverideData);
                break;
            case 2:
                RoomSave_W2.IsMarkChangeData();
                RoomSave_W2.GetRoomSaveData().SaveRoomData(room, isOverideData);
                break;
            case 3:
                RoomSave_W3.IsMarkChangeData();
                RoomSave_W3.GetRoomSaveData().SaveRoomData(room, isOverideData);
                break;
        }

    }
    public void SaveStaffData(StaffConfig staffConfig) {
        switch (selectedWorld) {
            case 1:
                RoomSave_W1.IsMarkChangeData();
                RoomSave_W1.GetRoomSaveData().SaveStaffData(staffConfig);
                break;
            case 2:
                RoomSave_W2.IsMarkChangeData();
                RoomSave_W2.GetRoomSaveData().SaveStaffData(staffConfig);
                break;
            case 3:
                RoomSave_W3.IsMarkChangeData();
                RoomSave_W3.GetRoomSaveData().SaveStaffData(staffConfig);
                break;
        }

    }


    public bool IsWolrdUnlocked(int world) {
        if (world <= unlockedWorld) {
            return true;
        }
        return false;
    }
    public void UnlockWorld(int world) {
        if (world > GetUnlockedWorld())
            unlockedWorld = world;
    }
    #endregion Room Data

    #region Resource
    public void ConsumeCash(BigNumber amount) {
        ResourceSave.ConsumeCash(selectedWorld, new BigNumber(amount));
        EventManager.TriggerEvent(EventName.UpdateMoney.ToString());
    }
    public void AddCash(float amount) {
        ResourceSave.AddCash(selectedWorld, amount);
        EventManager.TriggerEvent(EventName.UpdateMoney.ToString());
    }
    public void AddCash(BigNumber amount) {
        ResourceSave.AddCash(selectedWorld, new BigNumber(amount));
        EventManager.TriggerEvent(EventName.UpdateMoney.ToString());
    }
    public BigNumber GetCash() {
        return ResourceSave.GetCash(selectedWorld);
    }
    public int GetResearchValue() {
        return researchManager.GetResearchValue();
    }
    public BigNumber GetCashByWorld(int world = 1) {
        return ResourceSave.GetCash(world);
    }
    public void ConsumeGem(int amount) {
        ResourceSave.ConsumeGem(amount);
        EventManager.TriggerEvent(EventName.UpdateGem.ToString());
    }
    public void AddGem(int amount) {
        ResourceSave.AddGem(amount);
        EventManager.TriggerEvent(EventName.UpdateGem.ToString());
    }
    public int GetGem() {
        return ResourceSave.GetGem();
    }
    public void ConsumeBCoin(int amount) {
        ResourceSave.ConsumeBCoin(amount);
        EventManager.TriggerEvent(EventName.UpdateBCoin.ToString());
    }
    public void AddBCoin(int amount) {
        ResourceSave.AddBCoin(amount);
        EventManager.TriggerEvent(EventName.UpdateBCoin.ToString());
    }
    public int GetBurgerCoin() {
        return ResourceSave.GetBCoin();
    }
    public BigNumber GetTotalTipProfit() {
        return ResourceSave.GetTotalTipProfit(selectedWorld);
    }
    public void ResetTipProfit() {
        ResourceSave.ResetTipProfit(selectedWorld);
    }
    public void AddTipChef(BigNumber value) {
        ResourceSave.AddTipChef(selectedWorld, value);
    }
    public BigNumber GetTipChef() {
        return ResourceSave.GetTipChef(selectedWorld);
    }
   
    public void AddTipReception(BigNumber value) {
        ResourceSave.AddTipReception(selectedWorld, value);
    }
    public BigNumber GetTipReception() {
        return ResourceSave.GetTipReception(selectedWorld);
    }
    public void AddTipWaiter(BigNumber value) {
        ResourceSave.AddTipWaiter(selectedWorld, value);
    }
    public BigNumber GetTipWaiter() {
        return ResourceSave.GetTipWaiter(selectedWorld);
    }
    public void ConsumeTimeTravelTicket_1H(int value) { ResourceSave.ConsumeTimeSkipTicket_1H(value); }
    public void ConsumeTimeTravelTicket_4H(int value) { ResourceSave.ConsumeTimeSkipTicket_1H(value); }
    public void ConsumeTimeTravelTicket_24H(int value) { ResourceSave.ConsumeTimeSkipTicket_1H(value); }
    public int GetTimeTravelTicket_1H() { return ResourceSave.GetTimeSkipTicket_1H(); }
    public int GetTimeTravelTicket_4H() { return ResourceSave.GetTimeSkipTicket_4H(); }
    public int GetTimeTravelTicket_24H() { return ResourceSave.GetTimeSkipTicket_24H(); }
    //public void AddDropReputationStaff(StaffID staffID) {
    //    switch (staffID) {
    //        case StaffID.Lobby_Receptioner:
    //            ResourceSave.GetResourceByWorld(selectedWorld).Reputation_Lobby++;
    //            break;
    //        case StaffID.Waiter:
    //            ResourceSave.GetResourceByWorld(selectedWorld).Reputation_Waiter++;
    //            break;
    //        case StaffID.Chef:
    //            ResourceSave.GetResourceByWorld(selectedWorld).Reputation_Chef++;
    //            break;
    //        case StaffID.Cleaner:
    //            ResourceSave.GetResourceByWorld(selectedWorld).Reputation_Cleaner++;
    //            break;
    //        case StaffID.Bartender:
    //            ResourceSave.GetResourceByWorld(selectedWorld).Reputation_Bartender++;
    //            break;
    //        case StaffID.Barista:
    //            ResourceSave.GetResourceByWorld(selectedWorld).Reputation_Barista++;
    //            break;

    //    }
    //}
    #endregion Resource
    public OrderBookManager GetOrderBookManager() {
        return OrderSave.GetOrderBookManager(selectedWorld);
    }

    public AdBoostManager GetAdBoostManager() {
        return adBoostManager;
    }
    public MarketingManager GetMarketingManager() {
        return marketingManager;
    }
    public QuestManager GetQuestManager() {
        return QuestSave.GetQuestManager(selectedWorld);
    }
    public CardManager GetCardManager() {
        return cardManager;
    }
    public CustomTimeManager GetCustomTimeManager() {
        return timeManager;
    }
    public void OnSaveBoughtIAPPackage(OfferID offerID) {
        switch (offerID) {
            case OfferID.NoAds:
                ResourceSave.SetRemoveAds(true);
                break;
            case OfferID.Vip1Pack:
                ResourceSave.isBoughtV1Pack = true;
                break;
            case OfferID.Vip2Pack:
                ResourceSave.isBoughtV2Pack = true;
                break;
            case OfferID.Vip3Pack:
                ResourceSave.isBoughtV3Pack = true;
                break;
            case OfferID.ComboPack_Ads_Researcher_Order:
                ResourceSave.SetRemoveAds(true);
                break;
        }
    }
    public bool IsBoughtIAPPackage(OfferID offerID) {
        switch (offerID) {
            case OfferID.NoAds:
                return ResourceSave.activeRemoveAds;
            case OfferID.Vip1Pack:
                return ResourceSave.isBoughtV1Pack;
            case OfferID.Vip2Pack:
                return ResourceSave.isBoughtV2Pack;
            case OfferID.Vip3Pack:
                return ResourceSave.isBoughtV3Pack;
            case OfferID.ComboPack_Ads_Researcher_Order:
                return ResourceSave.activeRemoveAds;
        }
        return false;
    }

}
