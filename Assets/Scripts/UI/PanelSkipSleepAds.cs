using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSkipSleepAds : UIPanel
{
    public Button btnAds, btnClose, btnTicket;
    public override void Awake() {
        panelType = UIPanelType.PanelSkipSleepAds;
        base.Awake();
        btnAds.onClick.AddListener(OnSKipAds);
        btnClose.onClick.AddListener(OnClose);
        btnTicket.onClick.AddListener(OnSkipTicket);
    }
    private void OnEnable() {
        btnTicket.gameObject.SetActive(ProfileManager.PlayerData.ResourceSave.GetADTicket() > 0);
    }
    private void OnSKipAds() {
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.SkipSleep.ToString(), OnSkipAdsSuccess);
        else OnSkipAdsSuccess();
    }

    private void OnSkipAdsSuccess() {
        GameManager.instance.IsSkipSleep = true;
        OnClose();
    }
    void OnSkipTicket() {
        GameManager.instance.IsSkipSleep = true;
        ProfileManager.PlayerData.ResourceSave.ConsumeADTicket();
        OnClose();
    }
    private void OnClose() {
        UIManager.instance.ClosePanelSkipSleepAds();
    }
}
