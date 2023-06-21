using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverRoomController : RoomController<DeliverModelType> {
    QuestManager questManager;

    public override void TriggerQuestUpgrade(int indexItem) {
        base.TriggerQuestUpgrade(indexItem);
        int level = roomSetting.modelPositions[indexItem].level;
        if (level < 10 || (level > 10 && level % 25 != 0)) return;
        if (questManager == null) questManager = ProfileManager.PlayerData.GetQuestManager();
        DeliverModelType type = roomSetting.modelPositions[indexItem].type;
        switch (type) {
            case DeliverModelType.Deliver_StaffTable:
                ProfileManager.PlayerData.GetQuestManager().TriggerQuest(roomSetting.roomID, QuestType.Upgrade_Deliver_StaffTable, level);
                break;

            case DeliverModelType.Deliver_BakeryGlass:
                ProfileManager.PlayerData.GetQuestManager().TriggerQuest(roomSetting.roomID, QuestType.Upgrade_Deliver_BakeryGlass, level);
                break;

            case DeliverModelType.Deliver_Fridge:
                ProfileManager.PlayerData.GetQuestManager().TriggerQuest(roomSetting.roomID, QuestType.Upgrade_Deliver_Fridge, level);
                break;
            case DeliverModelType.Deliver_Decor:
                ProfileManager.PlayerData.GetQuestManager().TriggerQuest(roomSetting.roomID, QuestType.Upgrade_Deliver_Decor, level);
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