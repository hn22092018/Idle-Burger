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
        btnGoRoom.gameObject.SetActive(isUnlock);
        txtRoomIncome.text = new BigNumber(income).ToString();
        fillProcessUpgrade.localScale = new Vector3(process, 1, 1);
        txtRoomUpgradeProcess.text = Math.Round(process * 100, 1) + "%";
    }
    void GoToRoom() {
        UIManager.instance.ClosePanelStatistic();
        switch (roomID) {
            case RoomID.Table1:
            case RoomID.Table2:
            case RoomID.Table3:
            case RoomID.Table4:
            case RoomID.Table5:
            case RoomID.Table6:
                if (gameManager.IsUnlockSmallTable((int)roomID-1))
                    ShowPanelRoomInfo(gameManager.SmallTablesRoom[(int)roomID - 1]);
                else UIManager.instance.ShowPanelManagerBuild(roomID);
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
                GoToBigTable(roomID, (int)roomID - 7);
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
            case RoomID.Table2:
            case RoomID.Table3:
            case RoomID.Table4:
            case RoomID.Table5:
            case RoomID.Table6:
                isUnlock = gameManager.IsUnlockSmallTable((int)roomID - 1);
                income = gameManager.SmallTablesRoom[(int)roomID - 1].GetTotalMoneyEarn();
                process = gameManager.SmallTablesRoom[(int)roomID - 1].GetProcessUpgrade();
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
                GetBigTableValue((int)roomID - 7);
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
