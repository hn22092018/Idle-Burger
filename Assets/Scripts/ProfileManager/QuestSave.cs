using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class QuestSave {
    public QuestManager questManager_a = new QuestManager();
    public QuestManager questManager_2 = new QuestManager();
    public QuestManager questManager_3 = new QuestManager();
    bool IsChangeData;
    public void LoadData() {
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData)) {
            QuestSave dataSave = JsonUtility.FromJson<QuestSave>(jsonData);
            questManager_a = dataSave.questManager_a;
            questManager_2 = dataSave.questManager_2;
            questManager_3 = dataSave.questManager_3;
        }
        questManager_a.InitQuest(ProfileManager.Instance.dataConfig.QuestDataConfig_W1.questList);
        questManager_2.InitQuest(ProfileManager.Instance.dataConfig.QuestDataConfig_W2.questList);
        questManager_3.InitQuest(ProfileManager.Instance.dataConfig.QuestDataConfig_W3.questList);
    }
    public void IsMarkChangeData() {
        IsChangeData = true;
    }
    public void SaveData() {
        if (!IsChangeData) return;
        IsChangeData = false;
        PlayerPrefs.SetString("QuestSave", JsonUtility.ToJson(this).ToString());
    }
    string GetJsonData() {
        return PlayerPrefs.GetString("QuestSave");
    }
    public QuestManager GetQuestManager(int world) {
        switch (world) {
            case 1:
                return questManager_a;
            case 2:
                return questManager_2;
            case 3:
                return questManager_3;
        }
        return questManager_a;
    }
}
