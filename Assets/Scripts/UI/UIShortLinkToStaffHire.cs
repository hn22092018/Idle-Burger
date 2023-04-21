using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShortLinkToStaffHire : MonoBehaviour {
    public void OnGoToHireCleaner() {
        //ShowPanelRoomInfo(GameManager.instance.CleanRoom);
    }
    public void OnGoToHireChef() {
        ShowPanelRoomInfo(GameManager.instance.KitchenRoom);
    }
    public void OnGoToHireWaiter() {
        ShowPanelRoomInfo(GameManager.instance.SmallTablesRoom[0]);
    }
    void ShowPanelRoomInfo(IRoomController room) {
        GameManager.instance.selectedRoom = room;
        CameraMove.instance.ChangePosition(room.GetTransform().position - new Vector3(0, 6.5f, 0f), null);
        UIManager.instance.ShowPanelRoomInfo_Staff(room);
    }
}
