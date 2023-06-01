using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabStaffInfo : MonoBehaviour {
    [SerializeField] private Image imgStaffIcon;
    [SerializeField] private Text txtStaffName, txtStaffDescription, txtStaffPrice;
    [SerializeField] private Text txtAmount, txtSalary, txtTotalSalary;
    [SerializeField] private Button btnHire;
    BaseStaffSetting setting;
    [SerializeField] private int salary, maxStaff;
    [SerializeField] private float price;
    private void Awake() {
        btnHire.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            OnHire();
        });
    }
    public void Setup(BaseStaffSetting baseStaffSetting) {
        setting = baseStaffSetting;
        imgStaffIcon.sprite = setting.staffData.sprUIStaffBig;
        txtStaffName.text = setting.staffData.GetName();
        txtStaffDescription.text = setting.staffData.GetDes();
        maxStaff = setting.GetTotalStaffAvaiableHire();
        //salary = GameManager.instance.GetSalaryForPerStaff(baseStaffSetting);
        txtSalary.text = "-" + salary + "/h";
        price = setting.staffData.costPerStaff;
        txtStaffPrice.text = price.ToString();
        LoadStaffInfo2();
    }
    private void Update() {
        btnHire.interactable = GameManager.instance.IsEnoughCash(price);
    }
    void LoadStaffInfo2() {
        int totalStaffCurrent = setting.GetTotalStaffCurrent();
        txtAmount.text = totalStaffCurrent + "/" + maxStaff;
        txtTotalSalary.text = "-" + (salary * totalStaffCurrent) + "/h"; ;
        btnHire.gameObject.SetActive(totalStaffCurrent < maxStaff);
    }
    void OnHire() {
        if (GameManager.instance.IsEnoughCash(price)) {
            ProfileManager.PlayerData.ConsumeCash(price);
            setting.OnHireStaff();
            ProfileManager.Instance.playerData.SaveStaffData(setting.config);
            LoadStaffInfo2();
            PanelManagerStaff.instance.OnReloadStaff();
        }
    }
}
