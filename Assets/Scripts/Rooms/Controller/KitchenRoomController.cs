using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenRoomController : RoomController<KitchenModelType> {
    QuestManager questManager;
    public override void TriggerQuestUpgrade(int indexItem) {
        base.TriggerQuestUpgrade(indexItem);
        if (questManager == null) questManager = ProfileManager.PlayerData.GetQuestManager();
        KitchenModelType type = roomSetting.modelPositions[indexItem].type;
        int level = roomSetting.modelPositions[indexItem].level;
        switch (type) {
            case KitchenModelType.Kitchen_Stove_Oven:
                for (int i = 0; i <= level; i++) {
                    questManager.TriggerQuest(QuestType.Upgrade_Kitchen_StoveOven, level);
                }
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