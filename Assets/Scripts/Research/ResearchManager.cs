using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDK;
using System.Linq;
using System;

[System.Serializable]
public class ResearchSave {
    public ResearchType researchName;
    public int level;
}
[System.Serializable]
public class CurrentResearch {
    public ResearchType researchName;
    public string timeEnd;
}
[System.Serializable]
public class ResearchManager {
    public List<ResearchSave> researchSave = new();
    public List<CurrentResearch> currentResearchs = new List<CurrentResearch>();
    public int researchValue;
    public int maxResearchCount = 1;
    public bool onResearch;
    TimeSpan timeRemaining;
    public int Free_Customer_Ads_Watched;
    public int Free_Customer_NonAds;
    /// <summary>
    /// Is Bought Researcher Pack
    /// </summary>
    public bool IsBoughtResearchPack;
    public void Update() {
        if (onResearch) {
            for (int i = currentResearchs.Count - 1; i >= 0; i--) {
                timeRemaining = Convert.ToDateTime(currentResearchs[i].timeEnd).Subtract(DateTime.Now);
                if (timeRemaining.TotalSeconds <= 0)
                    ClaimResearch(currentResearchs[i]);
            }
        }
        onResearch = currentResearchs.Count > 0;
    }
    public void ResetFreeAdsCustomer() {
        Free_Customer_Ads_Watched = 0;
        Free_Customer_NonAds = 1;
    }

    public float GetTimeCoolDown(ResearchType researchName) {
        for (int i = 0; i < currentResearchs.Count; i++) {
            if (currentResearchs[i].researchName == researchName)
                return (float)Convert.ToDateTime(currentResearchs[i].timeEnd).Subtract(DateTime.Now).TotalSeconds;
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
            onResearch = currentResearchs.Count > 0;
            Free_Customer_Ads_Watched = data.Free_Customer_Ads_Watched;
            Free_Customer_NonAds = data.Free_Customer_NonAds;
            IsBoughtResearchPack = data.IsBoughtResearchPack;
        } else {
            Free_Customer_Ads_Watched = 0;
            Free_Customer_NonAds = 1;
        }
        LoadMaxResearchCount();
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

    string GetJsonData() { return PlayerPrefs.GetString("ResearchManager"); }
    public void SaveData() { PlayerPrefs.SetString("ResearchManager", JsonUtility.ToJson(this).ToString()); }
    public void Research(Research researchData) {
        CurrentResearch newCurrentResearch = new CurrentResearch() {
            researchName = researchData.researchType,
            timeEnd = DateTime.Now.AddSeconds(researchData.foodBlockTime).ToString(),
        };
        currentResearchs.Add(newCurrentResearch);
        ConsumeResearchValue(researchData.CalulateReseachPrice(0));
        SaveData();
    }
    public void ClaimResearch(CurrentResearch currentResearch) {
        if (currentResearchs == null)
            return;
        // remove current research 
        currentResearchs.Remove(currentResearch);
        // check saver has item with name == currentResearch Name
        ResearchSave researchSaveData = GetResearchInSaver(currentResearch.researchName);
        if (researchSaveData != null) researchSaveData.level++;
        else {
            researchSave.Add(new ResearchSave {
                researchName = currentResearch.researchName,
                level = 1
            });
        }
        ReloadListCache();
        SaveData();
    }
    public void UpgradeResearch(ResearchType researchName) {
        ResearchSave researchSaveData = GetResearchInSaver(researchName);
        ConsumeResearchValue(ProfileManager.Instance.dataConfig.researchDataConfig.GetResearch(researchName).CalulateReseachPrice(researchSaveData.level));
        researchSaveData.level++;
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
    public bool CheckCurrentResearch(ResearchType researchName) {
        if (!onResearch)
            return false;
        for (int i = 0; i < currentResearchs.Count; i++) {
            if (researchName == currentResearchs[i].researchName)
                return true;
        }
        return false;
    }
    ResearchType researchOnShowDetail;
    public void SkipNow(ResearchType researchName) {
        for (int i = 0; i < currentResearchs.Count; i++) {
            if (currentResearchs[i].researchName == researchName)
                currentResearchs[i].timeEnd = DateTime.Now.ToString();
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
                float timeCoolDown = (float)Convert.ToDateTime(currentResearchs[i].timeEnd).Subtract(DateTime.Now.AddMinutes(15)).TotalSeconds;
                currentResearchs[i].timeEnd = DateTime.Now.AddSeconds(timeCoolDown).ToString();
            }
        }
        ProfileManager.Instance.playerData.SaveData();
    }

    public bool IsMaxLevel(ResearchType researchName) {
        int levelSave = GetLevelByName(researchName);
        return levelSave >= 10;
    }
    public bool IsUnlockResearch(ResearchType researchName) {
        return true;
    }
    public bool IsEnoughResearchValue(ResearchType researchName) {
        if (IsMaxLevel(researchName))
            return false;
        Research research = ProfileManager.Instance.dataConfig.researchDataConfig.GetResearch(researchName);
        int levelSave = GetLevelByName(researchName);
        int requireValue = research.CalulateReseachPrice(levelSave);
        return GameManager.instance.IsEnoughResearchValue(requireValue);
    }
    public bool IsMaxResearcherWorking() { return currentResearchs.Count >= maxResearchCount; }
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
    /// <summary>
    ///  Add Research Value When Customer Accept In Res
    ///  If Is Bought Researcher Pack 1 => IBRP1=true, // increase rate 30%
    ///  If Is Bought Researcher Pack 2 => IBRP2=true, // increase rate 30%
    /// </summary>
    /// <param name="value"></param>

    public void AddResearchCount(int value, int max) {
        maxResearchCount += value;
        maxResearchCount = Mathf.Clamp(maxResearchCount, 1, max);
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

    public bool IsBoughtResearcherPack() {
        return IsBoughtResearchPack;
    }

    public void OnBoughtResearcherPack() {
        Debug.Log("OnBoughtResearcherPack1");
        IsBoughtResearchPack = true;
        LoadMaxResearchCount();
        SaveData();

    }
    void LoadMaxResearchCount() {
        maxResearchCount = 1;
        if (IsBoughtResearchPack) maxResearchCount++;
    }
}

