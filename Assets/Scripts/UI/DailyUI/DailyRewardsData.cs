using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DailyRewards", menuName = "ScriptableObjects/New DailyRewards")]
public class DailyRewardsData : ScriptableObject {
    public List<DailyRewards> rewards;
    public void GetDailyReward(int day) { }
    public void Load() {
        for (int i = 0; i < rewards.Count; i++) {
            rewards[i].Load();
        }
    }
    public void Reset() {
        for (int i = 0; i < rewards.Count; i++) {
            rewards[i].Reset();
        }
    }
    public void CollectDay(int day) {
        rewards[day - 1].OnCollect();
    }
    public bool IsCollectedDay(int day) {
        return rewards[day - 1].collected;
    }
}
