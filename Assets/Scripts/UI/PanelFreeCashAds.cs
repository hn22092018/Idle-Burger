using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelFreeCashAds : UIPanel {
    BigNumber cashValue;
    public Text txtValue;
    public Button btnAds, btnClose, btnTicket;
    public override void Awake() {
        panelType = UIPanelType.PanelFreeCashAds;
        base.Awake();
        btnAds.onClick.AddListener(OnGetCashAds);
        btnClose.onClick.AddListener(OnSkip);
        btnTicket.onClick.AddListener(OnGetCashTicket);
    }
    private void OnEnable() {
        cashValue = GameManager.instance.GetFreeCashAdsProfit();
        txtValue.text = "+" + cashValue.IntToString();
        btnTicket.gameObject.SetActive(ProfileManager.PlayerData.ResourceSave.GetADTicket() > 0);
    }
  
    private void OnGetCashAds() {
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.FreeCash.ToString(), OnGetCashAdsSuccess);
        else OnGetCashAdsSuccess();
    }
    private void OnGetCashAdsSuccess() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        UIManager.instance.ShowUIMoneyProfit(cashValue);
        GameManager.instance.AddCash(cashValue);
        GameManager.instance.ResetFreeCashRate();
        OnClose();
    }
    void OnGetCashTicket() {
        ProfileManager.PlayerData.ResourceSave.ConsumeADTicket();
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        UIManager.instance.ShowUIMoneyProfit(cashValue);
        GameManager.instance.AddCash(cashValue);
        GameManager.instance.ResetFreeCashRate();
        OnClose();
    }
    private void OnClose() {
        UIManager.instance.ClosePanelFreeCashAds();
    }
    private void OnSkip() {
        GameManager.instance.IncreaseFreeCashRate();
        UIManager.instance.ClosePanelFreeCashAds();
    }
}
