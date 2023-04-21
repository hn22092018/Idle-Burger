using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DailyRewards {
    public int day;
    public bool collected;
    public ItemReward reward;
    public void Load() {
        collected = PlayerPrefs.GetInt(DailyRewardsManager.strCollectedDay + day) == 1;
    }
    public void Reset() {
        collected = false;
        PlayerPrefs.SetInt(DailyRewardsManager.strCollectedDay + day, 0);
    }
    public void OnCollect() {
        collected = true;
        PlayerPrefs.SetInt(DailyRewardsManager.strCollectedDay + day, 1);
    }
}
