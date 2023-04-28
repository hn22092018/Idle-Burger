using DG.Tweening;
using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelStatistic : UIPanel {
    [SerializeField] Button btnClose;
    [SerializeField] Transform contentUIRooms;
    [SerializeField] UIRoomStatistic prefab;
    List<UIRoomStatistic> listUIRooms = new List<UIRoomStatistic>();
    GameManager gameManager;
    bool isCreatedUI;

    public override void Awake() {
        panelType = UIPanelType.PanelStatistic;
        base.Awake();
        btnClose.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            OnClose();
        });
        gameManager = GameManager.instance;
    }
    private void OnEnable() {
        LoadUI();
    }

    void LoadUI() {
        List<RoomID> roomIDs = GameManager.instance.GetAllRoomID();
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
    }
    int SortRoom(RoomID a, RoomID b) {
        if (IsRoomUnlock(a) && !IsRoomUnlock(b) && GetUnlockProgress(a) < 1) return -1;
        else if (IsRoomUnlock(a) && !IsRoomUnlock(b) && GetUnlockProgress(a) >= 1) return 1;
        else if (!IsRoomUnlock(a) && IsRoomUnlock(b) && GetUnlockProgress(b) < 1) return 1;
        else if (!IsRoomUnlock(a) && IsRoomUnlock(b) && GetUnlockProgress(b) >= 1) return -1;
        else if (IsRoomUnlock(a) && IsRoomUnlock(b)) {
            if (GetUnlockProgress(a) >= 1) return 1;
            else if (GetUnlockProgress(b) >= 1) return -1;
            else if (GetUnlockProgress(a) >= GetUnlockProgress(b)) return -1;
            else return 1;
        }
        return gameManager.buildData.GetData(a).starRequire >= gameManager.buildData.GetData(b).starRequire ? 1 : -1;
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
            RoomID.Kitchen => gameManager.KitchenRoom.GetProcessUpgrade(),
            RoomID.Restroom => gameManager.WCRooms[0].GetProcessUpgrade(),
            RoomID.Power => gameManager.PowerRoom.GetProcessUpgrade(),
            RoomID.Manager => gameManager.ManagerRoom.GetProcessUpgrade(),
            RoomID.DeliverRoom => gameManager.DeliverRoom.GetProcessUpgrade(),
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
            RoomID.Restroom2 => gameManager.WCRooms[1].GetProcessUpgrade(),
            _ => throw new System.NotImplementedException(),
        };
    }
   
}
