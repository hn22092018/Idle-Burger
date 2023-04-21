using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyRoomController : RoomController<LobbyModelType> {
    QuestManager questManager;
    public override void TriggerQuestUpgrade(int indexItem) {
        base.TriggerQuestUpgrade(indexItem);
        if (questManager == null) questManager = ProfileManager.PlayerData.GetQuestManager();
        LobbyModelType type = roomSetting.modelPositions[indexItem].type;
        int level = roomSetting.modelPositions[indexItem].level;
        switch (type) {
            case LobbyModelType.Lobby_Desk:
                for (int i = 1; i <= level; i++) {
                    questManager.TriggerQuest(QuestType.Upgrade_Lobby_Desk, i);
                }
                break;
            case LobbyModelType.Lobby_Computer:
                for (int i = 1; i <= level; i++) {
                    questManager.TriggerQuest(QuestType.Upgrade_Lobby_Computer, i);
                }
                break;
            case LobbyModelType.Lobby_Case:
                for (int i = 1; i <= level; i++) {
                    questManager.TriggerQuest(QuestType.Upgrade_Lobby_Case, i);
                }
                break;
            case LobbyModelType.Lobby_Decor:
                for (int i = 1; i <= level; i++) {
                    questManager.TriggerQuest(QuestType.Upgrade_Lobby_Decor, i);
                }
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
