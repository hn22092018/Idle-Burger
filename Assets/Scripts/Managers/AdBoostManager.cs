using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class AdBoostManager {
    public bool IsPassTutorial;
    public float remainBoostSeconds;
    public string timeOnlineLast;
    float minuteAddValue = 10f;
    private float hourMax = 2f;
    bool IsChangeData;
    public void LoadData() {
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData)) {
            AdBoostManager dataSave = JsonUtility.FromJson<AdBoostManager>(jsonData);
            remainBoostSeconds = dataSave.remainBoostSeconds;
            timeOnlineLast = dataSave.timeOnlineLast;
            IsPassTutorial = dataSave.IsPassTutorial;
        }
        if (!string.IsNullOrEmpty(timeOnlineLast)) {
            TimeSpan diffTime = DateTime.Now - DateTime.Parse(timeOnlineLast);
            if (remainBoostSeconds > 0 ) IsChangeData = true;
            remainBoostSeconds -= (float)diffTime.TotalSeconds;
        } else {
            remainBoostSeconds = 0f;
        }
    }
    public void SaveData() {
        if (!IsChangeData) return;
        IsChangeData = false;
        PlayerPrefs.SetString("AdBoostManager", JsonUtility.ToJson(this).ToString());
    }
    string GetJsonData() {
        return PlayerPrefs.GetString("AdBoostManager");
    }
    public void AddMoreTime() {
        // Add more ads boost time
        remainBoostSeconds += minuteAddValue * 60f;
        if (remainBoostSeconds >= hourMax * 3600) remainBoostSeconds = hourMax * 3600;
        IsChangeData = true;
        SaveData();
    }
    public void AddMoreTimeTutorial() {
        IsPassTutorial = true;
        // Add more ads boost time
        remainBoostSeconds += 120;
        if (remainBoostSeconds >= hourMax * 3600) remainBoostSeconds = hourMax * 3600;
    }
  
    public void Update() {
        timeOnlineLast = DateTime.Now.ToString();
        if (remainBoostSeconds > 0) {
            IsChangeData = true;
            remainBoostSeconds -= Time.unscaledDeltaTime;
        } else remainBoostSeconds = 0;
    }
  
    public string FinanceRemainTimeToString() {
        TimeSpan timeSpan = TimeSpan.FromSeconds(remainBoostSeconds);
        if (timeSpan.Hours > 0) return string.Format("{0:D1}h {1:D2}m", timeSpan.Hours, timeSpan.Minutes);
        return string.Format("{0:D2}m {1:D2}s", timeSpan.Minutes, timeSpan.Seconds);
    }
    public string FinanceRemainTimeToString2() {
        return TimeUtil.RemainTimeToString(remainBoostSeconds);
    }
    public bool IsBoostFinanceActive() {
        return remainBoostSeconds > 0;
    }
    public bool IsFullBoostFinance() {
        return remainBoostSeconds >= hourMax * 3600 - 2 * 60f;
    }
}
