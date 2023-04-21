using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelAdBoost : UIPanel {
    [SerializeField] Button btnAdsBoost, btnAdsBoostTicket, btnAdsBoostFree, btnClose, btnGoToShopPage;
    [SerializeField] Text txtRemainTime;
    [SerializeField] Slider sliderRemainTime;
    AdBoostManager adBoostManager;
    public override void Awake() {
        panelType = UIPanelType.PanelAdBoost;
        base.Awake();
        btnAdsBoost.onClick.AddListener(WatchVideo);
        btnAdsBoostFree.onClick.AddListener(ActiveFreeTutorial);
        btnAdsBoostTicket.onClick.AddListener(UseTicket);
        btnClose.onClick.AddListener(OnClose);
        btnGoToShopPage.onClick.AddListener(OpenShop);
        adBoostManager = ProfileManager.PlayerData.GetAdBoostManager();
    }
    private void OnEnable() {
        if (!adBoostManager.IsPassTutorial) {
            btnAdsBoost.gameObject.SetActive(false);
            btnAdsBoostTicket.gameObject.SetActive(false);
            btnAdsBoostFree.gameObject.SetActive(true);
            if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() == TutorialStepID.AdBoost)
                StartCoroutine(ICheckShowTut());
        } else {
            btnAdsBoostFree.gameObject.SetActive(false);
        }
    }
    IEnumerator ICheckShowTut() {
        yield return new WaitForSeconds(0.7f);
        Tutorials.instance.OnShow(btnAdsBoostFree.transform);
    }

    void Update() {
        if (adBoostManager.IsPassTutorial) {
            btnAdsBoostTicket.gameObject.SetActive(ProfileManager.PlayerData.ResourceSave.GetADTicket() > 0 && !adBoostManager.IsFullBoostFinance());
            if(ProfileManager.PlayerData.ResourceSave.GetADTicket() >0 || adBoostManager.IsFullBoostFinance())
               btnAdsBoost.gameObject.SetActive(false);
            else btnAdsBoost.gameObject.SetActive(true);
         
        } else {
            btnAdsBoostTicket.gameObject.SetActive(false);
            btnAdsBoost.gameObject.SetActive(false);
            btnAdsBoostFree.gameObject.SetActive(true);
        }
       
        if (adBoostManager.IsBoostFinanceActive()) {
            txtRemainTime.text = adBoostManager.FinanceRemainTimeToString2();
            sliderRemainTime.value = adBoostManager.remainBoostSeconds / (2 * 3600);
        } else {
            txtRemainTime.text = "0h 0p 0s";
            sliderRemainTime.value = 0;
        }

    }
    void OpenShop() {
        OnClose();
        UIManager.instance.GotoShopPage(TabName.Financial);
    }

    public void WatchVideo() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.X2_Profit.ToString(), BoostingMark);
        else BoostingMark();
    }
    public void UseTicket() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        BoostingMark();
        ProfileManager.PlayerData.ResourceSave.ConsumeADTicket();
    }
    public void BoostingMark() {
        adBoostManager.AddMoreTime();
    }
    void ActiveFreeTutorial() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        adBoostManager.AddMoreTimeTutorial();
        btnAdsBoostFree.gameObject.SetActive(false);
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() == TutorialStepID.AdBoost) {
            Tutorials.instance.FinishTutorial();
            Tutorials.instance.OnCloseTutorial();
        }
    }

    public void OnClose() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        UIManager.instance.ClosePanelAdBoost();
    }

}
