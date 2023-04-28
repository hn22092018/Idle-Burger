using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestroomController : RoomController<RestroomModelType> {
    QuestManager questManager;
    public override void TriggerQuestUpgrade(int indexItem) {
        base.TriggerQuestUpgrade(indexItem);
        if (questManager == null) questManager = ProfileManager.PlayerData.GetQuestManager();
        RestroomModelType type = roomSetting.modelPositions[indexItem].type;
        int level = roomSetting.modelPositions[indexItem].level;
        switch (type) {
            case RestroomModelType.Restroom_SinkFemale:
                for (int i = 1; i <= level; i++) {
                    questManager.TriggerQuest(QuestType.Upgrade_Restroom_FemaleSink, i);
                }
                break;
            case RestroomModelType.Restroom_SinkMale:
                for (int i = 1; i <= level; i++) {
                    questManager.TriggerQuest(QuestType.Upgrade_Restroom_MaleSink, i);
                }
                break;
            case RestroomModelType.Restroom_StallFemale:
                for (int i = 1; i <= level; i++) {
                    questManager.TriggerQuest(QuestType.Upgrade_Restroom_FemaleStall, i);
                }
                break;
            case RestroomModelType.Restroom_StallMale:
                for (int i = 1; i <= level; i++) {
                    questManager.TriggerQuest(QuestType.Upgrade_Restroom_MaleStall, i);
                }
                break;

        }
    }
    public override int GetFirstItemIndexByQuest(Quest quest) {
        int index = 0;
        switch (quest.type) {
            case QuestType.Upgrade_Restroom_MaleStall:
                index = GetFirstItemIndexByType(RestroomModelType.Restroom_StallMale.ToString());
                break;
            case QuestType.Upgrade_Restroom_FemaleStall:
                index = GetFirstItemIndexByType(RestroomModelType.Restroom_StallFemale.ToString());
                break;
            case QuestType.Upgrade_Restroom_MaleSink:
                index = GetFirstItemIndexByType(RestroomModelType.Restroom_SinkMale.ToString());
                break;
            case QuestType.Upgrade_Restroom_FemaleSink:
                index = GetFirstItemIndexByType(RestroomModelType.Restroom_SinkFemale.ToString());
                break;
        }
        return index;
    }
}