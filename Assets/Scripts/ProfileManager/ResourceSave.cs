using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ResourceSave {
    /// <summary>
    /// gem
    /// </summary>
    public int gm;
    /// <summary>
    /// Burger Coin
    /// </summary>
    public int bCoin;
    /// <summary>
    /// resource data world 1
    /// </summary>
    public ResourcePerWorld rpw_1 = new ResourcePerWorld();
    /// <summary>
    /// resource data world 2
    /// </summary>
    public ResourcePerWorld rpw_2 = new ResourcePerWorld();
    /// <summary>
    /// resource data world 3
    /// </summary>
    public ResourcePerWorld rpw_3 = new ResourcePerWorld();
  
    /// <summary>
    /// adTicket
    /// </summary>
    public int adTicket;
    /// <summary>
    /// Time Skip Ticket 1H
    /// </summary>
    public int timeSkipTicket_1H;
    public int timeSkipTicket_4H;
    public int timeSkipTicket_24H;
    public int advancedChest;
    public int normalChest;
    /// <summary>
    /// normalSkinBox
    /// </summary>
    public int normalSkinbox;
    /// <summary>
    /// advancedSkinBox
    /// </summary>
    public int advancedSkinBox;
    /// <summary>
    /// expertSkinBox
    /// </summary>
    public int expertSkinBox;
    /// <summary>
    /// active Premium Suit
    /// </summary>
    public bool activePremiumSuit;
    /// <summary>
    /// active Golden Suit
    /// </summary>
    public bool activeGoldenSuit;
    public bool activeRemoveAds;
    public bool isBoughtV1Pack, isBoughtV2Pack, isBoughtV3Pack;
    public int countRewardRank;
    public bool isWatchedFreeGemAds;
    bool isChangeResource;

    public void LoadData() {
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData)) {
            ResourceSave dataSave = JsonUtility.FromJson<ResourceSave>(jsonData);
            rpw_1 = dataSave.rpw_1;
            rpw_2 = dataSave.rpw_2;
            rpw_3 = dataSave.rpw_3;
            //rpw_a.ch = 100000000;
            //rpw_2.ch = 100000000;
            gm = dataSave.gm;
            bCoin = dataSave.bCoin;
            adTicket = dataSave.adTicket;
            timeSkipTicket_1H = dataSave.timeSkipTicket_1H;
            timeSkipTicket_4H = dataSave.timeSkipTicket_4H;
            timeSkipTicket_24H = dataSave.timeSkipTicket_24H;
            advancedChest = dataSave.advancedChest;
            normalChest = dataSave.normalChest;
            activePremiumSuit = dataSave.activePremiumSuit;
            activeGoldenSuit = dataSave.activeGoldenSuit;
            activeRemoveAds = dataSave.activeRemoveAds;
            isBoughtV1Pack = dataSave.isBoughtV1Pack;
            isBoughtV2Pack = dataSave.isBoughtV2Pack;
            isBoughtV3Pack = dataSave.isBoughtV3Pack;
            normalSkinbox = dataSave.normalSkinbox;
            advancedSkinBox = dataSave.advancedSkinBox;
            expertSkinBox = dataSave.expertSkinBox;
            countRewardRank = dataSave.countRewardRank;
            isWatchedFreeGemAds = dataSave.isWatchedFreeGemAds;

        } else {
            rpw_1.ch = 2000;
            rpw_2.ch = 2000;
            rpw_3.ch = 2000;
        }
    }
    public void SaveData() {
        if (!isChangeResource) return;
        isChangeResource = false;
        PlayerPrefs.SetString("ResourceSave", JsonUtility.ToJson(this).ToString());
    }
    string GetJsonData() {
        return PlayerPrefs.GetString("ResourceSave");
    }
    public ResourcePerWorld GetResourceByWorld(int world) {
        switch (world) {
            case 1:
                return rpw_1;
            case 2:
                return rpw_2;
            case 3:
                return rpw_3;
        }
        return rpw_1;
    }
    public void SetRemoveAds(bool isRemoveAds) {
        this.activeRemoveAds = isRemoveAds;
        EventManager.TriggerEvent("UpdateRemoteConfigs");
        if (this.activeRemoveAds) {
            AdsManager.Instance.HideBannerAds();
        }
    }
    public void ConsumeCash(int world, BigNumber amount) {
        isChangeResource = true;
        ResourcePerWorld data = GetResourceByWorld(world);
        data.ch.Substract(amount);

    }
    public void AddCash(int world, BigNumber amount) {
        isChangeResource = true;
        ResourcePerWorld data = GetResourceByWorld(world);
        data.ch.Add( amount);
    }
    public BigNumber GetCash(int world) {
        ResourcePerWorld data = GetResourceByWorld(world);
        return data.ch;
    }
    public void ConsumeGem(int amount) {
        isChangeResource = true;
        gm -= amount;
        if (gm <= 0) gm = 0;
    }
    public void AddGem(int amount) {
        isChangeResource = true;
        gm += amount;
    }
    public int GetGem() {
        return gm;
    }
    public void ConsumeBCoin(int amount) {
        isChangeResource = true;
        bCoin -= amount;
        if (bCoin <= 0) bCoin = 0;
    }
    public void AddBCoin(int amount) {
        isChangeResource = true;
        bCoin += amount;
    }
    public int GetBCoin() {
        return bCoin;
    }
    public BigNumber GetTotalTipProfit(int world) {
        ResourcePerWorld data = GetResourceByWorld(world);
        return data.trn + data.twr + data.tcf ;
    }
    public void ResetTipProfit(int world) {
        isChangeResource = true;
        ResourcePerWorld data = GetResourceByWorld(world);
        data.trn = data.twr = data.tcf  = 0;
    }
    public void AddTipChef(int world, BigNumber value) {
        isChangeResource = true;
        ResourcePerWorld data = GetResourceByWorld(world);
        data.tcf += value;
    }
    public BigNumber GetTipChef(int world) {
        ResourcePerWorld data = GetResourceByWorld(world);
        return data.tcf;
    }

    public void AddTipReception(int world, BigNumber value) {
        isChangeResource = true;
        ResourcePerWorld data = GetResourceByWorld(world);
        data.trn += value;
    }
    public BigNumber GetTipReception(int world) {
        ResourcePerWorld data = GetResourceByWorld(world);
        return data.trn;
    }
    public void AddTipWaiter(int world, BigNumber value) {
        isChangeResource = true;
        ResourcePerWorld data = GetResourceByWorld(world);
        data.twr += value;
    }
    public void ConsumeSkinBox(ItemType itemType) {
        isChangeResource = true;
        switch (itemType) {
            case ItemType.NormalSkinBox:
                normalSkinbox--;
                break;
            case ItemType.AdvancedSkinBox:
                advancedSkinBox--;
                break;
            case ItemType.FreeSkinBox:
                expertSkinBox--;
                break;
            default:
                break;
        }
    }
    public BigNumber GetTipWaiter(int world) {
        ResourcePerWorld data = GetResourceByWorld(world);
        return data.twr;
    }
    public int GetADTicket() {
        return adTicket;
    }
    public void ConsumeADTicket(int amount = 1) {
        adTicket -= amount;
        if (adTicket < 0) adTicket = 0;
        isChangeResource = true;
    }
    public void AddADTicket(int amount) {
        adTicket += amount;
        isChangeResource = true;
    }
    public int GetTimeSkipTicket_1H() {
        return timeSkipTicket_1H;
    }
    public void AddTimeSkipTicket_1H(int amount) {
        timeSkipTicket_1H += amount;
        isChangeResource = true;
    }
    public void ConsumeTimeSkipTicket_1H(int amount) {
        timeSkipTicket_1H -= amount;
        if (timeSkipTicket_1H < 0) timeSkipTicket_1H = 0;
        isChangeResource = true;
    }
    public int GetTimeSkipTicket_4H() {
        return timeSkipTicket_4H;
    }
    public void AddTimeSkipTicket_4H(int amount) {
        timeSkipTicket_4H += amount;
        isChangeResource = true;
    }
    public void ConsumeTimeSkipTicket_4H(int amount) {
        timeSkipTicket_4H -= amount;
        if (timeSkipTicket_4H < 0) timeSkipTicket_4H = 0;
        isChangeResource = true;
    }
    public int GetTimeSkipTicket_24H() {
        return timeSkipTicket_24H;
    }
    public void AddTimeSkipTicket_24H(int amount) {
        timeSkipTicket_24H += amount;
        isChangeResource = true;
    }
    public void ConsumeTimeSkipTicket_24H(int amount) {
        timeSkipTicket_24H -= amount;
        if (timeSkipTicket_24H < 0) timeSkipTicket_24H = 0;
        isChangeResource = true;
    }
    public void AddAdvancedChest(int amount = 1) {
        advancedChest += amount;
        isChangeResource = true;
    }
    public void ConsumeAdvancedChest(int amount = 1) {
        advancedChest -= amount;
        if (advancedChest < 0) advancedChest = 0;
        isChangeResource = true;
    }
    public int GetAdvancedChest() {
        return advancedChest;
    }
    public void AddNormalChest(int amount = 1) {
        normalChest += amount;
        isChangeResource = true;
    }
    public void ConsumeNormalChest(int amount = 1) {
        normalChest -= amount;
        if (normalChest < 0) normalChest = 0;
        isChangeResource = true;
    }
    public int GetNormalChest() {
        return normalChest;
    }

    public void AddNormalSkinBox(int amount = 1) {
        normalSkinbox += amount;
        isChangeResource = true;
    }
    public void AddAdvanceSkinBox(int amount = 1) {
        advancedSkinBox += amount;
        isChangeResource = true;
    }
    public void AddExpertSkinBox(int amount = 1) {
        expertSkinBox += amount;
        isChangeResource = true;
    }
    public void ClaimRewardRank() {
        countRewardRank++;
    }
    public void ResetFreeGemAds() {
        isWatchedFreeGemAds = false;
    }
    public void WatchedFreeGemAds() {
        isWatchedFreeGemAds = true;
    }
    public bool IsWatchedFreeGemAds() {
        return isWatchedFreeGemAds;
    }

}
[System.Serializable]
public class ResourcePerWorld {
    /// <summary>
    /// cash
    /// </summary>
    public BigNumber ch;
    /// <summary>
    /// tip chef
    /// </summary>
    public BigNumber tcf;
    /// <summary>
    /// tip waiter
    /// </summary>
    public BigNumber twr;
    /// <summary>
    /// tip reception
    /// </summary>
    public BigNumber trn;

}
