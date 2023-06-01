using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest {
    [ReadOnly]
    public int questID;
    [ReadOnly]
    public QuestType type;
    [ReadOnly]
    public RoomID roomTarget;
    [ReadOnly]
    public int priority;
    [ReadOnly]
    public int level;
    [ReadOnly]
    public ItemReward reward;
    [HideInInspector]
    public string des;
    public void GoToHelperUI() {
        QuestHelperUI.instance.OnGoQuest(this);
    }
    public string GetDes() {
        des = "Upgrade" + " " + ProfileManager.Instance.dataConfig.GameText.QuestTypeToShortString(type) + " " + "in" + " " + ProfileManager.Instance.dataConfig.GameText.RoomIDToString(roomTarget) + " " + "to level" + " " + level;
        return des;
    }
}
