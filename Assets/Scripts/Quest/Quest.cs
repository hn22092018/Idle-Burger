using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest {
    public int questID;
    public QuestType type;
    public string roomTarget;
    public int priority;
    public int level;
    public ItemReward reward;
    public string des;
    public int DesLocalizeID;

    public void GoToHelperUI() {
        QuestHelperUI.instance.OnGoQuest(this);
    }
    public string GetDes() {
        if (!string.IsNullOrEmpty(ProfileManager.Instance.dataConfig.GameText.GetTextByID(DesLocalizeID))) {
            string des = ProfileManager.Instance.dataConfig.GameText.GetTextByID(DesLocalizeID);
            des = string.Format(des, level);
            return des;
        }
        des = string.Format(des, level);
        return des;
    }
}
