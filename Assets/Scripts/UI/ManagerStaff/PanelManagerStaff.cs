using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManagerStaff : UIPanel {
    public static PanelManagerStaff instance;
    [SerializeField] private Transform rootContent;
    [SerializeField] private Transform prefabUIStaffInfo;
    List<BaseStaffSetting> settings = new List<BaseStaffSetting>();
    [SerializeField] private Button btnClose;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] Text txtMember, txtSalary;
    public override void Awake() {
        instance = this;
        panelType = UIPanelType.PanelManagerStaff;
        base.Awake();
        btnClose.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            OnClose();
        });
    }

    public void OnShow() {
        settings.Clear();
        //settings = GameManager.instance.GetAllStaffSettingInRoom();
        while (rootContent.childCount < settings.Count) {
            Instantiate(prefabUIStaffInfo, rootContent);
        }
        for (int i = 0; i < rootContent.childCount; i++) {
            if (i < settings.Count) {
                Transform t = rootContent.GetChild(i);
                t.GetComponent<UITabStaffInfo>().Setup(settings[i]);
                t.gameObject.SetActive(true);
            } else rootContent.GetChild(i).gameObject.SetActive(false);
        }
        int totalStaff = 0;
        int maxStaff = 0;
        for (int i = 0; i < settings.Count; i++) {
            totalStaff += settings[i].GetTotalStaffCurrent();
            maxStaff += settings[i].GetTotalStaffAvaiableHire();
        }
        txtMember.text = totalStaff + "/" + maxStaff;
        //txtSalary.text = GameManager.instance.salaryPerHour.ToString() + "/H";
    }
    public void OnReloadStaff() {
        settings.Clear();
        //settings = GameManager.instance.GetAllStaffSettingInRoom();
        int totalStaff = 0;
        int maxStaff = 0;
        for (int i = 0; i < settings.Count; i++) {
            totalStaff += settings[i].GetTotalStaffCurrent();
            maxStaff += settings[i].GetTotalStaff();
        }
        txtMember.text = totalStaff + "/" + maxStaff;
        //txtSalary.text = GameManager.instance.salaryPerHour.ToString() + "/H";
    }
    public void OnClose() {
        UIManager.instance.ClosePanelManagerStaff();
    }
}
