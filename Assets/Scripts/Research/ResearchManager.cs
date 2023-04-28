using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDK;
using System.Linq;
using System;

[System.Serializable]
public class ResearchSave {
    public ResearchName researchName;
    public int level;
}
[System.Serializable]
public class CurrentResearch {
    public ResearchName researchName;
    public ResearchType researchDepend;
    public string timeEnd;
}
[System.Serializable]
public class ResearchManager {
    public List<ResearchSave> researchSave = new();
    public List<CurrentResearch> currentResearchs= new List<CurrentResearch>();
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

    public float GetTimeCoolDown(ResearchName researchName) {
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
        if (!researchSave.Exists(x => x.researchName == ResearchName.Hamburger))
            researchSave.Add(new ResearchSave() {
                researchName = ResearchName.Hamburger,
                level = 1
            });
    }
    
    string GetJsonData() { return PlayerPrefs.GetString("ResearchManager"); }
    public void SaveData() { PlayerPrefs.SetString("ResearchManager", JsonUtility.ToJson(this).ToString()); }
    public void Research(Research researchData) {
        CurrentResearch newCurrentResearch = new CurrentResearch() {
            researchName = researchData.researchName,
            timeEnd = DateTime.Now.AddSeconds(researchData.timeBlock).ToString(),
            researchDepend = researchData.researchType
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
        ReloadListCache(currentResearch);
        SaveData();
    }
    public void UpgradeResearch(ResearchName researchName) {
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
    //List<Research> avaiables_food_japan_unlock;
    //List<Research> avaiables_food_japan;
    //List<Research> avaiables_food_mexico_unlock;
    //List<Research> avaiables_food_mexico;
    //List<Research> avaiables_drink_bar;
    //List<Research> avaiables_drink_bar_unlock;
    //List<Research> avaiables_drink_coffee;
    //List<Research> avaiables_drink_coffee_unlock;
    /// <summary>
    ///  Load all cache  list for Function Get Random Food , Drink
    /// </summary>
    void ReloadListCache() {
        List<Research> avaiables = ProfileManager.Instance.dataConfig.researchDataConfig.avaiables_food_default;
        avaiables_food_default_unlock = avaiables.Where(x => GetLevelByName(x.researchName) > 0).ToList();
        avaiables_food_default = avaiables;

        //List<Research> avaiables_1 = ProfileManager.Instance.dataConfig.researchDataConfig.avaiables_food_mexico;
        //avaiables_food_mexico_unlock = avaiables_1.Where(x => GetLevelByName(x.researchName) > 0).ToList();
        //avaiables_food_mexico = avaiables_1;

        //List<Research> avaiables_2 = ProfileManager.Instance.dataConfig.researchDataConfig.avaiables_food_japan;
        //avaiables_food_japan_unlock = avaiables_2.Where(x => GetLevelByName(x.researchName) > 0).ToList();
        //avaiables_food_japan = avaiables_2;

        //avaiables_drink_bar = ProfileManager.Instance.dataConfig.researchDataConfig.GetGroupResearch(GroupResearchName.Drink).Where(x => x.researchType == ResearchType.DrinkBar).ToList();
        //avaiables_drink_bar_unlock = avaiables_drink_bar.Where(x => GetLevelByName(x.researchName) > 0).ToList();

        //avaiables_drink_coffee = ProfileManager.Instance.dataConfig.researchDataConfig.GetGroupResearch(GroupResearchName.Drink).Where(x => x.researchType == ResearchType.DrinkCoffee).ToList();
        //avaiables_drink_coffee_unlock = avaiables_drink_coffee.Where(x => GetLevelByName(x.researchName) > 0).ToList();
    }

    void ReloadListCache(CurrentResearch currentResearch) {
        switch (currentResearch.researchDepend) {
            case ResearchType.DefaultFood:
                List<Research> avaiables = ProfileManager.Instance.dataConfig.researchDataConfig.avaiables_food_default;
                avaiables_food_default_unlock = avaiables.Where(x => GetLevelByName(x.researchName) > 0).ToList();
                break;
            //case ResearchType.MexicoFood:
            //    List<Research> avaiables_1 = ProfileManager.Instance.dataConfig.researchDataConfig.avaiables_food_mexico;
            //    avaiables_food_mexico_unlock = avaiables_1.Where(x => GetLevelByName(x.researchName) > 0).ToList();
            //    break;
            //case ResearchType.JapanFood:
            //    List<Research> avaiables_2 = ProfileManager.Instance.dataConfig.researchDataConfig.avaiables_food_japan;
            //    avaiables_food_japan_unlock = avaiables_2.Where(x => GetLevelByName(x.researchName) > 0).ToList();
            //    break;
            //case ResearchType.DrinkBar:
            //    avaiables_drink_bar_unlock = avaiables_drink_bar.Where(x => GetLevelByName(x.researchName) > 0).ToList();
            //    break;
            //case ResearchType.DrinkCoffee:
            //    avaiables_drink_coffee_unlock = avaiables_drink_coffee.Where(x => GetLevelByName(x.researchName) > 0).ToList();
            //    break;
        }
    }

    public ResearchSave GetResearchInSaver(ResearchName researchName) {
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
    public int GetLevelByName(ResearchName researchName) {
        ResearchSave researchSave = GetResearchInSaver(researchName);
        if (researchSave != null)
            return researchSave.level;
        return 0;
    }
    public bool CheckCurrentResearch(ResearchName researchName) {
        if (!onResearch)
            return false;
        for (int i = 0; i < currentResearchs.Count; i++) {
            if (researchName == currentResearchs[i].researchName)
                return true;
        }
        return false;
    }
    ResearchName researchOnShowDetail;
    public void SkipNow(ResearchName researchName) {
        for (int i = 0; i < currentResearchs.Count; i++) {
            if (currentResearchs[i].researchName == researchName)
                currentResearchs[i].timeEnd = DateTime.Now.ToString();
        }
        SaveData();
    }
    public void SkipByWatchVideo(ResearchName researchName) {
        researchOnShowDetail = researchName;
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds)
            AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.SkipResearch.ToString(), SkipFifteenMinutes);
        else SkipFifteenMinutes();
    }
    public void SkipByTicket(ResearchName researchName) {
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
 
    public bool IsMaxLevel(ResearchName researchName) {
        int levelSave = GetLevelByName(researchName);
        return levelSave >= 10;
    }
    public bool IsUnlockResearch(ResearchName researchName) {
        return true;
    }
    public bool IsEnoughResearchValue(ResearchName researchName) {
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
    public void AddResearchValue(int value=1) {
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
        int level = ProfileManager.PlayerData.selectedWorld;
        switch (level) {
            case 1: // Map 1
                /// 80% call food have unlocked, or all food default unlocked
                if ((UnityEngine.Random.Range(0, 100) >= 20 && avaiables_food_default_unlock.Count > 0) || avaiables_food_default.Count == 0) {
                    return avaiables_food_default_unlock[UnityEngine.Random.Range(0, avaiables_food_default_unlock.Count)];
                } else {
                    return avaiables_food_default[UnityEngine.Random.Range(0, avaiables_food_default.Count)];
                }
            //case 2: // Japan - Map2
            //    /// 70% call japan food, 30% call default food
            //    if (UnityEngine.Random.Range(0, 100) >= 30) {
            //        /// 80% call food have unlocked, or all food default unlocked
            //        if ((UnityEngine.Random.Range(0, 100) >= 20 && avaiables_food_japan_unlock.Count > 0) || avaiables_food_japan.Count == 0) {
            //            return avaiables_food_japan_unlock[UnityEngine.Random.Range(0, avaiables_food_japan_unlock.Count)];
            //        } else {
            //            return avaiables_food_japan[UnityEngine.Random.Range(0, avaiables_food_japan.Count)];
            //        }
            //    } else {
            //        return ProfileManager.Instance.dataConfig.researchDataConfig.avaiables_food_default[UnityEngine.Random.Range(0, ProfileManager.Instance.dataConfig.researchDataConfig.avaiables_food_default.Count)];
            //    }
            //case 3:// Mexico - Map3
            //    /// 70% call mexico food, 30% call default food
            //    if (UnityEngine.Random.Range(0, 100) >= 30) {
            //        /// 80% call food have unlocked, or all food default unlocked
            //        if ((UnityEngine.Random.Range(0, 100) >= 20 && avaiables_food_mexico_unlock.Count > 0) || avaiables_food_mexico.Count == 0) {
            //            return avaiables_food_mexico_unlock[UnityEngine.Random.Range(0, avaiables_food_mexico_unlock.Count)];
            //        } else {
            //            return avaiables_food_mexico[UnityEngine.Random.Range(0, avaiables_food_mexico.Count)];
            //        }
            //    } else {
            //        return ProfileManager.Instance.dataConfig.researchDataConfig.avaiables_food_default[UnityEngine.Random.Range(0, ProfileManager.Instance.dataConfig.researchDataConfig.avaiables_food_default.Count)];
            //    }
            default:
                break;
        }
        return null;
    }

    //public Research GetRandomDrink_Bar() {
    //    /// 80% call food have unlocked, or all drink bar default unlocked
    //    if (UnityEngine.Random.Range(0, 100) >= 20 && avaiables_drink_bar_unlock.Count > 0) {
    //        return avaiables_drink_bar_unlock[UnityEngine.Random.Range(0, avaiables_drink_bar_unlock.Count)];
    //    } else {
    //        return avaiables_drink_bar[UnityEngine.Random.Range(0, avaiables_drink_bar.Count)];
    //    }
    //}
    //public Research GetRandomDrink_Coffee() {
    //    /// 80% call food have unlocked, or all drink bar default unlocked
    //    if (UnityEngine.Random.Range(0, 100) >= 20 && avaiables_drink_coffee_unlock.Count > 0) {
    //        return avaiables_drink_coffee_unlock[UnityEngine.Random.Range(0, avaiables_drink_coffee_unlock.Count)];
    //    } else {
    //        return avaiables_drink_coffee[UnityEngine.Random.Range(0, avaiables_drink_coffee.Count)];
    //    }
    //}

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

