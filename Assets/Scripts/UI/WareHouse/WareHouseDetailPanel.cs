using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using SDK;

public class WareHouseDetailPanel : MonoBehaviour {
    [SerializeField] Text txtPrice;
    [SerializeField] Image imgIcon;
    [SerializeField] Button btnSale;
    [SerializeField] float timeShowSetting = 5f;
    [SerializeField] CanvasGroup bgCanvas;
    int level;
    int price;
    WareHouseSlotMaterial currentWareHouseSlot;
    WareHouseMaterialType materialType;
    PanelWareHouse panelWareHouse;
    string sLevel = "Level";
    private void Start() {
        btnSale.onClick.AddListener(OnSale);
        panelWareHouse = UIManager.instance.GetPanel(UIPanelType.PanelWareHouse).GetComponent<PanelWareHouse>();
    }

    public void ShowDetail(WareHouseSlotMaterial wareHouseSlotMaterial, WareHouseMaterial wareHouseMaterial, int level) {
        sLevel = ProfileManager.Instance.dataConfig.GameText.GetTextByID(27);
        txtPrice.text = "+" + wareHouseMaterial.priceSale.ToString();
        imgIcon.sprite = wareHouseMaterial.icon;
        bgCanvas.alpha = 1f;
        bgCanvas.DOFade(0f, .25f).SetDelay(timeShowSetting).OnComplete(() => {
            btnSale.interactable = false;
            gameObject.SetActive(false);
        });
        this.level = level;
        this.materialType = wareHouseMaterial.wareHouseMaterialType;
        this.price = wareHouseMaterial.priceSale;
        this.currentWareHouseSlot = wareHouseSlotMaterial;
        btnSale.gameObject.SetActive(true);
        btnSale.interactable = true;
       
    }
     void CloseDetail() {
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
   
 
  
}
