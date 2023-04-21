using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using SDK;

public class PanelMarketingCampaign : UIPanel {
    [SerializeField] Button btnClose;
    [Header("Ads Campaign Detail")]
    [SerializeField] Text txt_AdsSpawnRateIncreaseUI;
    [SerializeField] Text txt_AdsVIPRateIncreaseUI;
    [SerializeField] Slider sliderAdsCampaign;
    [SerializeField] Text adsCampaignTime;
    [SerializeField] Button btnWatchAdsCampaign;
    [SerializeField] Button btnWatch_Ticket;
    [SerializeField] Button btnFreeAdsCampaign;
    [Header("VIP Campaign Detail")]
    [SerializeField] Text txt_IAPSpawnRateVIPIncreaseUI;
    [SerializeField] Text txt_IAPVIPRateVIPIncreaseUI;
    [SerializeField] Button btnGoToShop;
    [SerializeField] GameObject objInApp;
    [SerializeField] GameObject objVIPActive;
    MarketingManager marketingManager;
    public override void Awake() {
        panelType = UIPanelType.PanelMarketingCampaign;
        base.Awake();
        btnWatchAdsCampaign.onClick.AddListener(OnActiveWatchADSCampaign);
        btnWatch_Ticket.onClick.AddListener(OnActiveTicket);
        btnFreeAdsCampaign.onClick.AddListener(OnFreeActiveWatchADSCampaign);
        btnGoToShop.onClick.AddListener(OnGoToShop);
        btnClose.onClick.AddListener(OnClose);
        marketingManager = ProfileManager.PlayerData.GetMarketingManager();
    }
    private void OnEnable() {
        OnSetup();
    }
    public void OnSetup() {
        txt_AdsSpawnRateIncreaseUI.text = "+" + marketingManager.GetAdsSpawnIncreaseRate().ToString() + "%";
        txt_AdsVIPRateIncreaseUI.text = "+" + marketingManager.GetAdsVIPIncreaseRate().ToString() + "%";
        txt_IAPSpawnRateVIPIncreaseUI.text = "+" + marketingManager.GetIAPSpawnIncreaseRate().ToString() + "%";
        txt_IAPVIPRateVIPIncreaseUI.text = "+" + marketingManager.GetIAPVIPIncreaseRate().ToString() + "%";
        btnWatchAdsCampaign.gameObject.SetActive(marketingManager.IsPassTutorial);
        btnWatch_Ticket.gameObject.SetActive(marketingManager.IsPassTutorial && ProfileManager.PlayerData.ResourceSave.GetADTicket() > 0);
        btnFreeAdsCampaign.gameObject.SetActive(!marketingManager.IsPassTutorial);
        if (!marketingManager.IsPassTutorial) {
            StartCoroutine(ICheckShowTut());
        }
    }
    IEnumerator ICheckShowTut() {
        yield return new WaitForSeconds(0.7f);
        if(Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep()== TutorialStepID.ActiveMarketingCampaign) {
            Tutorials.instance.OnShow(btnFreeAdsCampaign.transform);
        }
    }
    private void Update() {
        if (marketingManager.IsBoostingAdsActive()) {
            adsCampaignTime.text = marketingManager.RemainTimeToString();
            sliderAdsCampaign.value = marketingManager.remainBoostSeconds / (2f * 3600f);
            btnWatchAdsCampaign.interactable = marketingManager.CheckAbleWatchVideo();
        } else {
            adsCampaignTime.text = "0h 0p 0s";
            sliderAdsCampaign.value = 0f;
        }
        objInApp.gameObject.SetActive(!marketingManager.IsVIPActive);
        objVIPActive.gameObject.SetActive(marketingManager.IsVIPActive);
    }


    public void OnActiveWatchADSCampaign() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.Marketing.ToString(), OnActiveWatchADSCampaignSuccess);
        else OnActiveWatchADSCampaignSuccess();
    }
    void OnActiveWatchADSCampaignSuccess() {
        marketingManager.AddAdsCampaignTime();
    }
    public void OnActiveTicket() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        marketingManager.AddAdsCampaignTime();
        ProfileManager.PlayerData.ResourceSave.ConsumeADTicket();
        btnWatch_Ticket.gameObject.SetActive(ProfileManager.PlayerData.ResourceSave.GetADTicket() > 0 && !marketingManager.IsFullBoost());
    }
    public void OnFreeActiveWatchADSCampaign() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        if (!marketingManager.IsPassTutorial) {
            marketingManager.IsPassTutorial = true;
            Tutorials.instance.FinishTutorial();
            Tutorials.instance.OnCloseTutorial();
        }
        btnWatchAdsCampaign.gameObject.SetActive(true);
        btnFreeAdsCampaign.gameObject.SetActive(false);
        marketingManager.AddMoreTimeByTutorial();
    }
    public void OnGoToShop() {
        OnClose();
        UIManager.instance.ShowPanelShop();
    }
    public void OnClose() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        UIManager.instance.ClosePanelMarketingCampaign();
    }
}
