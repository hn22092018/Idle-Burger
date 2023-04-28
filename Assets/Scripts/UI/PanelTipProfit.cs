using SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelTipProfit : UIPanel {
    [SerializeField] Text txtProfitReceptionist;
    [SerializeField] Text txtProfitWatier;
    [SerializeField] Text txtProfitChef;
    [SerializeField] Text txtProfitCleaner;
    [SerializeField] Text txtTotal;
    [SerializeField] Button btnClaim, btnClaimX2, btnGoToCard, btnClose, btnClaimX2Ticket;
    BigNumber profits;
    public override void Awake() {
        panelType = UIPanelType.PanelTipProfit;
        base.Awake();
        btnClaim.onClick.AddListener(OnClaim);
        btnClaimX2.onClick.AddListener(OnClaimVideo);
        btnClaimX2Ticket.onClick.AddListener(OnClaimX2Ticket);
        btnGoToCard.onClick.AddListener(GoToPanelCard);
        btnClose.onClick.AddListener(OnClose);
    }
    private void Update() {
        txtProfitReceptionist.text = "+" + ProfileManager.PlayerData.GetTipReception();
        txtProfitWatier.text = "+" + ProfileManager.PlayerData.GetTipWaiter();
        txtProfitChef.text = "+" + ProfileManager.PlayerData.GetTipChef();
        txtProfitCleaner.text = "+" + ProfileManager.PlayerData.GetTipCleaner();
        txtTotal.text= "+" + ProfileManager.Instance.playerData.GetTotalTipProfit();
        btnClaimX2.interactable = AdsManager.Instance.IsRewardVideoLoaded();
        btnClaimX2Ticket.gameObject.SetActive(ProfileManager.PlayerData.ResourceSave.GetADTicket() > 0);
    }

    void OnClaim() {
        SoundManager.instance.PlaySoundEffect(SoundID.CASH_COLLECT);
        profits = ProfileManager.Instance.playerData.GetTotalTipProfit();
        ProfileManager.PlayerData.AddCash(profits);
        UIManager.instance.ShowUIMoneyProfit(profits);
        ProfileManager.Instance.playerData.ResetTipProfit();
        OnClose();
    }
    void OnClaimVideo() {
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.X2_Tip.ToString(), OnClaimVideoSuccess);
        else OnClaimVideoSuccess();
    }
    void OnClaimVideoSuccess() {
        SoundManager.instance.PlaySoundEffect(SoundID.CASH_COLLECT);
        profits = ProfileManager.Instance.playerData.GetTotalTipProfit();
        ProfileManager.PlayerData.AddCash(profits * 2);
        UIManager.instance.ShowUIMoneyProfit(profits * 2);
        ProfileManager.Instance.playerData.ResetTipProfit();
        OnClose();
    }
    void OnClaimX2Ticket() {
        SoundManager.instance.PlaySoundEffect(SoundID.CASH_COLLECT);
        ProfileManager.PlayerData.ResourceSave.ConsumeADTicket();
        profits = ProfileManager.Instance.playerData.GetTotalTipProfit();
        ProfileManager.PlayerData.AddCash(profits * 2);
        UIManager.instance.ShowUIMoneyProfit(profits * 2);
        ProfileManager.Instance.playerData.ResetTipProfit();
        OnClose();
    }
    void GoToPanelCard() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        UIManager.instance.CloseAllPopup();
        UIManager.instance.ShowPanelCardCollection();
    }
    public void OnClose() {
        UIManager.instance.ClosePanelTipProfit();
    }
}
