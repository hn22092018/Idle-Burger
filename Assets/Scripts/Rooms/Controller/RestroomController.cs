using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestroomController : RoomController<RestroomModelType> {
    QuestManager questManager;
    public override void TriggerQuestUpgrade(int indexItem) {
        base.TriggerQuestUpgrade(indexItem);
        int level = roomSetting.modelPositions[indexItem].level;
        if (level < 10 || (level > 10 && level % 25 != 0)) return;
        if (questManager == null) questManager = ProfileManager.PlayerData.GetQuestManager();
        RestroomModelType type = roomSetting.modelPositions[indexItem].type;
        switch (type) {
            case RestroomModelType.Restroom_SinkFemale:
                    questManager.TriggerQuest(roomSetting.roomID, QuestType.Upgrade_Restroom_FemaleSink, level);
                break;
            case RestroomModelType.Restroom_SinkMale:
                    questManager.TriggerQuest(roomSetting.roomID, QuestType.Upgrade_Restroom_MaleSink, level);
                break;
            case RestroomModelType.Restroom_StallFemale:
                    questManager.TriggerQuest(roomSetting.roomID, QuestType.Upgrade_Restroom_FemaleStall, level);
                break;
            case RestroomModelType.Restroom_StallMale:
                    questManager.TriggerQuest(roomSetting.roomID, QuestType.Upgrade_Restroom_MaleStall, level);
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