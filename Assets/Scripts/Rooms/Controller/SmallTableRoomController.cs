using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTableRoomController : RoomController<SmallTableModelType> {

    public override void TriggerQuestUpgrade(int indexItem) {
        base.TriggerQuestUpgrade(indexItem);
        int level = roomSetting.modelPositions[indexItem].level;
        if (level < 10 || (level > 10 && level % 25 != 0)) return;
        SmallTableModelType type = roomSetting.modelPositions[indexItem].type;
        switch (type) {
            case SmallTableModelType.SmallTable_Table:
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_SmallTable_Table, level);
                break;
            case SmallTableModelType.SmallTable_Chair:
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_SmallTable_Chair, level);
                break;

            case SmallTableModelType.SmallTable_Plate:
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_SmallTable_Plates, level);
                break;

            case SmallTableModelType.SmallTable_Decor:
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_SmallTable_Decor, level);
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
