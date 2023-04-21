using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MarketingManager {
    public bool IsPassTutorial;
    public bool IsVIPActive;
    public float remainBoostSeconds;
    public string timeOnlineLast;
    bool AdsCampainBooting = false;
    float minuteAddValue = 10;
    private float hourMax = 2f;
    private float defaultSpawnTime = 4.5f;
    private int AdsSpawnIncreaseRate = 50;
    private int AdsVIPIncreaseRate = 25;
    private int IAPSpawnIncreaseRate = 35;
    private int IAPVIPIncreaseRate = 25;
    bool IsChangeData;
    public void LoadData() {
        Debug.Log("MarketingManager LoadData");
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData)) {
            MarketingManager dataSave = JsonUtility.FromJson<MarketingManager>(jsonData);
            IsPassTutorial = dataSave.IsPassTutorial;
            IsVIPActive = dataSave.IsVIPActive;
            remainBoostSeconds = dataSave.remainBoostSeconds;
            timeOnlineLast = dataSave.timeOnlineLast;
        }
        if (!string.IsNullOrEmpty(timeOnlineLast)) {
            TimeSpan diffTime = DateTime.Now - DateTime.Parse(timeOnlineLast);
            if (remainBoostSeconds > 0) IsChangeData = true;
            remainBoostSeconds -= (float)diffTime.TotalSeconds;
            if (remainBoostSeconds > 0) {
                AdsCampainBooting = true;
            } else {
                AdsCampainBooting = false;
                remainBoostSeconds = 0f;
            }

        } else {
            AdsCampainBooting = false;
            remainBoostSeconds = 0f;
        }
    }
    public void SaveData() {
        if (!IsChangeData) return;
        IsChangeData = false;
        PlayerPrefs.SetString("MarketingManager", JsonUtility.ToJson(this).ToString());
    }
    string GetJsonData() {
        return PlayerPrefs.GetString("MarketingManager");
    }
    /// <summary>
    /// internet Campaint is active so dont need to calculate time at all
    /// </summary>
    public void Update() {
        timeOnlineLast = DateTime.Now.ToString();
        if (remainBoostSeconds > 0) {
            IsChangeData = true;
            if (Time.timeScale > 0) remainBoostSeconds -= Time.deltaTime / Time.timeScale;
            if (remainBoostSeconds <= 0) {
                AdsCampainBooting = false;
                remainBoostSeconds = 0f;
            }
        }
    }
    public float GetCustormerSpawnTime() {
        float rate = 1;
        if (AdsCampainBooting) rate -= 0.5f;
        if (IsVIPActive) rate -= 0.4f;
        if (rate <= 0.1f) rate = 0.1f;
        return defaultSpawnTime * rate;
    }
    public int GetSpawnVIPRate() {
        if (AdsCampainBooting && IsVIPActive) {
            return AdsVIPIncreaseRate + IAPVIPIncreaseRate;
        } else if (IsVIPActive) {
            return IAPVIPIncreaseRate;
        } else if (AdsCampainBooting) {
            return AdsVIPIncreaseRate;
        }
        return 5;
    }

    //neu thoi gian cua Cinema campaign <= 1h30p thi cho nguoi choi xem video
    public bool CheckAbleWatchVideo() {
        return remainBoostSeconds <= (hourMax * 3600f - 10f * 60f);
    }
    public string RemainTimeToString() {
        TimeSpan timeSpan = TimeSpan.FromSeconds(remainBoostSeconds);
        return string.Format("{0:D1}h {1:D2}m {2:D2}s", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }
    public void AddAdsCampaignTime() {
        remainBoostSeconds += minuteAddValue * 60f;
        AdsCampainBooting = true;
        if (remainBoostSeconds >= hourMax * 3600) remainBoostSeconds = hourMax * 3600;
    }
    public void AddMoreTimeByTutorial() {
        IsPassTutorial = true;
        remainBoostSeconds += 120;
        AdsCampainBooting = true;
        if (remainBoostSeconds >= hourMax * 3600) remainBoostSeconds = hourMax * 3600;
    }
    public bool IsBoostingAdsActive() {
        return remainBoostSeconds > 0;
    }
    public bool IsFullBoost() {
        return remainBoostSeconds >= hourMax * 3600 - 5 * 60f;
    }
    public int GetAdsSpawnIncreaseRate() {
        return AdsSpawnIncreaseRate;
    }
    public int GetAdsVIPIncreaseRate() {
        return AdsVIPIncreaseRate;
    }
    public int GetIAPSpawnIncreaseRate() {
        return IAPSpawnIncreaseRate;
    }
    public int GetIAPVIPIncreaseRate() {
        return IAPVIPIncreaseRate;
    }
    public string RemainTimeToString2() {
        TimeSpan timeSpan = TimeSpan.FromSeconds(remainBoostSeconds);
        if (timeSpan.Hours >= 1) {
            return string.Format("{0:D1}h {1:D2}m", timeSpan.Hours, timeSpan.Minutes);
        }
        return string.Format("{0:D1}m {1:D2}s", timeSpan.Minutes, timeSpan.Seconds);
    }
}
