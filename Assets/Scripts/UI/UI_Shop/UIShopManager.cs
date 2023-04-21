using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using System;

public class UIShopManager : UIPanel {
    public static UIShopManager instance;
    public Button closeButton;
    public RectTransform rectReBuild;
    public UIShopTabGroup tabGroup;
    public UIScroll scrollManager;
    public Button btnRestorePurchase;
    public override void Awake() {
        instance = this;
        panelType = UIPanelType.PanelShop;
        base.Awake();
        closeButton.onClick.AddListener(OnClose);
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer) {
            btnRestorePurchase.gameObject.SetActive(true);
            btnRestorePurchase.onClick.AddListener(RestorePurchase);
        }
        else btnRestorePurchase.gameObject.SetActive(false);
    }
    private void Start() {
        ReBuildUI();
    }
    public void RestorePurchase() {
        MyIAPManager.instance.RestorePurchases();
    }
    public void ReBuildUI() {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectReBuild);
    }
    public void OnClose() {
        scrollManager.ResetScroll();
        tabGroup.OnSelected(tabGroup.listTabs[0], false);
        UIManager.instance.ClosePanelShop();
    }
}
