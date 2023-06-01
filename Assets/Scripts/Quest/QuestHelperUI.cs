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
                break;
            case RoomID.Restroom:
            case RoomID.Restroom2:
                break;
            case RoomID.DeliverRoom:
                break;
            case RoomID.Table1:
            case RoomID.Table2:
            case RoomID.Table3:
            case RoomID.Table4:
            case RoomID.Table5:
            case RoomID.Table6:
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
                //questManager.TriggerQuest(QuestType.Build_BigTable, 0);
                break;
        }
    }
    //int GetTotalTableUnlock() {
    //    int total = 0;
    //    for (int i = 0; i < gameManager.SmallTablesRoom.Length; i++) {
    //        if (gameManager.IsUnlockSmallTable(i)) total++;
    //    }
    //    return total;
    //}
    //int GetTotalBigTableUnlock() {
    //    int total = 0;
    //    for (int i = 0; i < gameManager.BigTablesRoom.Length; i++) {
    //        if (gameManager.IsUnlockBigTable(i)) total++;
    //    }
    //    return total;
    //}

    public void OnGoQuest(Quest quest) {
        UIManager.instance.CloseAllPopup();
        switch (quest.roomTarget) {
            case RoomID.Lobby:
                ShowPanelRoomInfo(gameManager.LobbyRoom, quest);
                break;
            case RoomID.Kitchen:
                ShowPanelRoomInfo(gameManager.KitchenRoom, quest);
                break;
            case RoomID.DeliverRoom:
                ShowPanelRoomInfo(gameManager.DeliverRoom, quest);
                break;
            case RoomID.Power:
                if (gameManager.IsUnlockPowerRoom())
                    ShowPanelRoomInfo(gameManager.PowerRoom, quest);
                else UIManager.instance.ShowPanelManagerBuild(RoomID.Power);
                break;
            case RoomID.Restroom:
                if (gameManager.IsUnlockRestroom(gameManager.WCRooms[0]))
                    ShowPanelRoomInfo(gameManager.WCRooms[0], quest);
                else UIManager.instance.ShowPanelManagerBuild(quest.roomTarget);
                break;
            case RoomID.Restroom2:
                if (gameManager.IsUnlockRestroom(gameManager.WCRooms[1]))
                    ShowPanelRoomInfo(gameManager.WCRooms[1], quest);
                else UIManager.instance.ShowPanelManagerBuild(quest.roomTarget);
                break;

            case RoomID.Table1:
            case RoomID.Table2:
            case RoomID.Table3:
            case RoomID.Table4:
            case RoomID.Table5:
            case RoomID.Table6:
                if (gameManager.IsUnlockSmallTable((int)quest.roomTarget - 1))
                    ShowPanelRoomInfo(gameManager.SmallTablesRoom[(int)quest.roomTarget - 1], quest);
                else UIManager.instance.ShowPanelManagerBuild(quest.roomTarget);
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
                if (gameManager.IsUnlockBigTable((int)quest.roomTarget - 7))
                    ShowPanelRoomInfo(gameManager.BigTablesRoom[(int)quest.roomTarget - 7], quest);
                else UIManager.instance.ShowPanelManagerBuild(quest.roomTarget);
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
