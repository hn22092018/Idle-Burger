using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenRoomController : RoomController<KitchenModelType> {
    QuestManager questManager;
    public override void TriggerQuestUpgrade(int indexItem) {
        base.TriggerQuestUpgrade(indexItem);
        int level = roomSetting.modelPositions[indexItem].level;
        if (level < 10 || (level > 10 && level % 25 != 0)) return;
        if (questManager == null) questManager = ProfileManager.PlayerData.GetQuestManager();
        KitchenModelType type = roomSetting.modelPositions[indexItem].type;
        switch (type) {
            case KitchenModelType.Kitchen_Stove_Oven:
                    questManager.TriggerQuest(roomSetting.roomID, QuestType.Upgrade_Kitchen_StoveOven, level);
                break;
        }
    }
    public override int GetFirstItemIndexByQuest(Quest quest) {
        int index = 0;
        switch (quest.type) {
            case QuestType.Upgrade_Kitchen_StoveOven:
                index = GetFirstItemIndexByType(KitchenModelType.Kitchen_Stove_Oven.ToString());
                break;
        }
        return index;
    }
}