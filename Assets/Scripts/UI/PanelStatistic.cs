using DG.Tweening;
using SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelStatistic : UIPanel {
    [SerializeField] Button btnClose;
    [SerializeField] Button btnMap, btnExpan;
    [SerializeField] Transform contentUIRooms;
    [SerializeField] UIRoomStatistic prefab;
    [SerializeField] Image imgCurrentMap, imgNextMap;
    [SerializeField] GameObject grProcess;
    [SerializeField] Text txtCurrentProcess, txtBurgerCoin;
    List<UIRoomStatistic> listUIRooms = new List<UIRoomStatistic>();
    GameManager gameManager;
    bool isCreatedUI;
    int requireBurgerCoin;
    List<RoomID> roomIDs;
    public override void Awake() {
        panelType = UIPanelType.PanelStatistic;
        base.Awake();
        btnClose.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            OnClose();
        });
        btnMap.onClick.AddListener(OnOpenMap);
        btnExpan.onClick.AddListener(OnExpan);
        gameManager = GameManager.instance;
    }

    private void OnExpan() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ScaleEffectButton(btnExpan, () => {
            btnExpan.gameObject.SetActive(false);
            gameManager.OnExpanNewWorld();
        });
    }

    private void OnOpenMap() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ScaleEffectButton(btnMap, () => {

        });
    }

    private void OnEnable() {
        LoadUI();
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() == TutorialStepID.SelectNewWorld) {
            StartCoroutine(IShowTutNewWorld());
        }
    }
    IEnumerator IShowTutNewWorld() {
        yield return new WaitForSeconds(0.5f);
        Tutorials.instance.OnShow(btnExpan.transform);
        btnExpan.onClick.AddListener(() => {
            Tutorials.instance.FinishTutorial();
            Tutorials.instance.OnCloseTutorial();
        });
    }

    void LoadUI() {
        roomIDs = GameManager.instance.GetAllRoomID();
        roomIDs.Sort(SortRoom);
        if (!isCreatedUI) {
            isCreatedUI = true;
            foreach (var id in roomIDs) {
                Transform t = Instantiate(prefab, contentUIRooms).transform;
                t.GetComponent<RectTransform>().sizeDelta = new Vector2(t.GetComponent<RectTransform>().sizeDelta.x, 375f);
                UIRoomStatistic uIRoomStatistic = t.GetComponent<UIRoomStatistic>();
                listUIRooms.Add(uIRoomStatistic);
                uIRoomStatistic.OnSetupRoomID(id);
            }
        }
        for (int i = 0; i < roomIDs.Count; i++) {
            listUIRooms[i].OnSetupRoomID(roomIDs[i]);
        }
        foreach (var room in listUIRooms) {
            room.OnLoadRoomInfo();
        }
        LoadRequireInfos();
    }
    void LoadRequireInfos() {
        int curentLevel = ProfileManager.PlayerData.selectedWorld;
        WorldBaseData worldCurrent = ProfileManager.Instance.dataConfig.worldDataAsset.GetDataByLevel(curentLevel);
        WorldBaseData worldNext = ProfileManager.Instance.dataConfig.worldDataAsset.GetDataByLevel(curentLevel + 1);
        imgCurrentMap.sprite = worldCurrent.restaurantIcon;
        imgNextMap.sprite = worldNext.restaurantIcon;
        requireBurgerCoin = worldNext.burgerRequire;
        txtCurrentProcess.text = (int)(GameManager.instance.mapProcess * 100) + "%";
        int myBurgerCoin = ProfileManager.PlayerData.GetBurgerCoin();
        btnExpan.gameObject.SetActive(false);
        grProcess.SetActive(true);
        if (myBurgerCoin >= requireBurgerCoin) {
            if (GameManager.instance.mapProcess >= 1) {
                grProcess.SetActive(false);
                btnExpan.gameObject.SetActive(true);
            }
            txtBurgerCoin.text = ProfileManager.PlayerData.GetBurgerCoin() + "/" + requireBurgerCoin;
        } else {
            txtBurgerCoin.text = "<Color=#FF3C00>" + ProfileManager.PlayerData.GetBurgerCoin() + "</color>" + " / " + "<Color=#FFFFFF>" + requireBurgerCoin + "</color>";
        }
    }
    int SortRoom(RoomID a, RoomID b) {
        BigNumber minA = GetMinPrice(a);
        BigNumber minB = GetMinPrice(b);
        if (IsRoomUnlock(a) && !IsRoomUnlock(b) && GetUnlockProgress(a) < 1) return -1;
        else if (IsRoomUnlock(a) &&  GetUnlockProgress(a) >= 1) return 1;
        else if (!IsRoomUnlock(a) && IsRoomUnlock(b) && GetUnlockProgress(b) < 1) return 1;
        else if (IsRoomUnlock(b) && GetUnlockProgress(b) >= 1) return -1;
        else if (GetUnlockProgress(a) < 1 && GetUnlockProgress(b) < 1)
            return minA >= minB ? 1 : -1;
        else return 1;
    }
    void OnClose() {
        UIManager.instance.ClosePanelStatistic();
    }
    bool IsRoomUnlock(RoomID roomID) {
        return roomID switch {
            RoomID.Lobby => true,
            RoomID.Table1 => gameManager.IsUnlockSmallTable(0),
            RoomID.Table2 => gameManager.IsUnlockSmallTable(1),
            RoomID.Table3 => gameManager.IsUnlockSmallTable(2),
            RoomID.Table4 => gameManager.IsUnlockSmallTable(3),
            RoomID.Table5 => gameManager.IsUnlockSmallTable(4),
            RoomID.Table6 => gameManager.IsUnlockSmallTable(5),
            RoomID.BigTable1 => gameManager.IsUnlockBigTable(0),
            RoomID.BigTable2 => gameManager.IsUnlockBigTable(1),
            RoomID.BigTable3 => gameManager.IsUnlockBigTable(2),
            RoomID.BigTable4 => gameManager.IsUnlockBigTable(3),
            RoomID.Kitchen => true,
            RoomID.Restroom => gameManager.IsUnlockRestroom(gameManager.WCRooms[0]),
            RoomID.Power => gameManager.IsUnlockPowerRoom(),
            RoomID.Manager => true,
            RoomID.DeliverRoom => gameManager.IsUnlockDeliverRoom(),
            RoomID.BigTable5 => gameManager.IsUnlockBigTable(4),
            RoomID.BigTable6 => gameManager.IsUnlockBigTable(5),
            RoomID.BigTable7 => gameManager.IsUnlockBigTable(6),
            RoomID.BigTable8 => gameManager.IsUnlockBigTable(7),
            RoomID.BigTable9 => gameManager.IsUnlockBigTable(8),
            RoomID.BigTable10 => gameManager.IsUnlockBigTable(9),
            RoomID.BigTable11 => gameManager.IsUnlockBigTable(10),
            RoomID.BigTable12 => gameManager.IsUnlockBigTable(11),
            RoomID.BigTable13 => gameManager.IsUnlockBigTable(12),
            RoomID.BigTable14 => gameManager.IsUnlockBigTable(13),
            RoomID.Restroom2 => gameManager.IsUnlockRestroom(gameManager.WCRooms[1]),
            _ => throw new System.NotImplementedException(),
        };
    }
    float GetUnlockProgress(RoomID roomID) {
        return roomID switch {
            RoomID.Lobby => gameManager.LobbyRoom.GetProcessUpgrade(),
            RoomID.Table1 => gameManager.SmallTablesRoom[0].GetProcessUpgrade(),
            RoomID.Table2 => gameManager.SmallTablesRoom[1].GetProcessUpgrade(),
            RoomID.Table3 => gameManager.SmallTablesRoom[2].GetProcessUpgrade(),
            RoomID.Table4 => gameManager.SmallTablesRoom[3].GetProcessUpgrade(),
            RoomID.Table5 => gameManager.SmallTablesRoom[4].GetProcessUpgrade(),
            RoomID.Table6 => gameManager.SmallTablesRoom[5].GetProcessUpgrade(),
            RoomID.BigTable1 => gameManager.BigTablesRoom[0].GetProcessUpgrade(),
            RoomID.BigTable2 => gameManager.BigTablesRoom[1].GetProcessUpgrade(),
            RoomID.BigTable3 => gameManager.BigTablesRoom[2].GetProcessUpgrade(),
            RoomID.BigTable4 => gameManager.BigTablesRoom[3].GetProcessUpgrade(),
            RoomID.BigTable5 => gameManager.BigTablesRoom[4].GetProcessUpgrade(),
            RoomID.BigTable6 => gameManager.BigTablesRoom[5].GetProcessUpgrade(),
            RoomID.BigTable7 => gameManager.BigTablesRoom[6].GetProcessUpgrade(),
            RoomID.BigTable8 => gameManager.BigTablesRoom[7].GetProcessUpgrade(),
            RoomID.BigTable9 => gameManager.BigTablesRoom[8].GetProcessUpgrade(),
            RoomID.BigTable10 => gameManager.BigTablesRoom[9].GetProcessUpgrade(),
            RoomID.BigTable11 => gameManager.BigTablesRoom[10].GetProcessUpgrade(),
            RoomID.BigTable12 => gameManager.BigTablesRoom[11].GetProcessUpgrade(),
            RoomID.BigTable13 => gameManager.BigTablesRoom[12].GetProcessUpgrade(),
            RoomID.BigTable14 => gameManager.BigTablesRoom[13].GetProcessUpgrade(),
            RoomID.Kitchen => gameManager.KitchenRoom.GetProcessUpgrade(),
            RoomID.Restroom => gameManager.WCRooms[0].GetProcessUpgrade(),
            RoomID.Restroom2 => gameManager.WCRooms[1].GetProcessUpgrade(),
            RoomID.Power => gameManager.PowerRoom.GetProcessUpgrade(),
            RoomID.DeliverRoom => gameManager.DeliverRoom.GetProcessUpgrade(),
            _ => throw new System.NotImplementedException(),
        };
    }
    BigNumber GetMinPrice(RoomID roomID) {
        switch (roomID) {
            case RoomID.Lobby:
                gameManager.LobbyRoom.CalculateMinUpgrade();
                return gameManager.LobbyRoom.GetMinUpgradeValue();
            case RoomID.Table1:
            case RoomID.Table2:
            case RoomID.Table3:
            case RoomID.Table4:
            case RoomID.Table5:
            case RoomID.Table6:
                gameManager.SmallTablesRoom[(int)roomID - 1].CalculateMinUpgrade();
                return gameManager.SmallTablesRoom[(int)roomID - 1].GetMinUpgradeValue();
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
                gameManager.BigTablesRoom[(int)roomID - 7].CalculateMinUpgrade();
                return gameManager.BigTablesRoom[(int)roomID - 7].GetMinUpgradeValue();
            case RoomID.Kitchen:
                gameManager.KitchenRoom.CalculateMinUpgrade();
                return gameManager.KitchenRoom.GetMinUpgradeValue();
            case RoomID.Restroom:
            case RoomID.Restroom2:
                gameManager.WCRooms[(int)roomID - 22].CalculateMinUpgrade();
                return gameManager.WCRooms[(int)roomID - 22].GetMinUpgradeValue();
            case RoomID.Power:
                gameManager.PowerRoom.CalculateMinUpgrade();
                return gameManager.PowerRoom.GetMinUpgradeValue();
            case RoomID.DeliverRoom:
                gameManager.DeliverRoom.CalculateMinUpgrade();
                return gameManager.DeliverRoom.GetMinUpgradeValue();
        }
        return new BigNumber(0);
    }
}

