using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyRoomController : RoomController<LobbyModelType> {
    QuestManager questManager;
    public override void TriggerQuestUpgrade(int indexItem) {
        base.TriggerQuestUpgrade(indexItem);
        int level = roomSetting.modelPositions[indexItem].level;
        if (level < 10 || (level > 10 && level % 25 != 0)) return;
        if (questManager == null) questManager = ProfileManager.PlayerData.GetQuestManager();
        LobbyModelType type = roomSetting.modelPositions[indexItem].type;
        switch (type) {
            case LobbyModelType.Lobby_Desk:
                    questManager.TriggerQuest(QuestType.Upgrade_Lobby_Desk, level);
                break;
            case LobbyModelType.Lobby_Computer:
                    questManager.TriggerQuest(QuestType.Upgrade_Lobby_Computer, level);
                break;
            case LobbyModelType.Lobby_Case:
                    questManager.TriggerQuest(QuestType.Upgrade_Lobby_Case, level);
                break;
            case LobbyModelType.Lobby_Decor:
                    questManager.TriggerQuest(QuestType.Upgrade_Lobby_Decor, level);
                break;
        }
    }
    public override int GetFirstItemIndexByQuest(Quest quest) {
        int index = 0;
        switch (quest.type) {
            case QuestType.Upgrade_Lobby_Desk:
                index = GetFirstItemIndexByType(LobbyModelType.Lobby_Desk.ToString());
                break;
            case QuestType.Upgrade_Lobby_Computer:
                index = GetFirstItemIndexByType(LobbyModelType.Lobby_Computer.ToString());
                break;
            case QuestType.Upgrade_Lobby_Case:
                index = GetFirstItemIndexByType(LobbyModelType.Lobby_Case.ToString());
                break;
            case QuestType.Upgrade_Lobby_Decor:
                index = GetFirstItemIndexByType(LobbyModelType.Lobby_Decor.ToString());
                break;
        }
        return index;

    }
}
