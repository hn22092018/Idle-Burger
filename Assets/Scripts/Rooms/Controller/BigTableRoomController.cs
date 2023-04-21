using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTableRoomController : RoomController<BigTableModelType> ,IOnLoadRoomCallback{
    public override void TriggerQuestUpgrade(int indexItem) {
        base.TriggerQuestUpgrade(indexItem);
        BigTableModelType type = roomSetting.modelPositions[indexItem].type;
        int level = roomSetting.modelPositions[indexItem].level;
        switch (type) {
            case BigTableModelType.BigTable_Table:
                for (int i = 1; i <= level; i++) {
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_BigTable_Table, i);
                }
                break;
            case BigTableModelType.BigTable_Chair:
                for (int i = 1; i <= level; i++) {
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_BigTable_Chair, i);
                }
                break;

            case BigTableModelType.BigTable_Plate:
                for (int i = 1; i <= level; i++) {
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_BigTable_Plates, i);
                }
                break;

            case BigTableModelType.BigTable_Decor:
                for (int i = 1; i <= level; i++) {
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_BigTable_Decor, i);
                }
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
