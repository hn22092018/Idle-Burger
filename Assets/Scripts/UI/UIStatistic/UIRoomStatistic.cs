using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomStatistic : UIEffect {
    [SerializeField] RoomID roomID;
    [SerializeField] int GroupID;
    [SerializeField] Text txtRoomName;
    [SerializeField] Text txtRoomIncome;
    [SerializeField] Text txtRoomUpgradeProcess;
    [SerializeField] Transform fillProcessUpgrade;
    [SerializeField] Button btnGoRoom;
    [SerializeField] GameObject lockRoom;
    [SerializeField] Text txtLockInfo;
    [SerializeField] GameObject notifyEnableUpgrade;
    GameManager gameManager;
    BigNumber income;
    bool isUnlock;
    float process;
    BigNumber minUpgradeValue;
    int indexHasMinUpgrade;
    private void Awake() {
        btnGoRoom.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            ScaleEffectButton(btnGoRoom, GoToRoom);
        });
        gameManager = GameManager.instance;
    }
    public void OnSetupRoomID(RoomID id) {
        roomID = id;
    }
    public void OnLoadRoomInfo() {
        txtRoomName.text = ProfileManager.Instance.dataConfig.GameText.RoomIDToString(roomID);
        string strNotUnlock = " is not unlocked yet.";
        //if (ProfileManager.Instance.dataConfig.GameText.GetTextByID(88) != "") {
        //    strNotUnlock = " " + ProfileManager.Instance.dataConfig.GameText.GetTextByID(88);
        //}
        txtLockInfo.text = ProfileManager.Instance.dataConfig.GameText.RoomIDToString(roomID) + strNotUnlock;
        LoadState();
        lockRoom.SetActive(!isUnlock);
        txtRoomIncome.text = new BigNumber(income).ToString();
        fillProcessUpgrade.localScale = new Vector3(process, 1, 1);
        txtRoomUpgradeProcess.text = Math.Round(process * 100, 1) + "%";
        btnGoRoom.gameObject.SetActive(process < 1 && isUnlock);

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
                if (gameManager.IsUnlockSmallTable((int)roomID - 1))
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
                SetRoomValues(gameManager.IsUnlockSmallTable((int)roomID - 1), gameManager.SmallTablesRoom[(int)roomID - 1].GetTotalMoneyEarn(), gameManager.SmallTablesRoom[(int)roomID - 1].GetProcessUpgrade(), gameManager.SmallTablesRoom[(int)roomID - 1].GetMinUpgradeValue(), gameManager.SmallTablesRoom[(int)roomID - 1].GetIndexHasMinUpgradeValue());
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
                SetRoomValues(gameManager.IsUnlockBigTable((int)roomID - 7), gameManager.BigTablesRoom[(int)roomID - 7].GetTotalMoneyEarn(), gameManager.BigTablesRoom[(int)roomID - 7].GetProcessUpgrade(), gameManager.BigTablesRoom[(int)roomID - 7].GetMinUpgradeValue(), gameManager.BigTablesRoom[(int)roomID - 7].GetIndexHasMinUpgradeValue());
                break;
            case RoomID.Lobby:
                SetRoomValues(true, gameManager.LobbyRoom.GetTotalMoneyEarn(), gameManager.LobbyRoom.GetProcessUpgrade(), gameManager.LobbyRoom.GetMinUpgradeValue(), gameManager.LobbyRoom.GetIndexHasMinUpgradeValue());
                break;
            case RoomID.Kitchen:
                SetRoomValues(true, gameManager.KitchenRoom.GetTotalMoneyEarn(), gameManager.KitchenRoom.GetProcessUpgrade(), gameManager.KitchenRoom.GetMinUpgradeValue(), gameManager.KitchenRoom.GetIndexHasMinUpgradeValue());
                break;
            case RoomID.Restroom:
                SetRoomValues(gameManager.IsUnlockRestroom(gameManager.WCRooms[0]), gameManager.WCRooms[0].GetTotalMoneyEarn(), gameManager.WCRooms[0].GetProcessUpgrade(), gameManager.WCRooms[0].GetMinUpgradeValue(), gameManager.WCRooms[0].GetIndexHasMinUpgradeValue());
                break;
            case RoomID.Restroom2:
                SetRoomValues(gameManager.IsUnlockRestroom(gameManager.WCRooms[1]), gameManager.WCRooms[1].GetTotalMoneyEarn(), gameManager.WCRooms[1].GetProcessUpgrade(), gameManager.WCRooms[1].GetMinUpgradeValue(), gameManager.WCRooms[1].GetIndexHasMinUpgradeValue());
                break;

            case RoomID.DeliverRoom:
                SetRoomValues(gameManager.IsUnlockDeliverRoom(), gameManager.DeliverRoom.GetTotalMoneyEarn(), gameManager.DeliverRoom.GetProcessUpgrade(), gameManager.DeliverRoom.GetMinUpgradeValue(), gameManager.DeliverRoom.GetIndexHasMinUpgradeValue());
                break;
            case RoomID.Power:
                SetRoomValues(true, 0, gameManager.PowerRoom.GetProcessUpgrade(), gameManager.PowerRoom.GetMinUpgradeValue(), gameManager.PowerRoom.GetIndexHasMinUpgradeValue());
                break;
        }
    }
    void SetRoomValues(bool _isUnlock, BigNumber _income, float _process, BigNumber _minUpgrade, int _indexHasMinUpgrade) {
        isUnlock = _isUnlock;
        income = _income;
        process = _process;
        minUpgradeValue = _minUpgrade;
        indexHasMinUpgrade = _indexHasMinUpgrade;
    }
    void ShowPanelRoomInfo(IRoomController room) {
        GameManager.instance.selectedRoom = room;
        CameraMove.instance.ChangePosition(room.GetTransform().position - new Vector3(0, 6.5f, 0f), null);
        UIManager.instance.ShowPanelRoomInfoSpecifiedIndex(room, indexHasMinUpgrade);
    }

    private void Update() {
        btnGoRoom.interactable = gameManager.IsEnoughCash(minUpgradeValue);
        notifyEnableUpgrade.SetActive(btnGoRoom.interactable && process < 1);
    }
}
