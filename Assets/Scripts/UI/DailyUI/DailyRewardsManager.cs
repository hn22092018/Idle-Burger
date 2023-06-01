using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class DailyRewardsManager {
    public const string strCollectedDay = "DailyGift_Collected_Day_";
    private const string keyDay = "DailyRewardDay";
    private const string keyDayOfYear = "DayOfYear";
    [SerializeField]
    private int dayPrefs;
    [SerializeField]
    private int dayOfYearPrefs;
    public void LoadData() {
        Debug.Log("DailyRewardsManager  InitData");
        ProfileManager.Instance.dataConfig.dailyRewardConfig.Load();
        if (!PlayerPrefs.HasKey(keyDay)) {
            ProfileManager.Instance.dataConfig.dailyRewardConfig.Reset();
            SaveDay(1);
        } else {
            dayPrefs = GetDayPrefs();
            dayOfYearPrefs = GetDayOfYearPrefs();
        }
    }
    public void Update() {
        CompareTime();
    }
    void SaveDay(int day) {
        dayPrefs = day;
        dayOfYearPrefs = DateTime.Now.DayOfYear;
        PlayerPrefs.SetInt(keyDay, dayPrefs);
        PlayerPrefs.SetInt(keyDayOfYear, dayOfYearPrefs);
    }
    void IncreaseDayPrefs() {
        dayPrefs++;
        if (dayPrefs > 7) {
            dayPrefs = 1;
            ProfileManager.Instance.dataConfig.dailyRewardConfig.Reset();
        }
        SaveDay(dayPrefs);
    }
    public int GetDayPrefs() {
        return PlayerPrefs.GetInt(keyDay, 1);
    }
    int GetDayOfYearPrefs() {
        return PlayerPrefs.GetInt(keyDayOfYear, DateTime.Now.DayOfYear);
    }
    void CompareTime() {
        int dayOfYearNow = DateTime.Now.DayOfYear;
        if (dayOfYearNow != dayOfYearPrefs && IsCollectedDay()) {
           
            IncreaseDayPrefs();
        }
    }
    public void CollectNow() {
        ProfileManager.Instance.dataConfig.dailyRewardConfig.CollectDay(dayPrefs);
        ClaimReward(dayPrefs);
    }
    public bool IsCollectedDay() {
        return ProfileManager.Instance.dataConfig.dailyRewardConfig.IsCollectedDay(dayPrefs);
    }
    public void ClaimReward(int index)
    {
        List<ItemReward> rewards = new List<ItemReward>();
        ItemReward reward = ProfileManager.Instance.dataConfig.dailyRewardConfig.rewards[index - 1].reward;
        rewards.Add(reward);
        switch (reward.type)
        {
            case ItemType.Gem:
                ProfileManager.Instance.playerData.AddGem(reward.amount);
                break;
            case ItemType.FreeChest:
                ProfileManager.Instance.playerData.boxManager.OpenBox(ItemType.FreeChest);
                break;
            case ItemType.NormalChest:
                ProfileManager.Instance.playerData.boxManager.OpenBox(ItemType.NormalChest);
                break;
            case ItemType.AdvancedChest:
                ProfileManager.Instance.playerData.boxManager.OpenBox(ItemType.AdvancedChest);
                break;
            case ItemType.Card:
                break;
            case ItemType.Cash:
                ProfileManager.PlayerData.AddCash(reward.amount);
                break;
            case ItemType.PremiumSuit:
                break;
            case ItemType.GodenSuit:
                break;
            case ItemType.OfflineTime:
                break;
            case ItemType.IncreaseProfit:
                break;
            case ItemType.RemoveAds:
                break;
            case ItemType.TimeSkip_1H:
                ProfileManager.PlayerData.ResourceSave.AddTimeSkipTicket_1H(reward.amount);
                break;
            case ItemType.TimeSkip_4H:
                break;
            case ItemType.TimeSkip_24H:
                break;
            case ItemType.ADTicket:
                break;
           
            default:
                break;
        }
        UIManager.instance.ShowUIPanelReward(rewards);
    }
}
