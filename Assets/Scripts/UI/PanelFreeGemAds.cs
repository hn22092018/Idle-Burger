using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelFreeGemAds : UIPanel {
    int gemValue;
    public Text txtValue;
    public Button btnAds, btnClose, btnTicket;
    public override void Awake() {
        panelType = UIPanelType.PanelFreeGemAds;
        base.Awake();
        btnAds.onClick.AddListener(OnGetGemAds);
        btnClose.onClick.AddListener(OnClose);
        btnTicket.onClick.AddListener(OnGetGemTicket);
    }
    private void OnEnable() {
        gemValue = GameManager.instance.GetFreeGemAdsProfit();
        txtValue.text = "+" + gemValue;
        btnTicket.gameObject.SetActive(ProfileManager.PlayerData.ResourceSave.GetADTicket() > 0);
    }
    private void Update() {
        btnAds.interactable = AdsManager.Instance.IsRewardVideoLoaded();
    }
    private void OnGetGemAds() {
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.FreeGem.ToString(), OnGetGemAdsSuccess);
        else OnGetGemAdsSuccess();
    }

    private void OnGetGemAdsSuccess() {
        ProfileManager.PlayerData.AddGem(gemValue);
        ABIAnalyticsManager.Instance.TrackEventGem(GemAction.Earn_Ads, gemValue);
        OnClose();
    }
    void OnGetGemTicket() {
        ProfileManager.PlayerData.ResourceSave.ConsumeADTicket();
        ProfileManager.PlayerData.AddGem(gemValue);
        OnClose();
    }
    private void OnClose() {
        UIManager.instance.ClosePanelFreeGemAds();
    }
}
