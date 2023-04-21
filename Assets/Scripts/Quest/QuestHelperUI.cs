using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHelperUI : MonoBehaviour {
    public static QuestHelperUI instance;
    QuestManager questManager;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start() {
        instance = this;
        questManager = ProfileManager.PlayerData.GetQuestManager();
        gameManager = GameManager.instance;
    }
    public void TriggerQuestBuildRoom(RoomID buildTarget) {
        switch (buildTarget) {
            case RoomID.Power:
                questManager.TriggerQuest(QuestType.Build_PowerRoom, 0);
                break;
            case RoomID.Restroom:
            case RoomID.Restroom2:
                questManager.TriggerQuest(QuestType.Build_Restroom, 0);
                break;
            case RoomID.DeliverRoom:
                questManager.TriggerQuest(QuestType.Build_DeliverRoom, 0);
                break;
            case RoomID.Table1:
            case RoomID.Table2:
            case RoomID.Table3:
            case RoomID.Table4:
            case RoomID.Table5:
            case RoomID.Table6:
                questManager.TriggerQuest(QuestType.Build_Table, 0);
                questManager.TriggerQuest(QuestType.Buy_SmallTables, GetTotalTableUnlock());
                break;
            case RoomID.BigTable1:
            case RoomID.BigTable2:
            case RoomID.BigTable3:
            case RoomID.BigTable4:
            case RoomID.BigTable5:
            case RoomID.BigTable6:
            case RoomID.BigTable7:
            case RoomID.BigTable8:
            case RoomID.BigTable9:
            case RoomID.BigTable10:
            case RoomID.BigTable11:
            case RoomID.BigTable12:
            case RoomID.BigTable13:
            case RoomID.BigTable14:
                questManager.TriggerQuest(QuestType.Build_BigTable, 0);
                questManager.TriggerQuest(QuestType.Buy_BigTables, GetTotalBigTableUnlock());
                break;
        }
    }
    int GetTotalTableUnlock() {
        int total = 0;
        for (int i = 0; i < gameManager.SmallTablesRoom.Length; i++) {
            if (gameManager.IsUnlockSmallTable(i)) total++;
        }
        return total;
    }
    int GetTotalBigTableUnlock() {
        int total = 0;
        for (int i = 0; i < gameManager.BigTablesRoom.Length; i++) {
            if (gameManager.IsUnlockBigTable(i)) total++;
        }
        return total;
    }
  
    public void OnGoQuest(Quest quest) {
        UIManager.instance.CloseAllPopup();
        switch (quest.roomTarget) {
            case "Lobby":
                ShowPanelRoomInfo(gameManager.LobbyRoom, quest);
                break;
            case "Manager":
                ShowPanelRoomInfo(gameManager.ManagerRoom, quest);
                break;
            case "Kitchen":
                ShowPanelRoomInfo(gameManager.KitchenRoom, quest);
                break;
            case "Power":
                if (gameManager.IsUnlockPowerRoom())
                    ShowPanelRoomInfo(gameManager.PowerRoom, quest);
                else UIManager.instance.ShowPanelManagerBuild(RoomID.Power);
                break;
            case "Restroom":
                if (gameManager.IsUnlockRestroom(gameManager.WCRooms[0]))
                    ShowPanelRoomInfo(gameManager.WCRooms[0], quest);
                else UIManager.instance.ShowPanelManagerBuild(RoomID.Restroom);
                break;
            
            case "CoffeeRoom":
                if (gameManager.IsUnlockDeliverRoom())
                    ShowPanelRoomInfo(gameManager.DeliverRoom, quest);
                else UIManager.instance.ShowPanelManagerBuild(RoomID.DeliverRoom);
                break;
            case "Table":
                if (quest.type == QuestType.Buy_SmallTables) {
                    if (!gameManager.IsUnlockSmallTable(1)) UIManager.instance.ShowPanelManagerBuild(RoomID.Table2);
                } else {
                    if (gameManager.IsUnlockSmallTable(0))
                        ShowPanelRoomInfo(gameManager.SmallTablesRoom[0], quest);
                    else UIManager.instance.ShowPanelManagerBuild(RoomID.Table1);
                }

                break;
            case "BigTable":
                if (quest.type == QuestType.Buy_BigTables) {
                    if (!gameManager.IsUnlockBigTable(0)) UIManager.instance.ShowPanelManagerBuild(RoomID.BigTable1);
                    else if (!gameManager.IsUnlockBigTable(1)) UIManager.instance.ShowPanelManagerBuild(RoomID.BigTable2);
                    else if (!gameManager.IsUnlockBigTable(2)) UIManager.instance.ShowPanelManagerBuild(RoomID.BigTable3);
                    else if (!gameManager.IsUnlockBigTable(3)) UIManager.instance.ShowPanelManagerBuild(RoomID.BigTable4);
                    else if (!gameManager.IsUnlockBigTable(4)) UIManager.instance.ShowPanelManagerBuild(RoomID.BigTable5);
                    else if (!gameManager.IsUnlockBigTable(5)) UIManager.instance.ShowPanelManagerBuild(RoomID.BigTable6);
                    else if (!gameManager.IsUnlockBigTable(6)) UIManager.instance.ShowPanelManagerBuild(RoomID.BigTable7);
                    else if (!gameManager.IsUnlockBigTable(7)) UIManager.instance.ShowPanelManagerBuild(RoomID.BigTable8);
                    else if (!gameManager.IsUnlockBigTable(8)) UIManager.instance.ShowPanelManagerBuild(RoomID.BigTable9);
                    else if (!gameManager.IsUnlockBigTable(9)) UIManager.instance.ShowPanelManagerBuild(RoomID.BigTable10);
                    else if (!gameManager.IsUnlockBigTable(10)) UIManager.instance.ShowPanelManagerBuild(RoomID.BigTable11);
                    else if (!gameManager.IsUnlockBigTable(11)) UIManager.instance.ShowPanelManagerBuild(RoomID.BigTable12);
                    else if (!gameManager.IsUnlockBigTable(12)) UIManager.instance.ShowPanelManagerBuild(RoomID.BigTable13);
                    else if (!gameManager.IsUnlockBigTable(13)) UIManager.instance.ShowPanelManagerBuild(RoomID.BigTable14);
                } else {
                    if (gameManager.IsUnlockBigTable(0))
                        ShowPanelRoomInfo(gameManager.BigTablesRoom[0], quest);
                    else UIManager.instance.ShowPanelManagerBuild(RoomID.BigTable1);
                }
                break;
        }

    }
    void ShowPanelRoomInfo(IRoomController room, Quest quest) {
        int indexItem = room.GetFirstItemIndexByQuest(quest);
        GameManager.instance.selectedRoom = room;
        CameraMove.instance.ChangePosition(room.GetTransform().position - new Vector3(0, 6.5f, 0f), null);
        UIManager.instance.ShowPanelRoomInfo(room);
        PanelRoomInfo.instance.OnShowInfoItem(indexItem);
    }
}
