using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public enum EventID {
    Christmas,
    ChinaTet
}
[System.Serializable]
public class EventData {
    public string eventName;
    public Sprite eventIcon;
    public int selectedWorld;
    public EventID eventID;
    public List<MissionTarget> missionStars = new List<MissionTarget>();
    public List<MissionTarget> missionCustomers = new List<MissionTarget>();
    public MissionTarget GetMissionByID(int id) {
        foreach (var m in missionStars) {
            if (m.missionID == id) return m;
        }
        foreach (var m in missionCustomers) {
            if (m.missionID == id) return m;
        }
        return null;
    }
    public void OnEnable() {
        for (int i = 0; i < missionCustomers.Count; i++) {
            missionCustomers[i].OnEnable();

        }
        for (int i = 0; i < missionStars.Count; i++) {
            missionStars[i].OnEnable();
        }
    }
}
[System.Serializable]
public class MissionTarget {
    public int missionID;
    public int checkPoint;
    public ItemReward reward;
    public void OnEnable() {
        reward.OnEnable();
    }
}
[CreateAssetMenu(fileName = "EventDataAsset", menuName = "ScriptableObjects/EventDataAsset", order = 1)]
public class EventDataAsset : ScriptableObject {
    public Sprite[] sprs;
    public List<EventData> Events;
    public EventData GetEventData(EventID eventID) {
        foreach (EventData eventData in Events) {
            if (eventData.eventID == eventID)
                return eventData;
        }
        return null;
    }
    private void OnValidate() {
        for (int i = 0; i < Events.Count; i++)
            Events[i].OnEnable();
    }
    [Button]
    void LoadRewardChristmas() {
        Events[0].missionCustomers.Clear();
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 1,
            checkPoint = 10,
            reward = new ItemReward() { type = ItemType.Gem, amount = 5, spr = GetSpriteByName("icon gem") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 2,
            checkPoint = 20,
            reward = new ItemReward() { type = ItemType.ADTicket, amount = 1, spr = GetSpriteByName("ticket_1") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 3,
            checkPoint = 50,
            reward = new ItemReward() { type = ItemType.Gem, amount = 20, spr = GetSpriteByName("icon gem") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 4,
            checkPoint = 100,
            reward = new ItemReward() { type = ItemType.NormalChest, amount = 1, spr = GetSpriteByName("chest_2") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 5,
            checkPoint = 150,
            reward = new ItemReward() { type = ItemType.Uniform, amount = 1,skinID=51, spr = GetSpriteByName("Skin_Christmas_Waiter") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 6,
            checkPoint = 200,
            reward = new ItemReward() { type = ItemType.Gem, amount = 25, spr = GetSpriteByName("icon gem") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 7,
            checkPoint = 280,
            reward = new ItemReward() { type = ItemType.TimeSkip_1H, amount = 1, spr = GetSpriteByName("timeTravel_1") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 8,
            checkPoint = 350,
            reward = new ItemReward() { type = ItemType.Gem, amount = 30, spr = GetSpriteByName("icon gem") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 9,
            checkPoint = 500,
            reward = new ItemReward() { type = ItemType.Uniform, amount = 1, skinID = 19, spr = GetSpriteByName("Skin_Christmas_Cleaner") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 10,
            checkPoint = 650,
            reward = new ItemReward() { type = ItemType.Gem, amount = 30, spr = GetSpriteByName("icon gem") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 11,
            checkPoint = 800,
            reward = new ItemReward() { type = ItemType.ADTicket, amount = 1, spr = GetSpriteByName("ticket_1") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 12,
            checkPoint = 1000,
            reward = new ItemReward() { type = ItemType.Gem, amount = 30, spr = GetSpriteByName("icon gem") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 13,
            checkPoint = 1200,
            reward = new ItemReward() { type = ItemType.Uniform, amount = 1, skinID = 14, spr = GetSpriteByName("Skin_Christmas_Manager") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 14,
            checkPoint = 1400,
            reward = new ItemReward() { type = ItemType.Gem, amount = 30, spr = GetSpriteByName("icon gem") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 15,
            checkPoint = 1600,
            reward = new ItemReward() { type = ItemType.ADTicket, amount = 1, spr = GetSpriteByName("ticket_1") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 16,
            checkPoint = 1800,
            reward = new ItemReward() { type = ItemType.NormalChest, amount = 2, spr = GetSpriteByName("chest_2") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 17,
            checkPoint = 2000,
            reward = new ItemReward() { type = ItemType.Gem, amount = 50, spr = GetSpriteByName("icon gem") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 18,
            checkPoint = 2300,
            reward = new ItemReward() { type = ItemType.Uniform, amount = 1, skinID = 52, spr = GetSpriteByName("Skin_Christmas_Chef") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 19,
            checkPoint = 2700,
            reward = new ItemReward() { type = ItemType.AdvancedChest, amount = 1, spr = GetSpriteByName("chest_1") }
        });
        Events[0].missionCustomers.Add(new MissionTarget() {
            missionID = 20,
            checkPoint = 3000,
            reward = new ItemReward() { type = ItemType.SpecialCharactor, amount = 1, eventStaffID = 0, spr = GetSpriteByName("Santa Claus") }
        });
    }
    public Sprite GetSpriteByName(string name) {
        foreach (Sprite spr in sprs) {
            if (spr.name == name) return spr;
        }
        return null;
    }
}
