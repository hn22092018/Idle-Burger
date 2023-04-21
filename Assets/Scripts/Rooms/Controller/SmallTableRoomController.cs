using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTableRoomController : RoomController<SmallTableModelType> {

    public override void TriggerQuestUpgrade(int indexItem) {
        base.TriggerQuestUpgrade(indexItem);
        SmallTableModelType type = roomSetting.modelPositions[indexItem].type;
        int level = roomSetting.modelPositions[indexItem].level;
        switch (type) {
            case SmallTableModelType.SmallTable_Table:
                for (int i = 1; i <= level; i++) {
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_SmallTable_Table, i);
                }
                break;
            case SmallTableModelType.SmallTable_Chair:
                for (int i = 1; i <= level; i++) {
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_SmallTable_Chair, i);
                }
                break;

            case SmallTableModelType.SmallTable_Plate:
                for (int i = 1; i <= level; i++) {
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_SmallTable_Plates, i);
                }
                break;

            case SmallTableModelType.SmallTable_Decor:
                for (int i = 1; i <= level; i++) {
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_SmallTable_Decor, i);
                }
                break;
        }
    }

    public override int GetFirstItemIndexByQuest(Quest quest) {
        base.GetFirstItemIndexByQuest(quest);
        int index = 0;
        switch (quest.type) {
            case QuestType.Upgrade_SmallTable_Table:
                index = GetFirstItemIndexByType(SmallTableModelType.SmallTable_Table.ToString());
                break;
            case QuestType.Upgrade_SmallTable_Chair:
                index = GetFirstItemIndexByType(SmallTableModelType.SmallTable_Chair.ToString());
                break;
            case QuestType.Upgrade_SmallTable_Plates:
                index = GetFirstItemIndexByType(SmallTableModelType.SmallTable_Plate.ToString());
                break;
            case QuestType.Upgrade_SmallTable_Decor:
                index = GetFirstItemIndexByType(SmallTableModelType.SmallTable_Decor.ToString());
                break;
        }
        return index;
    }

  
}
