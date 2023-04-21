using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRoomController : RoomController<PowerModelType> {
    public override void TriggerQuestUpgrade(int indexItem) {
        base.TriggerQuestUpgrade(indexItem);
        PowerModelType type = roomSetting.modelPositions[indexItem].type;
        int level = roomSetting.modelPositions[indexItem].level;
        switch (type) {
            case PowerModelType.Power_BigGenerator:
                ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_Power_BigGenerator, level);
                break;
            case PowerModelType.Power_SmallGenerator:
                ProfileManager.PlayerData.GetQuestManager().TriggerQuest(QuestType.Upgrade_Power_SmallGenerator, level);
                break;

        }
    }
    public override int GetFirstItemIndexByQuest(Quest quest) {
        base.GetFirstItemIndexByQuest(quest);
        int index = 0;
        switch (quest.type) {
            case QuestType.Upgrade_Power_SmallGenerator:
                index = GetFirstItemIndexByType(PowerModelType.Power_SmallGenerator.ToString());
                break;
            case QuestType.Upgrade_Power_BigGenerator:
                index = GetFirstItemIndexByType(PowerModelType.Power_BigGenerator.ToString());
                break;
        }
        return index;
    }
}