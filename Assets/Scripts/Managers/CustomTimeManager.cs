using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CustomTimeManager {
    public string trueDateOff;
    int offlineTimeMax;
    int offlineTime;
    public int DayRealPlayed;
    public void LoadData() {
        Debug.Log("CustomTimeManager LoadData");
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData)) {
            CustomTimeManager dataSave = JsonUtility.FromJson<CustomTimeManager>(jsonData);
            trueDateOff = dataSave.trueDateOff;
            DayRealPlayed = dataSave.DayRealPlayed;
        }
        DateTime now = DateTime.Now;
        if (string.IsNullOrEmpty(trueDateOff)) {
            // Init Data
            trueDateOff = DateTime.Now.ToString();
        }
        offlineTime = (int)(now.Subtract(DateTime.Parse(trueDateOff)).TotalSeconds);
        if (DayRealPlayed == 0) DayRealPlayed = 1;
    }

    public void SaveData() {
        if (Tutorials.instance == null || (Tutorials.instance != null && Tutorials.instance.IsRunStory)) return;
        PlayerPrefs.SetString("CustomTimeManager", JsonUtility.ToJson(this).ToString());
    }
    string GetJsonData() {
        return PlayerPrefs.GetString("CustomTimeManager");
    }
    public void Update() {
        if (!string.IsNullOrEmpty(trueDateOff) && DateTime.Now.Date != DateTime.Parse(trueDateOff).Date) {
            ProfileManager.PlayerData.ResourceSave.ResetFreeGemAds();
            ProfileManager.PlayerData.researchManager.ResetFreeAdsCustomer();
            DayRealPlayed++;
        }
        trueDateOff = DateTime.Now.ToString();
    }

    /// <summary>
    /// return offline time (second)
    /// </summary>
    /// <returns></returns>
    /// 
    public int GetOfflineTime() {
        int hourOfflineMax = ProfileManager.PlayerData.GetCardManager().GetExtraHour_OfflineTimeCardIAP();
        offlineTimeMax = hourOfflineMax * 3600;
        return Mathf.Clamp(offlineTime, 0, offlineTimeMax);
    }
    public int GetOfflineTimeExactly() {
        return offlineTime;
    }
    public string FormatOfflineTimeToString() {
        TimeSpan timeSpan = DateTime.Now.Subtract(DateTime.Parse(trueDateOff));
        return string.Format("{0:D1}d {1:D2}h {2:D2}m", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes);
    }
}
