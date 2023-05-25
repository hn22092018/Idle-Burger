using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using SDK;

public class WareHouseDetailPanel : MonoBehaviour {
    [SerializeField] Text txtTitle;
    [SerializeField] Text txtPrice;
    [SerializeField] Text txtTimeCreateCoolDown;
    [SerializeField] Text txtCoolDownPrice;
    [SerializeField] Image imgIcon;
    [SerializeField] Button btnSale;
    [SerializeField] Button btnGemPay;
    [SerializeField] float timeShowSetting = 5f;
    [SerializeField] CanvasGroup bgCanvas;
    [SerializeField] GameObject objCreateCoolDown;
    [SerializeField] GameObject objTimeCoolDownWrap;
    [SerializeField] RectTransform rectGemPriceRebuild;
    float timeShow;
    int level;
    int price;
    int countCreateDetail;
    TimeSpan timeTarget;
    float timeCreateCoolDown;
    bool onShowCreate;
    WareHouseSlotMaterial currentWareHouseSlot;
    WareHouseMaterialType materialType;
    PanelWareHouse panelWareHouse;
    string sLevel = "Level";
    private void Start() {
        btnSale.onClick.AddListener(OnSale);
        btnGemPay.onClick.AddListener(OnPayGem);
        panelWareHouse = UIManager.instance.GetPanel(UIPanelType.PanelWareHouse).GetComponent<PanelWareHouse>();
    }
    private void Update() {
        if (onShowCreate)
            OnShowCreateDetail();
    }
    public void ShowDetail(WareHouseSlotMaterial wareHouseSlotMaterial, WareHouseMaterial wareHouseMaterial, int level) {
        sLevel = ProfileManager.Instance.dataConfig.GameText.GetTextByID(27);
        onShowCreate = false;
        objCreateCoolDown.SetActive(false);
        txtTitle.text = wareHouseMaterial.wareHouseMaterialType.ToString();
        txtTitle.text += " (" + sLevel+" " + level.ToString() + ")";
        txtPrice.text = "+" + wareHouseMaterial.priceSale.ToString();
        imgIcon.sprite = wareHouseMaterial.icon;
        timeShow = timeShowSetting;
        bgCanvas.alpha = 0f;
        bgCanvas.DOFade(1f, .25f);
        this.level = level;
        this.materialType = wareHouseMaterial.wareHouseMaterialType;
        this.price = wareHouseMaterial.priceSale;
        this.currentWareHouseSlot = wareHouseSlotMaterial;
        btnSale.gameObject.SetActive(true);
        btnSale.interactable = true;
       
    }
    public void CloseDetail() {
        btnSale.interactable = false;
        bgCanvas.DOFade(0f, .25f).OnComplete(() => {
            gameObject.SetActive(false);
        });
    }
    void OnSale() {
        ProfileManager.PlayerData.wareHouseManager.RemoveWareHouseMaterialSaves(materialType, level);
        currentWareHouseSlot.OnDeActivate();
        panelWareHouse.CheckDuplicate();
        ProfileManager.PlayerData.researchManager.AddResearchValue(price);
        CloseDetail();
    }
    void OnPayGem() {
        if (GameManager.instance.IsEnoughGem(price)) {
            ProfileManager.PlayerData.wareHouseManager.ResetCountCreate(materialType);
            ProfileManager.PlayerData.ConsumeGem(price);
            ABIAnalyticsManager.Instance.TrackEventWareHouse(WareHouseAction.UseGem_SkipTime);
            ABIAnalyticsManager.Instance.TrackEventGem(GemAction.Spend_WareHouse_MoreEnergy, price);
            gameObject.SetActive(false);
        } 
    }
    void OnShowCreateDetail() {
        countCreateDetail = ProfileManager.PlayerData.wareHouseManager.GetCountCreate(materialType);
        if (countCreateDetail == 0) {
            timeTarget = DateTime.Parse(ProfileManager.PlayerData.wareHouseManager.GetTargetTime(materialType)).Subtract(DateTime.Now);
            if (timeTarget > TimeSpan.Zero) {
                objTimeCoolDownWrap.SetActive(true);
                timeCreateCoolDown = (float)timeTarget.TotalSeconds;
                txtTimeCreateCoolDown.text = TimeUtil.TimeToString(timeCreateCoolDown);
                price = (int)(timeCreateCoolDown / 10);
                txtCoolDownPrice.text = price.ToString();
            } else objTimeCoolDownWrap.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectGemPriceRebuild);
        } else {
            objTimeCoolDownWrap.SetActive(false);
            objCreateCoolDown.SetActive(false);
            onShowCreate = false;
        }
    }
    public void ShowDetailCreateMaterial(WareHouseMaterialType wareHouseMaterialType, Sprite icon) {
        this.materialType = wareHouseMaterialType;
        objCreateCoolDown.SetActive(true);
        onShowCreate = true;
        btnSale.gameObject.SetActive(false);
        timeShow = timeShowSetting;
        bgCanvas.alpha = 0f;
        bgCanvas.DOFade(1f, .25f);
        txtTitle.text = materialType.ToString();
        imgIcon.sprite = icon;
    }
}
