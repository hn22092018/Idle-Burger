using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerRoomController : RoomController<ManagerModelType> {
    //public override void TriggerQuestUpgrade(int indexItem) {
    //    base.TriggerQuestUpgrade(indexItem);
    //    ManagerModelType type = roomSetting.modelPositions[indexItem].type;
    //    int level = roomSetting.modelPositions[indexItem].level;
    //    switch (type) {
    //        case ManagerModelType.Manager_Desk:
    //            for (int i = 0; i <= level; i++) {
    //                ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_Manager_Desk, level);
    //            }
    //            break;
    //        case ManagerModelType.Manager_Table:
    //            for (int i = 0; i <= level; i++) {
    //                ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_Manager_Table, level);
    //            }
    //            break;
    //        case ManagerModelType.Manager_Bookshelf:
    //            for (int i = 0; i <= level; i++) {
    //                ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_Manager_Bookshelf, level);
    //            }
    //            break;
           
    //        case ManagerModelType.Manager_Fridge:
    //            for (int i = 0; i <= level; i++) {
    //                ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_Manager_Fridge, level);
    //            }
    //            break;
    //    }
    //}
    //public override int GetFirstItemIndexByQuest(Quest quest) {
    //    int index = 0;
    //    switch (quest.type) {
    //        case QuestType.Upgrade_Manager_Desk:
    //            index = GetFirstItemIndexByType(ManagerModelType.Manager_Desk.ToString());
    //            break;
    //        case QuestType.Upgrade_Manager_Table:
    //            index = GetFirstItemIndexByType(ManagerModelType.Manager_Table.ToString());
    //            break;
    //        case QuestType.Upgrade_Manager_Fridge:
    //            index = GetFirstItemIndexByType(ManagerModelType.Manager_Fridge.ToString());
    //            break;
    //        case QuestType.Upgrade_Manager_Bookshelf:
    //            index = GetFirstItemIndexByType(ManagerModelType.Manager_Bookshelf.ToString());
    //            break;
    //    }
    //    return index;

    //}
}