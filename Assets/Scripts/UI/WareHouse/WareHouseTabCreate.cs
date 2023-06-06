using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WareHouseTabCreate : MonoBehaviour
{
    public WareHouseMaterialType materialType;
    [SerializeField] Text txtName;
    [SerializeField] Button btnCreate;
    [SerializeField] PanelWareHouse myWareHousePanel;
    [SerializeField] Text txtTimeCoolDown;
    [SerializeField] GameObject objTimeWrap;
    [SerializeField] Image imgProcess;
    [SerializeField] Image imgIcon;
    [SerializeField] GameObject objBlurDeactive;
    [SerializeField] UIButtonEffect uIButtonEffect;
    int countCreate;
    int countMax;
    string sOnCoolDown = "On CoolDown...";
    string sOutOfChance = "Out of Chance...";
    string sOutOfEnergy = "Out of Energy...";
    private void Awake()
    {
        btnCreate.onClick.AddListener(CreateMaterial);
        txtName.text = materialType.ToString();
    }
    private void OnEnable() {
        sOnCoolDown = ProfileManager.Instance.dataConfig.GameText.GetTextByID(222);
        sOutOfChance = ProfileManager.Instance.dataConfig.GameText.GetTextByID(223);
        sOutOfEnergy = ProfileManager.Instance.dataConfig.GameText.GetTextByID(224);
    }
    private void Start()
    {
        countMax = ProfileManager.PlayerData.wareHouseManager.GetMaxCount();
        if (ProfileManager.PlayerData.wareHouseManager.GetTargetTime(materialType) == "")
        {
            ProfileManager.PlayerData.wareHouseManager.ChangeCountCreate(materialType, countMax);
            return;
        }
        DateTime timeTarget = DateTime.Parse(ProfileManager.PlayerData.wareHouseManager.GetTargetTime(materialType));
        float ftimeTarget = (float)DateTime.Now.Subtract(timeTarget).TotalSeconds;
        ProfileManager.PlayerData.wareHouseManager.ChangeCountCreate(materialType, (int)(ftimeTarget / 180f));
    }
    void CreateMaterial() {
        if (!ProfileManager.PlayerData.wareHouseManager.IsHaveEnergy())
        {
            CreateFaildByEnergy();
            return;
        }
        if (countCreate <= 0)
        {
            myWareHousePanel.ShowDetailCreateMaterial(materialType, imgIcon.sprite);
            CreateFaildByCoolDown();
            return;
        }
        myWareHousePanel.AddMaterial(materialType, CreateCompleted, CreateFaildByOutOfSlot);
    }
    void CreateCompleted()
    {
        ProfileManager.PlayerData.wareHouseManager.UsingEnergy();
        ProfileManager.PlayerData.wareHouseManager.ChangeCountCreate(materialType, -1);
        if (ProfileManager.PlayerData.wareHouseManager.GetCountCreate(materialType) == 0)
            ProfileManager.PlayerData.wareHouseManager.SetTargetTime(materialType);
    }
    void CreateFaildByCoolDown() {
        myWareHousePanel.CreateFailEffect(sOnCoolDown);
        Debug.Log("On CoolDown!");
    }
    void CreateFaildByOutOfSlot() {
        myWareHousePanel.CreateFailEffect(sOutOfChance);
        Debug.Log("Out of change!");
    }
    void CreateFaildByEnergy()
    {
        myWareHousePanel.CreateFailEffect(sOutOfEnergy);
        Debug.Log("Out of energy!");
    }
    private void Update()
    {
        countCreate = ProfileManager.PlayerData.wareHouseManager.GetCountCreate(materialType);
        if (countCreate == 0)
        {
            DateTime timeTarget = DateTime.Parse(ProfileManager.PlayerData.wareHouseManager.GetTargetTime(materialType));
            float ftimeTarget = 0;
            if (timeTarget.Subtract(DateTime.Now) > TimeSpan.Zero)
            {
                ftimeTarget = (float)timeTarget.Subtract(DateTime.Now).TotalSeconds;
                txtTimeCoolDown.text = TimeUtil.TimeToString(ftimeTarget);
                imgProcess.fillAmount = 1 - ftimeTarget / 180f;
            }
            else {
                ProfileManager.PlayerData.wareHouseManager.ChangeCountCreate(materialType, countMax - countCreate);
            }
            objTimeWrap.SetActive(true);
            objBlurDeactive.SetActive(true);
        }
        else { 
            objTimeWrap.SetActive(false);
            objBlurDeactive.SetActive(false);
        }
    }
}
