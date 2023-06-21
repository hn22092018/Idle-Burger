using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDK;
using System.Linq;
using System;
[System.Serializable]
public enum EResearchSlotIndex {
    Slot1, Slot2, Slot3, Slot4, Slot5, Slot6
}
[System.Serializable]
public class ResearchSave {
    public ResearchType researchName;
    public int level;
}
[System.Serializable]
public class CurrentResearch {
    public EResearchSlotIndex slot;
    public ResearchType researchName;
    public float timeEnd;
}
[System.Serializable]
public class ResearchManager {
    public List<ResearchSave> researchSave = new();
    public List<CurrentResearch> currentResearchs = new List<CurrentResearch>();
    public List<EResearchSlotIndex> unlockSlots = new List<EResearchSlotIndex>();
    public int researchValue;
    TimeSpan timeRemaining;
    public int Free_Customer_Ads_Watched;
    public int Free_Customer_NonAds;
    /// <summary>
    /// Is Bought Researcher Pack
    /// </summary>

    public void Update() {
        for (int i = currentResearchs.Count - 1; i >= 0; i--) {
            if (currentResearchs[i].timeEnd > 0) currentResearchs[i].timeEnd -= Time.deltaTime;
            if (currentResearchs[i].timeEnd <= 0 && currentResearchs[i].researchName != ResearchType.None) {
                ClaimResearch(currentResearchs[i]);
            }
        }
    }
    public void ResetFreeAdsCustomer() {
        Free_Customer_Ads_Watched = 0;
        Free_Customer_NonAds = 1;
    }

    public float GetTimeEndResearch(ResearchType researchName) {
        for (int i = 0; i < currentResearchs.Count; i++) {
            if (currentResearchs[i].researchName == researchName)
                return currentResearchs[i].timeEnd;
        }
        return 0;
    }

    public void LoadData() {
        Debug.Log("Load Research data.");
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData)) {
            ResearchManager data = JsonUtility.FromJson<ResearchManager>(jsonData);
            researchSave = data.researchSave;
            currentResearchs = data.currentResearchs;
            researchValue = data.researchValue;
            Free_Customer_Ads_Watched = data.Free_Customer_Ads_Watched;
            Free_Customer_NonAds = data.Free_Customer_NonAds;
            unlockSlots = data.unlockSlots;
        } else {
            Free_Customer_Ads_Watched = 0;
            Free_Customer_NonAds = 1;
        }

        InitRearchManager();
        UnlockResearchSlot(EResearchSlotIndex.Slot1);
        // unlock default 
        UnlockDefaultResearch();
        ReloadListCache();
    }
    void UnlockDefaultResearch() {
        if (!researchSave.Exists(x => x.researchName == ResearchType.Hamburger))
            researchSave.Add(new ResearchSave() {
                researchName = ResearchType.Hamburger,
                level = 1
            });
    }

    string GetJsonData() {
        return PlayerPrefs.GetString("ResearchManager");
    }
    public void SaveData() {
        PlayerPrefs.SetString("ResearchManager", JsonUtility.ToJson(this).ToString());
    }
    public void Research(Research researchData) {
        for (int i = 0; i < currentResearchs.Count; i++) {
            if (currentResearchs[i].researchName == ResearchType.None && IsUnlockResearchSlot(currentResearchs[i].slot)) {
                currentResearchs[i].researchName = researchData.researchType;
                currentResearchs[i].timeEnd = researchData.GetResearchTime(GetLevelByName(researchData.researchType));
                break;
            }
        }
        ConsumeResearchValue(researchData.GetReseachPrice(0));
        SaveData();
    }
    public void ClaimResearch(CurrentResearch currentResearch) {
        if (currentResearchs == null)
            return;
        // check saver has item with name == currentResearch Name
        ResearchSave researchSaveData = GetResearchInSaver(currentResearch.researchName);
        if (researchSaveData != null) researchSaveData.level++;
        else {
            researchSave.Add(new ResearchSave {
                researchName = currentResearch.researchName,
                level = 1
            });
        }
        for (int i = 0; i < currentResearchs.Count; i++) {
            if (currentResearchs[i].researchName == currentResearch.researchName) {
                currentResearchs[i].researchName = ResearchType.None;
                currentResearchs[i].timeEnd = 0;
                break;
            }
        }
        ReloadListCache();
        SaveData();
    }

    /// <summary>
    /// cache list for Function Get Random Food , Drink
    /// </summary>
    List<Research> avaiables_food_default_unlock;
    List<Research> avaiables_food_default;

    /// <summary>
    ///  Load all cache  list for Function Get Random Food , Drink
    /// </summary>
    void ReloadListCache() {
        List<Research> avaiables = new List<Research>(ProfileManager.Instance.dataConfig.researchDataConfig.foodResearchs);
        avaiables_food_default_unlock = avaiables.Where(x => GetLevelByName(x.researchType) > 0).ToList();
        avaiables_food_default = avaiables;
    }

    public ResearchSave GetResearchInSaver(ResearchType researchName) {
        foreach (ResearchSave research in researchSave) {
            if (research.researchName == researchName)
                return research;
        }
        return null;
    }
    /// <summary>
    ///  Get Level Reseach By ResearchName
    /// </summary>
    /// <param name="researchName"></param>
    /// <returns></returns>
    public int GetLevelByName(ResearchType researchName) {
        ResearchSave researchSave = GetResearchInSaver(researchName);
        if (researchSave != null)
            return researchSave.level;
        return 0;
    }
    public bool IsResearching(ResearchType researchName) {
        for (int i = 0; i < currentResearchs.Count; i++) {
            if (researchName == currentResearchs[i].researchName && currentResearchs[i].timeEnd > 0)
                return true;
        }
        return false;
    }
    public ResearchType CheckCurrentResearchOnSlot(EResearchSlotIndex slot) {

        for (int i = 0; i < currentResearchs.Count; i++) {
            if (slot == currentResearchs[i].slot)
                return currentResearchs[i].researchName;
        }
        return ResearchType.None;
    }

    ResearchType researchOnShowDetail;
    public void SkipNow(ResearchType researchName) {
        for (int i = 0; i < currentResearchs.Count; i++) {
            if (currentResearchs[i].researchName == researchName) {
                currentResearchs[i].timeEnd = 0;
                ClaimResearch(currentResearchs[i]);
            }
        }
        SaveData();
    }
    public void SkipByWatchVideo(ResearchType researchName) {
        researchOnShowDetail = researchName;
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds)
            AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.SkipResearch.ToString(), SkipFifteenMinutes);
        else SkipFifteenMinutes();
    }
    public void SkipByTicket(ResearchType researchName) {
        researchOnShowDetail = researchName;
        SkipFifteenMinutes();
    }
    public void SkipFifteenMinutes() {
        for (int i = 0; i < currentResearchs.Count; i++) {
            if (currentResearchs[i].researchName == researchOnShowDetail) {
                currentResearchs[i].timeEnd -= 15 * 60;
            }
        }
        SaveData();
    }

    public bool IsMaxLevel(ResearchType researchName) {
        int levelSave = GetLevelByName(researchName);
        return levelSave >= 10;
    }

    public bool IsEnoughResearchValue(ResearchType researchName) {
        if (IsMaxLevel(researchName))
            return false;
        Research research = ProfileManager.Instance.dataConfig.researchDataConfig.GetResearch(researchName);
        int levelSave = GetLevelByName(researchName);
        int requireValue = research.GetReseachPrice(levelSave);
        return GameManager.instance.IsEnoughResearchValue(requireValue);
    }

    public int GetResearchValue() { return researchValue; }
    public void ConsumeResearchValue(int value) {
        if (researchValue - value < 0)
            researchValue = 0;
        else
            researchValue -= value;
    }
    public void AddResearchValue(int value = 1) {
        researchValue += value;
    }

    public Research GetTutorialFood() {
        return avaiables_food_default_unlock[UnityEngine.Random.Range(0, avaiables_food_default_unlock.Count)];
    }
    public Research GetRandomFood() {
        /// 80% call food have unlocked, or all food default unlocked
        if ((UnityEngine.Random.Range(0, 100) >= 20 && avaiables_food_default_unlock.Count > 0) || avaiables_food_default.Count == 0) {
            return avaiables_food_default_unlock[UnityEngine.Random.Range(0, avaiables_food_default_unlock.Count)];
        } else {
            return avaiables_food_default[UnityEngine.Random.Range(0, avaiables_food_default.Count)];
        }
    }

    public bool IsHasFreeCustomerCanWatched() {
        if (Free_Customer_NonAds > 0 || Free_Customer_Ads_Watched < 5)
            return true;
        return false;
    }
    public bool IsHasFreeCustomerCanClaimed() {
        return Free_Customer_NonAds > 0;
    }

    public void OnBoughtResearcherPack() {
        UnlockResearchSlot(EResearchSlotIndex.Slot5);
        UnlockResearchSlot(EResearchSlotIndex.Slot6);
        SaveData();
    }

    public void InitRearchManager() {
        if (currentResearchs.Count == 0) {
            currentResearchs.Add(new CurrentResearch() { researchName = ResearchType.None, slot = EResearchSlotIndex.Slot1 });
            currentResearchs.Add(new CurrentResearch() { researchName = ResearchType.None, slot = EResearchSlotIndex.Slot2 });
            currentResearchs.Add(new CurrentResearch() { researchName = ResearchType.None, slot = EResearchSlotIndex.Slot3 });
            currentResearchs.Add(new CurrentResearch() { researchName = ResearchType.None, slot = EResearchSlotIndex.Slot4 });
            currentResearchs.Add(new CurrentResearch() { researchName = ResearchType.None, slot = EResearchSlotIndex.Slot5 });
            currentResearchs.Add(new CurrentResearch() { researchName = ResearchType.None, slot = EResearchSlotIndex.Slot6 });
        }
        SaveData();
    }
    public bool IsUnlockResearchSlot(EResearchSlotIndex index) {
        return unlockSlots.Contains(index);
    }
    public void UnlockResearchSlot(EResearchSlotIndex index) {
        if (!unlockSlots.Contains(index)) unlockSlots.Add(index);
        SaveData();
    }
    public bool IsAvaiableSlotToResearch() {
        for (int i = 0; i < currentResearchs.Count; i++) {
            if (currentResearchs[i].researchName == ResearchType.None && IsUnlockResearchSlot(currentResearchs[i].slot)) return true;
        }
        return false;
    }
}

