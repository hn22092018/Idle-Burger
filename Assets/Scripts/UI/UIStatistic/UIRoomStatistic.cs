using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomStatistic : MonoBehaviour {
    [SerializeField] RoomID roomID;
    [SerializeField] int GroupID;
    [SerializeField] Text txtRoomName;
    [SerializeField] Text txtRoomIncome;
    [SerializeField] Text txtRoomUpgradeProcess;
    [SerializeField] Transform fillProcessUpgrade;
    [SerializeField] Button btnGoRoom;
    [SerializeField] GameObject lockRoom;
    [SerializeField] Text txtLockInfo;
    [SerializeField] Text txtRequireStar;
    GameManager gameManager;
    BigNumber income;
    bool isUnlock;
    float process;
    private void Awake() {
        btnGoRoom.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            GoToRoom();
        });
        gameManager = GameManager.instance;
    }
    public void OnSetupRoomID(RoomID id) {
        roomID = id;
    }
    public void OnLoadRoomInfo() {
        txtRoomName.text = ProfileManager.Instance.dataConfig.GameText.RoomIDToString(roomID);
        string strNotUnlock = " is not unlocked yet.";
        if (ProfileManager.Instance.dataConfig.GameText.GetTextByID(88) != "") {
            strNotUnlock = " " + ProfileManager.Instance.dataConfig.GameText.GetTextByID(88);
        }
        txtLockInfo.text = ProfileManager.Instance.dataConfig.GameText.RoomIDToString(roomID) + strNotUnlock;
        LoadState();
        lockRoom.SetActive(!isUnlock);
        string sRequire = ProfileManager.Instance.dataConfig.GameText.GetTextByID(100).ToUpper();
        txtRequireStar.text = sRequire + " " + gameManager.buildData.GetData(roomID).starRequire;
        txtRequireStar.gameObject.SetActive(!isUnlock && gameManager.buildData.GetData(roomID).starRequire > 0);
        btnGoRoom.gameObject.SetActive(isUnlock);
        txtRoomIncome.text = new BigNumber(income).ToString();
        fillProcessUpgrade.localScale = new Vector3(process, 1, 1);
        txtRoomUpgradeProcess.text = Math.Round(process * 100, 1) + "%";
    }
    void GoToRoom() {
        UIManager.instance.ClosePanelStatistic();
        switch (roomID) {
            case RoomID.Table1:
                if (gameManager.IsUnlockSmallTable(0))
                    ShowPanelRoomInfo(gameManager.SmallTablesRoom[0]);
                else UIManager.instance.ShowPanelManagerBuild(RoomID.Table1);
                break;
            case RoomID.Table2:
                if (gameManager.IsUnlockSmallTable(1))
                    ShowPanelRoomInfo(gameManager.SmallTablesRoom[1]);
                else UIManager.instance.ShowPanelManagerBuild(RoomID.Table2);
                break;
            case RoomID.Table3:
                if (gameManager.IsUnlockSmallTable(2))
                    ShowPanelRoomInfo(gameManager.SmallTablesRoom[2]);
                else UIManager.instance.ShowPanelManagerBuild(RoomID.Table3);
                break;
            case RoomID.Table4:
                if (gameManager.IsUnlockSmallTable(3))
                    ShowPanelRoomInfo(gameManager.SmallTablesRoom[3]);
                else UIManager.instance.ShowPanelManagerBuild(RoomID.Table4);
                break;
            case RoomID.Table5:
                if (gameManager.IsUnlockSmallTable(4))
                    ShowPanelRoomInfo(gameManager.SmallTablesRoom[4]);
                else UIManager.instance.ShowPanelManagerBuild(RoomID.Table5);
                break;
            case RoomID.Table6:
                if (gameManager.IsUnlockSmallTable(5))
                    ShowPanelRoomInfo(gameManager.SmallTablesRoom[5]);
                else UIManager.instance.ShowPanelManagerBuild(RoomID.Table6);
                break;
            case RoomID.BigTable1:
                GoToBigTable(roomID, 0);
                break;
            case RoomID.BigTable2:
                GoToBigTable(roomID, 1);
                break;
            case RoomID.BigTable3:
                GoToBigTable(roomID, 2);
                break;
            case RoomID.BigTable4:
                GoToBigTable(roomID, 3);
                break;
            case RoomID.BigTable5:
                GoToBigTable(roomID, 4);
                break;
            case RoomID.BigTable6:
                GoToBigTable(roomID, 5);
                break;
            case RoomID.BigTable7:
                GoToBigTable(roomID, 6);
                break;
            case RoomID.BigTable8:
                GoToBigTable(roomID, 7);
                break;
            case RoomID.BigTable9:
                GoToBigTable(roomID, 8);
                break;
            case RoomID.BigTable10:
                GoToBigTable(roomID, 9);
                break;
            case RoomID.BigTable11:
                GoToBigTable(roomID, 10);
                break;
            case RoomID.BigTable12:
                GoToBigTable(roomID, 11);
                break;
            case RoomID.BigTable13:
                GoToBigTable(roomID, 12);
                break;
            case RoomID.BigTable14:
                GoToBigTable(roomID, 13);
                break;
            case RoomID.Lobby:
                ShowPanelRoomInfo(gameManager.LobbyRoom);
                break;
            case RoomID.Kitchen:
                ShowPanelRoomInfo(gameManager.KitchenRoom);
                break;
            case RoomID.Restroom:
                if (gameManager.IsUnlockRestroom(gameManager.WCRooms[0]))
                    ShowPanelRoomInfo(gameManager.WCRooms[0]);
                else UIManager.instance.ShowPanelManagerBuild(RoomID.Restroom);
                break;
            case RoomID.Restroom2:
                if (gameManager.IsUnlockRestroom(gameManager.WCRooms[1]))
                    ShowPanelRoomInfo(gameManager.WCRooms[1]);
                else UIManager.instance.ShowPanelManagerBuild(RoomID.Restroom2);
                break;
            
            case RoomID.DeliverRoom:
                if (gameManager.IsUnlockDeliverRoom())
                    ShowPanelRoomInfo(gameManager.DeliverRoom);
                else UIManager.instance.ShowPanelManagerBuild(RoomID.DeliverRoom);
                break;
            case RoomID.Power:
                if (gameManager.IsUnlockPowerRoom())
                    ShowPanelRoomInfo(gameManager.PowerRoom);
                else UIManager.instance.ShowPanelManagerBuild(RoomID.Power);
                break;
        }
    }
  
    void GoToBigTable(RoomID roomID, int index) {
        if (gameManager.IsUnlockBigTable(index))
            ShowPanelRoomInfo(gameManager.BigTablesRoom[index]);
        else UIManager.instance.ShowPanelManagerBuild(roomID);
    }
    void LoadState() {
        switch (roomID) {
            case RoomID.Table1:
                isUnlock = gameManager.IsUnlockSmallTable(0);
                income = gameManager.SmallTablesRoom[0].GetTotalMoneyEarn();
                process = gameManager.SmallTablesRoom[0].GetProcessUpgrade();
                break;
            case RoomID.Table2:
                isUnlock = gameManager.IsUnlockSmallTable(1);
                income = gameManager.SmallTablesRoom[1].GetTotalMoneyEarn();
                process = gameManager.SmallTablesRoom[1].GetProcessUpgrade();
                break;
            case RoomID.Table3:
                isUnlock = gameManager.IsUnlockSmallTable(2);
                income = gameManager.SmallTablesRoom[2].GetTotalMoneyEarn();
                process = gameManager.SmallTablesRoom[2].GetProcessUpgrade();
                break;
            case RoomID.BigTable1:
                GetBigTableValue(0);
                break;
            case RoomID.BigTable2:
                GetBigTableValue(1);
                break;
            case RoomID.BigTable3:
                GetBigTableValue(2);
                break;
            case RoomID.BigTable4:
                GetBigTableValue(3);
                break;
            case RoomID.BigTable5:
                GetBigTableValue(4);
                break;
            case RoomID.BigTable6:
                GetBigTableValue(5);
                break;
            case RoomID.BigTable7:
                GetBigTableValue(6);
                break;
            case RoomID.BigTable8:
                GetBigTableValue(7);
                break;
            case RoomID.BigTable9:
                GetBigTableValue(8);
                break;
            case RoomID.BigTable10:
                GetBigTableValue(9);
                break;
            case RoomID.BigTable11:
                GetBigTableValue(10);
                break;
            case RoomID.BigTable12:
                GetBigTableValue(11);
                break;
            case RoomID.BigTable13:
                GetBigTableValue(12);
                break;
            case RoomID.BigTable14:
                GetBigTableValue(13);
                break;
            case RoomID.Manager:
                isUnlock = true;
                income = gameManager.ManagerRoom.GetTotalMoneyEarn();
                process = gameManager.ManagerRoom.GetProcessUpgrade();
                break;
            case RoomID.Lobby:
                isUnlock = true;
                income = gameManager.LobbyRoom.GetTotalMoneyEarn();
                process = gameManager.LobbyRoom.GetProcessUpgrade();
                break;
            case RoomID.Kitchen:
                isUnlock = true;
                income = gameManager.KitchenRoom.GetTotalMoneyEarn();
                process = gameManager.KitchenRoom.GetProcessUpgrade();
                break;
            case RoomID.Restroom:
                isUnlock = gameManager.IsUnlockRestroom(gameManager.WCRooms[0]);
                income = gameManager.WCRooms[0].GetTotalMoneyEarn();
                process = gameManager.WCRooms[0].GetProcessUpgrade();
                break;
            case RoomID.Restroom2:
                isUnlock = gameManager.IsUnlockRestroom(gameManager.WCRooms[1]);
                income = gameManager.WCRooms[1].GetTotalMoneyEarn();
                process = gameManager.WCRooms[1].GetProcessUpgrade();
                break;
           
            case RoomID.DeliverRoom:
                isUnlock = gameManager.IsUnlockDeliverRoom();
                income = gameManager.DeliverRoom.GetTotalMoneyEarn();
                process = gameManager.DeliverRoom.GetProcessUpgrade();
                break;
            case RoomID.Power:
                isUnlock = true;
                income = 0;
                process = gameManager.PowerRoom.GetProcessUpgrade();
                break;
        }
    }
    void ShowPanelRoomInfo(IRoomController room) {
        GameManager.instance.selectedRoom = room;
        CameraMove.instance.ChangePosition(room.GetTransform().position - new Vector3(0, 6.5f, 0f), null);
        UIManager.instance.ShowPanelRoomInfo(room);
    }
    void GetBigTableValue(int id) {
        isUnlock = gameManager.IsUnlockBigTable(id);
        income = gameManager.BigTablesRoom[id].GetTotalMoneyEarn();
        process = gameManager.BigTablesRoom[id].GetProcessUpgrade();
    }
   
}
