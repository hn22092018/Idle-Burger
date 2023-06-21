using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTableRoomController : RoomController<BigTableModelType>, IOnLoadRoomCallback {
    public override void TriggerQuestUpgrade(int indexItem) {
        base.TriggerQuestUpgrade(indexItem);
        int level = roomSetting.modelPositions[indexItem].level;
        if (level < 10 || (level > 10 && level % 25 != 0)) return;
        BigTableModelType type = roomSetting.modelPositions[indexItem].type;
        switch (type) {
            case BigTableModelType.BigTable_Table:
                ProfileManager.PlayerData.GetQuestManager().TriggerQuest(roomSetting.roomID, QuestType.Upgrade_BigTable_Table, level);
                break;
            case BigTableModelType.BigTable_Chair:
                ProfileManager.PlayerData.GetQuestManager().TriggerQuest(roomSetting.roomID, QuestType.Upgrade_BigTable_Chair, level);
                break;

            case BigTableModelType.BigTable_Plate:
                ProfileManager.PlayerData.GetQuestManager().TriggerQuest(roomSetting.roomID, QuestType.Upgrade_BigTable_Plates, level);
                break;

            case BigTableModelType.BigTable_Decor:
                ProfileManager.PlayerData.GetQuestManager().TriggerQuest(roomSetting.roomID, QuestType.Upgrade_BigTable_Decor, level);
                break;
        }
    }

    public override int GetFirstItemIndexByQuest(Quest quest) {
        base.GetFirstItemIndexByQuest(quest);
        int index = 0;
        switch (quest.type) {
            case QuestType.Upgrade_BigTable_Table:
                index = GetFirstItemIndexByType(BigTableModelType.BigTable_Table.ToString());
                break;
            case QuestType.Upgrade_BigTable_Chair:
                index = GetFirstItemIndexByType(BigTableModelType.BigTable_Chair.ToString());
                break;
            case QuestType.Upgrade_BigTable_Plates:
                index = GetFirstItemIndexByType(BigTableModelType.BigTable_Plate.ToString());
                break;
            case QuestType.Upgrade_BigTable_Decor:
                index = GetFirstItemIndexByType(BigTableModelType.BigTable_Decor.ToString());
                break;
        }
        return index;
    }

    public void OLoadRoomCallback() {
        Debug.Log("xxxxxxx");
        OnHireStaff();
    }
}
