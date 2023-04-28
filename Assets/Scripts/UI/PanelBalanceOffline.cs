using SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelBalanceOffline : UIPanel {
    [SerializeField] Button btnX2, btnX3, btnMoreCard, btnX2Ticket;
    [SerializeField] Button btnX2_Reputation, btnX3_Reputation, btnX2Ticket_Reputation;
    [SerializeField] Button bntContinueGame;
    [SerializeField] Text txtProfitOffline, txtTotalReputation;
    [SerializeField] Text txtHourLimitedMax, txtOfflineTime;
    int gemPriceX3 = 100;
    BigNumber profit;
    int reputation;
    bool isCollectBonusCash, isCollectBonusReputation;
    public override void Awake() {
        panelType = UIPanelType.PanelBalanceOffline;
        base.Awake();
        btnX2.onClick.AddListener(OnX2);
        btnX2Ticket.onClick.AddListener(OnX2Ticket);
        btnX3.onClick.AddListener(OnX3);

        btnX2_Reputation.onClick.AddListener(OnX2_Reputation);
        btnX2Ticket_Reputation.onClick.AddListener(OnX2Ticket_Reputation);
        btnX3_Reputation.onClick.AddListener(OnX3_Reputation);

        btnMoreCard.onClick.AddListener(OpenShop);
        bntContinueGame.onClick.AddListener(OnDefaultClaim);
    }
    private void OnEnable() {
        bntContinueGame.gameObject.SetActive(true);
        profit = GameManager.instance.GetOfflineProfit();
        reputation = GameManager.instance.GetTotalReputationOffline();
        txtTotalReputation.text = "+" + reputation;
        txtProfitOffline.text = "+" +profit.ToString();
        int hourOfflineMax = ProfileManager.PlayerData.GetCardManager().GetExtraHour_OfflineTimeCardIAP();
        string strLimited = ProfileManager.Instance.dataConfig.GameText.GetTextByID(56) + " ";
        txtHourLimitedMax.text = strLimited + hourOfflineMax + "H";
        string timeOffline = GameManager.instance.timeManager.FormatOfflineTimeToString();
        string str1 = "You have been away for ";
        string str2 = " and have only received earnings for ";
        if (ProfileManager.Instance.dataConfig.GameText.GetTextByID(57) != "") str1 = ProfileManager.Instance.dataConfig.GameText.GetTextByID(57) + " ";
        if (ProfileManager.Instance.dataConfig.GameText.GetTextByID(58) != "") str2 = ProfileManager.Instance.dataConfig.GameText.GetTextByID(58) + " ";
        txtOfflineTime.text = str1 + timeOffline + " " + str2 + hourOfflineMax + "h.";
        bntContinueGame.gameObject.SetActive(false);
        isCollectBonusCash = false;
        isCollectBonusReputation = false;
        Invoke("ActiveBtnContinue", 2);
    }
    void ActiveBtnContinue() {
        bntContinueGame.gameObject.SetActive(true);
    }
    private void Update() {
        if (isCollectBonusCash) {
            btnX3.gameObject.SetActive(false);
            btnX2Ticket.gameObject.SetActive(false);
            btnX2.gameObject.SetActive(false);
        } else {
            btnX3.gameObject.SetActive(true);
            btnX3.interactable = GameManager.instance.IsEnoughGem(gemPriceX3);
            btnX2.gameObject.SetActive(ProfileManager.PlayerData.ResourceSave.GetADTicket() <= 0);
            btnX2Ticket.gameObject.SetActive(ProfileManager.PlayerData.ResourceSave.GetADTicket() > 0);
        }
        if (isCollectBonusReputation) {
            btnX3_Reputation.gameObject.SetActive(false);
            btnX2Ticket_Reputation.gameObject.SetActive(false);
            btnX2_Reputation.gameObject.SetActive(false);
        } else {
            btnX3_Reputation.gameObject.SetActive(true);
            btnX3_Reputation.interactable = GameManager.instance.IsEnoughGem(gemPriceX3);
            btnX2_Reputation.gameObject.SetActive(ProfileManager.PlayerData.ResourceSave.GetADTicket() <= 0);
            btnX2Ticket_Reputation.gameObject.SetActive(ProfileManager.PlayerData.ResourceSave.GetADTicket() > 0);
        }

    }
    private void OpenShop() {
        if (GameManager.instance && !isCollectBonusCash) {
            ProfileManager.PlayerData.AddCash(profit);
        }
        if (!isCollectBonusReputation) {
            ProfileManager.PlayerData.researchManager.AddResearchValue(reputation);
        }
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        UIManager.instance.ClosePanelBalanceOffline();
        UIManager.instance.GotoShopPage(TabName.OfflineTime);
    }
    private void OnX3() {
        if (GameManager.instance.IsEnoughGem(gemPriceX3)) {
            SoundManager.instance.PlaySoundEffect(SoundID.CASH_COLLECT);
            ProfileManager.PlayerData.ConsumeGem(gemPriceX3);
            ProfileManager.PlayerData.AddCash(profit * 3);
            isCollectBonusCash = true;
            ABIAnalyticsManager.Instance.TrackEventGem(GemAction.Spend_X3_Cash, gemPriceX3);
        }
    }

    private void OnX2() {
        SoundManager.instance.PlaySoundEffect(SoundID.CASH_COLLECT);
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.X2_Offline.ToString(), OnAdsX2Success);
        else OnAdsX2Success();
    }
    private void OnX2Ticket() {
        SoundManager.instance.PlaySoundEffect(SoundID.CASH_COLLECT);
        OnAdsX2Success();
        ProfileManager.PlayerData.ResourceSave.ConsumeADTicket();
    }

    void OnAdsX2Success() {
        isCollectBonusCash = true;
        ProfileManager.PlayerData.AddCash(profit * 2);
        if (UIManager.instance) UIManager.instance.ShowUIMoneyProfit(profit * 2);
    }

    void OnX2_Reputation() {
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.X2_Offline.ToString(), OnX2_Reputation_Success);
        else {
            OnX2_Reputation_Success();
        }
    }

    void OnX2_Reputation_Success() {
        isCollectBonusReputation = true;
        ProfileManager.PlayerData.researchManager.AddResearchValue(reputation * 2);
    }
    void OnX2Ticket_Reputation() {
        SoundManager.instance.PlaySoundEffect(SoundID.CASH_COLLECT);
        OnX2_Reputation_Success();
        ProfileManager.PlayerData.ResourceSave.ConsumeADTicket();
    }
    void OnX3_Reputation() {
        if (GameManager.instance.IsEnoughGem(gemPriceX3)) {
            SoundManager.instance.PlaySoundEffect(SoundID.CASH_COLLECT);
            ProfileManager.PlayerData.ConsumeGem(gemPriceX3);
            isCollectBonusReputation = true;
            ProfileManager.PlayerData.researchManager.AddResearchValue(reputation * 3);
            ABIAnalyticsManager.Instance.TrackEventGem(GemAction.Spend_X3_Reputation, gemPriceX3);
        }
    }
    void OnDefaultClaim() {
        bntContinueGame.gameObject.SetActive(false);
        SoundManager.instance.PlaySoundEffect(SoundID.CASH_COLLECT);
        if (GameManager.instance && !isCollectBonusCash) {
            ProfileManager.PlayerData.AddCash(profit);
            if (UIManager.instance) UIManager.instance.ShowUIMoneyProfit(profit);
        }
        if (!isCollectBonusReputation) {
            ProfileManager.PlayerData.researchManager.AddResearchValue(reputation);
        }
        UIManager.instance.ClosePanelBalanceOffline();
    }

}
