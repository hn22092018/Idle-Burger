using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabStaff : MonoBehaviour {
    public Text txtStaffName;
    public List<Image> imgStaffIcons;
    public List<GameObject> imgSwapStaffIcons;
    public Color[] colorStaffIcons;
    //public Text txtSalary, txtTotalSalary;
    //int salary, cost;
    int totalStaff, totalStaffCurrent;
    //[SerializeField] GameObject panelWarningChef;
    [SerializeField] RectTransform contentRebuild;
    IRoomController room;
    float timeRebuildLayout = 0;
    [HideInInspector]
    public bool isHiring;
    [HideInInspector] public int _SelectedIndexStaff;
   
    public void Setup() {
        isHiring = false;
        room = GameManager.instance.selectedRoom;
        txtStaffName.text = room.GetStaffSetting().staffData.GetName().ToUpper();
        totalStaff = room.GetStaffSetting().GetTotalStaff();
        totalStaffCurrent = room.GetStaffSetting().GetTotalStaffCurrent();
        for (int i = 0; i < imgStaffIcons.Count; i++) {
            Sprite sprDefault = ProfileManager.PlayerData.skinManager.GetListSkinConfigByStaffType(room.GetStaffSetting().config.staffID)[0].skinIcon;
            Sprite sprSaver = ProfileManager.PlayerData.skinManager.GetSpriteIconFromSaver(room.GetStaffSetting().config.staffID, i);
            imgStaffIcons[i].sprite = sprSaver != null ? sprSaver : sprDefault;
            imgStaffIcons[i].gameObject.SetActive(i < totalStaff);
            imgStaffIcons[i].transform.parent.gameObject.SetActive(i < totalStaff);
            imgStaffIcons[i].color = colorStaffIcons[i < totalStaffCurrent ? 0 : 1];
        }
        for (int i = 0; i < imgSwapStaffIcons.Count; i++) {
            imgSwapStaffIcons[i].SetActive(i < totalStaffCurrent ? true : false);
        }
            //cost = room.GetStaffSetting().staffData.costPerStaff;
        //salary = room.GetStaffSetting().staffData.salaryPerStaff;
        //txtSalary.text = salary.ToString();
        //txtTotalSalary.text = (salary * totalStaffCurrent).ToString();
        //btnHire.gameObject.SetActive(totalStaffCurrent < room.GetStaffSetting().GetTotalStaffAvaiableHire());
        timeRebuildLayout = 0;

    }
    private void Update() {
        if (timeRebuildLayout < 1) {
            timeRebuildLayout += Time.deltaTime;
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRebuild);
        }
    }
    public void CheckOnHire() {
        //if (!isHiring) return;
        //isHiring = false;
        //IRoomController roomControl = GameManager.instance.selectedRoom;
        //roomControl.OnHireStaff();
        //totalStaffCurrent = roomControl.GetStaffSetting().GetTotalStaffCurrent();
        //for (int i = 0; i < imgStaffIcons.Count; i++) {
        //    imgStaffIcons[i].color = colorStaffIcons[i < totalStaffCurrent ? 0 : 1];
        //}
        //for (int i = 0; i < imgSwapStaffIcons.Count; i++) {
        //    imgSwapStaffIcons[i].SetActive(i < totalStaffCurrent ? true : false);
        //}
        //txtTotalSalary.text = (salary * totalStaffCurrent).ToString();
        //btnHire.gameObject.SetActive(totalStaffCurrent < room.GetStaffSetting().GetTotalStaffAvaiableHire());
        //PanelRoomInfo.instance.SetupTabProfit();
        //if (isShowTut) ProfileManager.PlayerData.SaveData();
    }

    public void ShowPanelSelectSkin(int indexStaff) {
        _SelectedIndexStaff = indexStaff;
        if (indexStaff >= totalStaffCurrent) return;
        UIManager.instance.ShowPanelSkin(room.GetStaffSetting().config.staffID);
    }
    public void UpdateSkinUI(int indexStaff) {
        Sprite sprDefault = ProfileManager.PlayerData.skinManager.GetListSkinConfigByStaffType(room.GetStaffSetting().config.staffID)[0].skinIcon;
        Sprite sprSaver = ProfileManager.PlayerData.skinManager.GetSpriteIconFromSaver(room.GetStaffSetting().config.staffID, indexStaff);
        imgStaffIcons[indexStaff].sprite = sprSaver != null ? sprSaver : sprDefault;
        imgSwapStaffIcons[indexStaff].SetActive(true);
    }
}
