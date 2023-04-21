using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverRoomController : RoomController<DeliverModelType> {
    QuestManager questManager;

    public override void TriggerQuestUpgrade(int indexItem) {
        base.TriggerQuestUpgrade(indexItem);
        if (questManager == null) questManager = ProfileManager.PlayerData.GetQuestManager();
        DeliverModelType type = roomSetting.modelPositions[indexItem].type;
        int level = roomSetting.modelPositions[indexItem].level;
        switch (type) {
            case DeliverModelType.Deliver_StaffTable:
                for (int i = 1; i <= level; i++) {
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_Deliver_StaffTable, i);
                }
                questManager.TriggerQuest(QuestType.Buy_DeliverStaffTables, GetTotalTableUnlock());
                break;
           
            case DeliverModelType.Deliver_BakeryGlass:
                for (int i = 1; i <= level; i++) {
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_Deliver_BakeryGlass, i);
                }
                break;
           
            case DeliverModelType.Deliver_Fridge:
                for (int i = 1; i <= level; i++) {
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_Deliver_Fridge, i);
                }
                break;
            case DeliverModelType.Deliver_Decor:
                for (int i = 1; i <= level; i++) {
                    ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_Deliver_Decor, i);
                }
                break;

        }
    }
    int GetTotalTableUnlock() {
        int total = 0;
        total += GetTotalModelOwnerByType(DeliverModelType.Deliver_StaffTable.ToString());
        return total;
    }
    public override int GetFirstItemIndexByQuest(Quest quest) {
        int index = 0;
        switch (quest.type) {
            case QuestType.Upgrade_Deliver_StaffTable:
                index = GetFirstItemIndexByType(DeliverModelType.Deliver_StaffTable.ToString());
                break;
            case QuestType.Upgrade_Deliver_BakeryGlass:
                index = GetFirstItemIndexByType(DeliverModelType.Deliver_BakeryGlass.ToString());
                break;
            case QuestType.Upgrade_Deliver_Fridge:
                index = GetFirstItemIndexByType(DeliverModelType.Deliver_Fridge.ToString());
                break;
            case QuestType.Upgrade_Deliver_Decor:
                index = GetFirstItemIndexByType(DeliverModelType.Deliver_Decor.ToString());
                break;
        }
        return index;
    }
}