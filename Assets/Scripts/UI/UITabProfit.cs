using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabProfit : MonoBehaviour {
    [SerializeField] GameObject grMoney, grTime, grEnergy;
    [SerializeField] Text txtTotalMoney, txtTimeService, txtEnergy;
    [SerializeField] RectTransform rectTransform;
    float timeRebuildLayout = 0;
    public void Setup() {
        IRoomController room = GameManager.instance.selectedRoom;
        timeRebuildLayout = 0;
       
        if (room.GetRoomID() == RoomID.Power) {
            grMoney.SetActive(false);
            grTime.SetActive(false);
            grEnergy.SetActive(true);
            txtEnergy.text = room.GetTotalEnergyEarn().ToString();
        } else {
            grMoney.SetActive(room.GetTotalMoneyEarn() > 0);
            grTime.SetActive(room.GetTimeServiceCurrent() > 0);
            grEnergy.SetActive(false);
            txtTotalMoney.text = room.GetTotalMoneyEarn().ToString();
            txtTimeService.text = room.GetTimeServiceCurrent().ToString("0.00") + "s";
            //txtEnergy.text = room.GetTotalEnergyRequireCurrent().ToString();
        }

    }
    private void Update() {
        if (timeRebuildLayout <= 1) {
            timeRebuildLayout += Time.deltaTime;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }
    }
}
