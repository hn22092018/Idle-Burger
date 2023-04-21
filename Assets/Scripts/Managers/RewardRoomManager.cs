using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RewardRoomType {
    Stage_1,
    Stage_2
}
[System.Serializable]
public class RewardRoom {
    public RoomID roomID;
    public RewardRoomType type;
    public int world;
}
[System.Serializable]
public class RewardRoomManager {
    public List<RewardRoom> _RewardRooms;
    public void LoadData() {
        Debug.Log("RewardRoomManager LoadData");
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData)) {
            RewardRoomManager dataSave = JsonUtility.FromJson<RewardRoomManager>(jsonData);
            _RewardRooms = dataSave._RewardRooms;
        }
    }
    public bool IsCollectReward(RoomID roomID, RewardRoomType type) {
        int world = ProfileManager.PlayerData.selectedWorld;
        return IsContainRewardRoom(roomID, type, world);
    }
    public void CollectReward(RoomID roomID, RewardRoomType type) {
        int world = ProfileManager.PlayerData.selectedWorld;
        if (!IsContainRewardRoom(roomID, type, world)) {
            _RewardRooms.Add(new RewardRoom() {
                roomID = roomID,
                type = type,
                world = world
            });
            SaveData();
        }
    }
    bool IsContainRewardRoom(RoomID roomID, RewardRoomType type, int world) {
        for (int i = 0; i < _RewardRooms.Count; i++) {
            if (_RewardRooms[i].roomID == roomID && _RewardRooms[i].type == type && _RewardRooms[i].world == world) return true;
        }
        return false;
    }
    public void SaveData() {
        PlayerPrefs.SetString("RewardRoomManager", JsonUtility.ToJson(this).ToString());
    }
    string GetJsonData() {
        return PlayerPrefs.GetString("RewardRoomManager");
    }
}
