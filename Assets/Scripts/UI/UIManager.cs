using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using SDK;

public class UIManager : MonoBehaviour {
    public bool IsHideUI;
    public static UIManager instance;
    [SerializeField] RectTransform TopContent;
    [SerializeField] Transform uiMoneyEffRoot;
    public PanelResourceGem _UIPanelResourceGem;
    [SerializeField] Button btnShop, btnCardCollection;
    public Button btnMission, btnAdBoost, btnMarketingCampaign, btnStatistic;
    [SerializeField] Button btnOrderBook;
    [SerializeField] Button btnSetting;
    [SerializeField] GameObject questNotify;
    [SerializeField] GameObject marketingCampaignNotify;
    [SerializeField] GameObject marketingTime;
    [SerializeField] Text txtMarketingTime;
    public Transform gemRewardPos, cashRewardPos;
    [SerializeField] Transform uiMoneyNotiRoot;
    [SerializeField] Animator btnFreeCashAds;
    [SerializeField] Animator btnFreeGemAds;
    [SerializeField] GameObject orderBookNotify;
    [SerializeField] GameObject shopNotify;
    [SerializeField] GameObject cardNotify;
    [SerializeField] Button btnDailyReward;
    public Button btnTech;
    [SerializeField] Button btnIAPResearcherPack;
    [SerializeField] GameObject researchNotify;
    public Transform mainCanvas;
    [SerializeField] RectTransform panelWarningEmployeeSleep;
    //[SerializeField]
    //Text  txtIapPackage_Vip3Pack;
    [SerializeField] GameObject cheatgr;
    [SerializeField] Button btnWareHouse;
    [SerializeField] GameObject _WareHouseNotify;
    [SerializeField] GameObject _FreeGemAdsNotify;
    [SerializeField] GameObject processMap;
    public DOTweenManager dotweenManager;
    public bool isHasPopupOnScene = false;
    float deltaTimeFreeResourceAds;
    float timeFreeResourceAds = 20f;
    Dictionary<UIPanelType, GameObject> listPanel = new Dictionary<UIPanelType, GameObject>();
    MarketingManager marketingManager;
    QuestManager questManager;
    OrderBookManager orderBookManager;
    BoxManager boxManager;
    ResearchManager researchManager;
    float timeShowButtonFreeResource;

    public RectTransform m_PanelContent;
    public RectTransform m_PanelTutorial;

    // Start is called before the first frame update
    void Awake() {
        instance = this;
        btnShop.onClick.AddListener(ShowPanelShop);
        btnAdBoost.onClick.AddListener(ShowPanelAdBoost);
        btnMarketingCampaign.onClick.AddListener(ShowPanelMarketingCampaign);
        btnCardCollection.onClick.AddListener(ShowPanelCardCollection);
        btnMission.onClick.AddListener(ShowPanelQuest);
        btnFreeCashAds.GetComponent<Button>().onClick.AddListener(ShowPanelFreeCashAds);
        btnFreeGemAds.GetComponent<Button>().onClick.AddListener(ShowPanelFreeGemAds);
        btnOrderBook.onClick.AddListener(ShowPanelOrderBook);
        btnSetting.onClick.AddListener(ShowPanelSetting);
        btnStatistic.onClick.AddListener(ShowPanelStatistic);
        if (btnDailyReward) btnDailyReward.onClick.AddListener(ShowPanelDailyReward);
        if (btnTech) btnTech.onClick.AddListener(ShowPanelTech);
        if (btnWareHouse) btnWareHouse.onClick.AddListener(ShowPanelWareHouse);
        btnIAPResearcherPack.onClick.AddListener(ShowPanelOfferForProsPack);
        isHasPopupOnScene = false;
        deltaTimeFreeResourceAds = 0;
        marketingManager = ProfileManager.PlayerData.GetMarketingManager();
        questManager = ProfileManager.PlayerData.GetQuestManager();
        orderBookManager = ProfileManager.PlayerData.GetOrderBookManager();
        boxManager = ProfileManager.PlayerData.boxManager;
        researchManager = ProfileManager.PlayerData.researchManager;
        UpdateSceneRatio();
        if (IsHideUI) {
            HideOrShowUI(false);
        } else cheatgr.gameObject.SetActive(false);
        //LoadTimeFreeAdsServer();
        ActiveBannerAds(false);
        //EventManager.AddListener("UpdateRemoteConfigs", UpdateBannerAdsStatus);
    }
    private void Start() {
        questManager.CheckAnyQuestClaimable();
        UpdateBannerAdsStatus();
    }
    public void UpdateBannerAdsStatus() {
        if (AdsManager.Instance.IsGoodToShowBannerAds()) {
            ActiveBannerAds(true);
        } else {
            ActiveBannerAds(false);
        }
    }
    public void HideOrShowUI(bool isShow) {
        IsHideUI = !isShow;
        TopContent.gameObject.SetActive(isShow);
        btnShop.gameObject.SetActive(isShow);
        btnAdBoost.gameObject.SetActive(isShow);
        btnMarketingCampaign.gameObject.SetActive(isShow);
        btnMission.gameObject.SetActive(isShow);
        btnCardCollection.gameObject.SetActive(isShow);
        btnOrderBook.gameObject.SetActive(isShow);
        btnSetting.gameObject.SetActive(isShow);
        cheatgr.gameObject.SetActive(!isShow);
        btnIAPResearcherPack.gameObject.SetActive(isShow);
        btnWareHouse.gameObject.SetActive(isShow);
        btnTech.gameObject.SetActive(isShow);
        btnStatistic.gameObject.SetActive(isShow);
        processMap.gameObject.SetActive(isShow);
    }

    void UpdateSceneRatio() {
        float aspect = (float)Screen.height / (float)Screen.width;
        if (aspect >= 1.87) {
            TopContent.anchoredPosition = new Vector2(0, -120);
        } else TopContent.anchoredPosition = new Vector2(0, 0);
    }
    void Update() {
        CheckMarketingCampaignNotify();
        CheckQuestNotify();
        CheckAdsFreeResourceNotify();
        CheckorderBookNotify();
        CheckFreeShopNotify();
        CheckResearchNotify();
        UpdateTimeShowButtonFreeResource();
        CheckWareHouseNotify();
        CheckWarningEmployeeSleep();
        if (Tutorials.instance.IsRunStory) {
            btnIAPResearcherPack.gameObject.SetActive(false);
        } else btnIAPResearcherPack.gameObject.SetActive(!ProfileManager.PlayerData.ResourceSave.isBoughtOfferProsPack && !IsHideUI);
        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.WebGLPlayer) {
            if (Input.GetKeyDown(KeyCode.H)) {
                HideOrShowUI(false);
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                HideOrShowUI(true);
            }
        }
    }
    bool isMarketingActiving;
    void CheckMarketingCampaignNotify() {
        isMarketingActiving = marketingManager.IsBoostingAdsActive();
        if (marketingManager.IsPassTutorial) {
            marketingCampaignNotify.SetActive(!isMarketingActiving);
        } else marketingCampaignNotify.SetActive(false);
        marketingTime.SetActive(isMarketingActiving);
        txtMarketingTime.text = marketingManager.RemainTimeToString2();
    }
    bool isHasQuestClaimable;
    void CheckQuestNotify() {
        isHasQuestClaimable = questManager.hasQuestClaimable;
        questNotify.SetActive(isHasQuestClaimable);
    }
    bool isHasFreeChest;
    bool isHasGemAdsFree;
    void CheckFreeShopNotify() {
        isHasFreeChest = boxManager.IsHasFreeChest();
        isHasGemAdsFree = !ProfileManager.PlayerData.ResourceSave.IsWatchedFreeGemAds();
        shopNotify.SetActive(isHasFreeChest || isHasGemAdsFree);
        _FreeGemAdsNotify.SetActive(isHasGemAdsFree);
        cardNotify.SetActive(isHasFreeChest);
    }

    bool isResearchNotify;
    void CheckResearchNotify() {
        isResearchNotify = researchManager.IsHasFreeCustomerCanWatched();
        researchNotify.SetActive(isResearchNotify);
    }

    bool hasOrderClaimable;
    void CheckorderBookNotify() {
        hasOrderClaimable = orderBookManager.IsNotify();
        orderBookNotify.SetActive(hasOrderClaimable);
    }
    void CheckWareHouseNotify() {
        _WareHouseNotify.SetActive(ProfileManager.PlayerData.wareHouseManager.IsHasNotify());
    }
    public bool isShowingFreeAds = false;
    void CheckAdsFreeResourceNotify() {
        //Debug.Log(Time.unscaledDeltaTime);
        if (isShowingFreeAds) return;
        deltaTimeFreeResourceAds += Time.unscaledDeltaTime;
        if (Tutorials.instance.IsShow) return;
        if (deltaTimeFreeResourceAds >= timeFreeResourceAds) {
            deltaTimeFreeResourceAds = 0;
            countFreeResourceAds++;
            if (countFreeResourceAds % 4 > 0) {
                ShowButtonFreeCashAds();
            } else {
                ShowButtonFreeGemAds();
            }
        }
    }
    int countFreeResourceAds;
    public void CreatUIMoneyEff(BigNumber value, Transform trans) {
        //return;
        Transform t = PoolManager.Pools["GameEntity"].Spawn("UIMoneyEff");
        string strValue = value.IntToString();
        t.GetComponent<UIMoneyEff>().Show(strValue, trans);
    }
    public void ShowPanelRoomInfo(IRoomController roomManager) {
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelRoomInfo).SetActive(true);
        GetPanel(UIPanelType.PanelRoomInfo).GetComponent<PanelRoomInfo>().Setup(roomManager);
    }
    public void ShowPanelRoomInfoSpecifiedIndex(IRoomController roomManager, int index) {
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelRoomInfo).SetActive(true);
        GetPanel(UIPanelType.PanelRoomInfo).GetComponent<PanelRoomInfo>().Setup(roomManager);
        GetPanel(UIPanelType.PanelRoomInfo).GetComponent<PanelRoomInfo>().OnShowInfoItem(index);
    }
    public void ShowPanelRoomInfo_Staff(IRoomController roomManager) {
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelRoomInfo).SetActive(true);
        GetPanel(UIPanelType.PanelRoomInfo).GetComponent<PanelRoomInfo>().Setup(roomManager, false);
    }
    public void ClosePanelRoomInfo() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelRoomInfo);
        GetPanel(UIPanelType.PanelRoomInfo).SetActive(false);
    }
    public void ShowPanelManagerStaff() {
        if (Tutorials.instance.IsShow) return;
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowInterstitial(null, null);
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelManagerStaff).SetActive(true);
        GetPanel(UIPanelType.PanelManagerStaff).GetComponent<PanelManagerStaff>().OnShow();
    }
    public void ClosePanelManagerStaff() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelManagerStaff);
        GetPanel(UIPanelType.PanelManagerStaff).SetActive(false);
    }
    public void ShowPanelShop() {
        if (Tutorials.instance.IsShow) return;
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowInterstitial(null, null);
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelShop).SetActive(true);
    }
    public void ClosePanelShop() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelShop);
        GetPanel(UIPanelType.PanelShop).SetActive(false);
    }
    public void GotoShopPage(TabName shopName) {
        isHasPopupOnScene = true;
        GetPanel(UIPanelType.PanelShop).SetActive(true);
        UIShopTabButton tab = GetPanel(UIPanelType.PanelShop).GetComponent<UIShopManager>().tabGroup.GetTabButton(shopName);
        if (!tab.gameObject.activeInHierarchy) return;
        StartCoroutine(WaitUntilShopScrollSettingShopPagePos(GetPanel(UIPanelType.PanelShop).GetComponent<UIShopManager>(), tab));
    }
    IEnumerator WaitUntilShopScrollSettingShopPagePos(UIShopManager uIShopManager, UIShopTabButton tabButton) {
        yield return new WaitForSeconds(.2f);
        uIShopManager.tabGroup.OnSelected(tabButton, true);
    }
    public void ShowPanelAdBoost() {
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() != TutorialStepID.AdBoost) return;
        if (Tutorials.instance.IsBlockInput) return;
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowInterstitial(null, null);
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelAdBoost).SetActive(true);
    }
    public void ClosePanelAdBoost() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelAdBoost);
        GetPanel(UIPanelType.PanelAdBoost).SetActive(false);
    }
    public void ShowConfirmPanel() {
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelConfirmOpenChest).SetActive(true);
    }
    public void CloseConfirmPanel() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelConfirmOpenChest);
        GetPanel(UIPanelType.PanelConfirmOpenChest).SetActive(false);
    }

    public void ShowConfirmGachaPanel() {
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelConfirmGacha).SetActive(true);
    }
    public void CloseConfirmGachaPanel() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelConfirmGacha);
        GetPanel(UIPanelType.PanelConfirmGacha).SetActive(false);
    }
    public void ShowPanelManagerBuild(RoomID target, int GroupID = 0) {
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelManagerBuild).SetActive(true);
        GetPanel(UIPanelType.PanelManagerBuild).GetComponent<PanelManagerBuild>().Show(target);
    }
    public void ClosePanelManagerBuild() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelManagerBuild);
        GetPanel(UIPanelType.PanelManagerBuild).SetActive(false);
    }
    public void ShowPanelDailyReward() {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelDailyReward);
        go.SetActive(true);
        PanelDailyReward dailyRewardPanel = go.GetComponent<PanelDailyReward>();
        dailyRewardPanel.InitUI();
    }
    public void ClosePanelDailyReward() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelDailyReward);
        GetPanel(UIPanelType.PanelDailyReward).SetActive(false);
    }
    public void ShowPanelMarketingCampaign() {
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() != TutorialStepID.ActiveMarketingCampaign) return;
        if (Tutorials.instance.IsBlockInput) return;
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowInterstitial(null, null);
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelMarketingCampaign).SetActive(true);
    }
    public void ClosePanelMarketingCampaign() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelMarketingCampaign);
        GetPanel(UIPanelType.PanelMarketingCampaign).SetActive(false);
    }
    public void ShowPanelCardCollection() {
        if (Tutorials.instance.IsShow) return;
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowInterstitial(null, null);
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelCard).SetActive(true);
        GetPanel(UIPanelType.PanelCard).GetComponent<PanelCard>().OnOpen();
    }
    public void ClosePanelCard() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelCard);
        GetPanel(UIPanelType.PanelCard).SetActive(false);
    }
    public void ShowPanelSkin(StaffID charaterType) {
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() != TutorialStepID.HireStaff) return;
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowInterstitial(null, null);
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelSkin).SetActive(true);
        GetPanel(UIPanelType.PanelSkin).GetComponent<PanelSkin>().LoadListSkin(charaterType);
    }
    public bool IsPanelSkinActive() {
        foreach (KeyValuePair<UIPanelType, GameObject> item in listPanel) {
            if (item.Value.activeInHierarchy && item.Key == UIPanelType.PanelSkin) {
                return true;
            }
        }
        return false;
    }
    public void ClosePanelSkin() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelSkin);
        GetPanel(UIPanelType.PanelSkin).SetActive(false);
    }
    public void ShowPanelQuest() {
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() != TutorialStepID.ClaimQuest) return;
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowInterstitial(null, null);
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelQuest).SetActive(true);
    }
    public void ClosePanelQuest() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelQuest);
        GetPanel(UIPanelType.PanelQuest).SetActive(false);
    }
    public void ShowPanelBalanceOffline() {
        if (Tutorials.instance.IsShow) return;
        CloseAllPopup();
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelBalanceOffline).SetActive(true);
    }
    public void ClosePanelBalanceOffline() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelBalanceOffline);
        GetPanel(UIPanelType.PanelBalanceOffline).SetActive(false);
    }

    public void ShowPanelReward(ItemType type) {
        GameObject panelReward = GetPanel(UIPanelType.PanelReward);
        panelReward.SetActive(true);
        panelReward.GetComponent<PanelReward>().ShowBoxReward(type);
    }

    public void ClosePanelReward() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelReward);
        GetPanel(UIPanelType.PanelReward).SetActive(false);
    }
    public void ShowPanelTipProfit() {
        if (Tutorials.instance.IsShow) return;
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowInterstitial(null, null);
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelTipProfit).SetActive(true);
    }
    public void ClosePanelTipProfit() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelTipProfit);
        GetPanel(UIPanelType.PanelTipProfit).SetActive(false);
    }

    public void ShowPanelOrderBook() {
        if (Tutorials.instance.IsShow) return;
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelOrderBook).SetActive(true);
    }
    public void ClosePanelOrderBook() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelOrderBook);
        GetPanel(UIPanelType.PanelOrderBook).SetActive(false);
    }
    public void ShowPanelStatistic() {
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() != TutorialStepID.SelectNewWorld) return;
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowInterstitial(null, null);
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelStatistic).SetActive(true);
    }
    public void ClosePanelStatistic() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelStatistic);
        GetPanel(UIPanelType.PanelStatistic).SetActive(false);
    }

    void CheckHasPopupOnSceneWhenClosePopup(UIPanelType current) {
        isHasPopupOnScene = false;
        foreach (KeyValuePair<UIPanelType, GameObject> item in listPanel) {
            if (item.Value.activeInHierarchy && item.Key != current) {
                isHasPopupOnScene = true;
                break;
            }
        }
    }
    void UpdateTimeShowButtonFreeResource() {
        if (timeShowButtonFreeResource > 0) {
            timeShowButtonFreeResource -= Time.unscaledDeltaTime;
            if (timeShowButtonFreeResource <= 0) {
                CloseButtonFreeCashAds();
                CloseButtonFreeGemAds();
            }
        }
    }
    public void ShowButtonFreeCashAds() {
        isShowingFreeAds = true;
        btnFreeCashAds.SetBool("IsShow", true);
        timeShowButtonFreeResource = 20;
    }
    void CloseButtonFreeCashAds() {
        isShowingFreeAds = false;
        deltaTimeFreeResourceAds = 0;
        btnFreeCashAds.SetBool("IsShow", false);
    }
    public void ShowButtonFreeGemAds() {
        isShowingFreeAds = true;
        btnFreeGemAds.SetBool("IsShow", true);
        timeShowButtonFreeResource = 20;
    }
    void CloseButtonFreeGemAds() {
        isShowingFreeAds = false;
        deltaTimeFreeResourceAds = 0;
        btnFreeGemAds.SetBool("IsShow", false);
    }
    public void ShowPanelFreeCashAds() {
        if (Tutorials.instance.IsShow) return;
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelFreeCashAds).SetActive(true);
        CloseButtonFreeCashAds();
        isShowingFreeAds = true;
    }
    public void ClosePanelFreeCashAds() {
        isShowingFreeAds = false;
        isHasPopupOnScene = false;
        GetPanel(UIPanelType.PanelFreeCashAds).SetActive(false);
        deltaTimeFreeResourceAds = 0;
    }
    public void ShowPanelFreeGemAds() {
        if (Tutorials.instance.IsShow) return;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        CloseButtonFreeGemAds();
        isHasPopupOnScene = true;
        GetPanel(UIPanelType.PanelFreeGemAds).SetActive(true);
        isShowingFreeAds = true;
    }
    public void ClosePanelFreeGemAds() {
        isShowingFreeAds = false;
        isHasPopupOnScene = false;
        GetPanel(UIPanelType.PanelFreeGemAds).SetActive(false);
        deltaTimeFreeResourceAds = 0;
    }
    public void ShowPanelSetting() {
        if (Tutorials.instance.IsShow) return;
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowInterstitial(null, null);
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        isHasPopupOnScene = true;
        GetPanel(UIPanelType.PanelSetting).SetActive(true);
    }
    public void ClosePanelSetting() {
        isHasPopupOnScene = false;
        GetPanel(UIPanelType.PanelSetting).SetActive(false);
    }
    public void ShowUIMoneyCost(float cost) {
        if (PoolManager.Pools["GameEntity"] == null) return;
        Transform t = PoolManager.Pools["GameEntity"].Spawn("UIMoneyNotifi", uiMoneyNotiRoot);
        if (t != null) {
            t.gameObject.SetActive(true);
            t.GetComponent<UIMoneyNotify>().ShowSalaryCost(cost);
        }
    }

    public void ShowUIMoneyProfit(BigNumber profit) {
        if (PoolManager.Pools["GameEntity"] == null) return;
        Transform t = PoolManager.Pools["GameEntity"].Spawn("UIMoneyNotifi", uiMoneyNotiRoot);
        if (t != null) {
            t.gameObject.SetActive(true);
            t.GetComponent<UIMoneyNotify>().ShowProfitEarn(profit);
        }
    }
    public void ShowUIPanelReward(List<ItemReward> list, bool skin = false) {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelRewardItem);
        go.SetActive(true);
        PanelRewardItem panel = go.GetComponent<PanelRewardItem>();
        panel.ShowItem(list, skin);
    }

    public void CloseUIPanelReward() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelRewardItem);
        GetPanel(UIPanelType.PanelRewardItem).SetActive(false);
    }
    public void ShowEventPanel() {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelEvent);
        go.SetActive(true);
    }
    public void CloseEventPanel() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelEvent);
        GetPanel(UIPanelType.PanelEvent).SetActive(false);
    }
    public void ShowPanelViewProcess() {
        isHasPopupOnScene = true;
        GetPanel(UIPanelType.PanelViewProcessEvent).SetActive(true);
    }
    public void ClosePanelViewProcess() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelViewProcessEvent);
        GetPanel(UIPanelType.PanelViewProcessEvent).SetActive(false);
    }
    public void ShowPanelSkipSleepAds() {
        if (Tutorials.instance.IsShow) return;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        isHasPopupOnScene = true;
        GetPanel(UIPanelType.PanelSkipSleepAds).SetActive(true);
    }
    public void ClosePanelSkipSleepAds() {
        isHasPopupOnScene = false;
        GetPanel(UIPanelType.PanelSkipSleepAds).SetActive(false);
    }

    public void ShowPanelTech() {
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() != TutorialStepID.Research) return;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelTechnology);
        go.SetActive(true);
        ABIAnalyticsManager.Instance.TrackEventResearch(ResearchAction.CheckIn);
    }
    public void ClosePanelTech() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelTechnology);
        GetPanel(UIPanelType.PanelTechnology).SetActive(false);
    }
    public void ShowPanelOfferForProsPack() {
        if (Tutorials.instance.IsShow) return;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        isHasPopupOnScene = true;
        GetPanel(UIPanelType.PanelOfferForProsPack).SetActive(true);
    }
    public void ClosePanelOfferForProsPack() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelOfferForProsPack);
        GetPanel(UIPanelType.PanelOfferForProsPack).SetActive(false);
    }
    public void ShowPanelManagerCardLevelUp() {
        if (Tutorials.instance.IsShow) return;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        isHasPopupOnScene = true;
        GetPanel(UIPanelType.PanelManagerCardLevelUp).SetActive(true);
    }
    public void ClosePanelManagerCardLevelUp() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelManagerCardLevelUp);
        GetPanel(UIPanelType.PanelManagerCardLevelUp).SetActive(false);
    }
    public void ShowPanelManagerCardBuyResources(ManagerStaffID staffID) {
        if (Tutorials.instance.IsShow) return;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        isHasPopupOnScene = true;
        GetPanel(UIPanelType.PanelManagerCardBuyResources).SetActive(true);

    }
    public void ClosePanelManagerCardBuyResources() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelManagerCardBuyResources);
        GetPanel(UIPanelType.PanelManagerCardBuyResources).SetActive(false);
    }
    public void CloseAllPopup() {
        isHasPopupOnScene = false;
        SoundManager.instance.StopSubMusic();
        foreach (KeyValuePair<UIPanelType, GameObject> item in listPanel) {
            item.Value.SetActive(false);
        }
    }
    public string FormatTimeToString(TimeSpan timeSpan) {
        return string.Format("{0:D1}d {1:D2}h {2:D2}m {3:D2}s", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }
    public void RegisterPanel(UIPanelType type, GameObject obj) {
        GameObject go = null;
        if (!listPanel.TryGetValue(type, out go)) {
            Debug.Log("RegisterPanel " + type.ToString());
            listPanel.Add(type, obj);
        }
        obj.SetActive(false);
    }
    public GameObject GetPanel(UIPanelType type) {
        GameObject panel = null;
        if (!listPanel.TryGetValue(type, out panel)) {
            switch (type) {
                case UIPanelType.PanelConfirmOpenChest:
                    panel = Instantiate(Resources.Load("UI/PanelConfirmOpenChest") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelTipProfit:
                    panel = Instantiate(Resources.Load("UI/PanelTipProfit") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelDailyReward:
                    panel = Instantiate(Resources.Load("UI/PanelDailyReward") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelAdBoost:
                    panel = Instantiate(Resources.Load("UI/PanelAdBoost") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelMarketingCampaign:
                    panel = Instantiate(Resources.Load("UI/PanelMarketing") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelManagerStaff:
                    panel = Instantiate(Resources.Load("UI/PanelManagerStaff") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelManagerBuild:
                    panel = Instantiate(Resources.Load("UI/PanelManagerBuild") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelQuest:
                    panel = Instantiate(Resources.Load("UI/PanelQuest") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelBalanceOffline:
                    panel = Instantiate(Resources.Load("UI/PanelBalanceOffline") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelRoomInfo:
                    panel = Instantiate(Resources.Load("UI/PanelRoomInfo") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelCard:
                    panel = Instantiate(Resources.Load("UI/PanelCard") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelShop:
                    panel = Instantiate(Resources.Load("UI/PanelShop") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelReward:
                    panel = Instantiate(Resources.Load("UI/PanelRewardChest") as GameObject, mainCanvas);
                    break;
                case UIPanelType.CardOpenGetAnimation:
                    panel = Instantiate(Resources.Load("UI/CardOpenAnimPanel") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelOrderBook:
                    panel = Instantiate(Resources.Load("UI/PanelOrderBook") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelFreeCashAds:
                    panel = Instantiate(Resources.Load("UI/PanelFreeCashAds") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelFreeGemAds:
                    panel = Instantiate(Resources.Load("UI/PanelFreeGemAds") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelStatistic:
                    panel = Instantiate(Resources.Load("UI/PanelStatistic") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelAdBoostSpeed:
                    panel = Instantiate(Resources.Load("UI/PanelAdBoostSpeed") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelSetting:
                    panel = Instantiate(Resources.Load("UI/PanelSetting") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelWorldSelect:
                    panel = Instantiate(Resources.Load("UI/PanelWorldSelect") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelIAPPackage:
                    panel = Instantiate(Resources.Load("UI/PanelIAPPackage") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelRewardItem:
                    panel = Instantiate(Resources.Load("UI/PanelRewardItem") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelEvent:
                    panel = Instantiate(Resources.Load("UI/PanelEvent") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelSkin:
                    panel = Instantiate(Resources.Load("UI/PanelSkin") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelRewardSkin:
                    panel = Instantiate(Resources.Load("UI/PanelRewardSkin") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelRate:
                    panel = Instantiate(Resources.Load("UI/PanelRate") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelViewProcessEvent:
                    panel = Instantiate(Resources.Load("UI/PanelViewProcessEvent") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelConfirmGacha:
                    panel = Instantiate(Resources.Load("UI/PanelConfirmGacha") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelSkipSleepAds:
                    panel = Instantiate(Resources.Load("UI/PanelSkipSleepAds") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelStarReward:
                    panel = Instantiate(Resources.Load("UI/PanelStarReward") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelTechnology:
                    panel = Instantiate(Resources.Load("UI/PanelResearch") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelChristmasPack:
                    panel = Instantiate(Resources.Load("UI/PanelChristmasPack") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelWareHouse:
                    panel = Instantiate(Resources.Load("UI/PanelWareHouse") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelOfferForProsPack:
                    panel = Instantiate(Resources.Load("UI/PanelOfferForProsPack") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelManagerCardLevelUp:
                    panel = Instantiate(Resources.Load("UI/PanelManagerCardLevelUp") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelManagerCardBuyResources:
                    panel = Instantiate(Resources.Load("UI/PanelManagerCardBuyResources") as GameObject, mainCanvas);
                    break;
            }
            if (panel) panel.SetActive(true);
            return panel;
        }
        return listPanel[type];
    }

    public void OnShowMainCanvas(bool isShow) {
        mainCanvas.gameObject.SetActive(isShow);
    }

    public void CheckWarningEmployeeSleep() {
        if (Tutorials.instance.IsShow) return;
        panelWarningEmployeeSleep.gameObject.SetActive(GameManager.instance._EmployeeSleeping.Count > 0);
        LayoutRebuilder.MarkLayoutForRebuild(panelWarningEmployeeSleep);
    }

    public void ShowPanelWorldSelect() {
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() != TutorialStepID.SelectNewWorld) return;
        if (Tutorials.instance.IsBlockInput) return;
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowInterstitial(null, null);
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelWorldSelect).SetActive(true);
    }
    public void ClosePanelWorldSelect() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelWorldSelect);
        GetPanel(UIPanelType.PanelWorldSelect).SetActive(false);
    }
    public void ShowPanelIAPPackage(OfferID offerID) {
        if (Tutorials.instance.IsShow) return;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelIAPPackage);
        go.GetComponent<PanelIAPPackage>().SetupIAPPackge(offerID);
        go.SetActive(true);
    }
    public void ClosePanelIAPPackage() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelIAPPackage);
        GetPanel(UIPanelType.PanelIAPPackage).SetActive(false);
    }
    public void ShowPanelChristmasPack() {
        if (Tutorials.instance.IsShow) return;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelChristmasPack);
        go.SetActive(true);
    }
    public void ClosePanelChristmasPack() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelChristmasPack);
        GetPanel(UIPanelType.PanelChristmasPack).SetActive(false);
    }
    public void ShowPanelWareHouse() {
        if (Tutorials.instance.IsShow) return;
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowInterstitial(null, null);
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelWareHouse);
        go.SetActive(true);
        PanelWareHouse wareHousePanel = go.GetComponent<PanelWareHouse>();
        wareHousePanel.OnOpen();
        ABIAnalyticsManager.Instance.TrackEventWareHouse(WareHouseAction.CheckIn);
    }
    public void ClosePanelWareHouse() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelWareHouse);
        GetPanel(UIPanelType.PanelWareHouse).SetActive(false);
    }


    public void ShowPanelRate() {
        if (Tutorials.instance.IsShow) return;
        isHasPopupOnScene = true;
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        GetPanel(UIPanelType.PanelRate).SetActive(true);
    }
    public void ClosePanelRate() {
        CheckHasPopupOnSceneWhenClosePopup(UIPanelType.PanelRate);
        GetPanel(UIPanelType.PanelRate).SetActive(false);
    }

    void LoadTimeFreeAdsServer() {
        timeFreeResourceAds = 60;
        string data = ABIFirebaseManager.Instance.m_FirebaseRemoteConfigManager.GetValues(ABI.Keys.key_remote_free_ads).StringValue;
        Debug.Log("Time == " + data);
        if (!string.IsNullOrEmpty(data)) {
            int time = int.Parse(data);
            //if (time > 0) timeFreeResourceAds = time;
        }
    }
    public GameObject m_ButtonShowReview;
    public void ShowAppReview() {
        AppReviewManager.Instance.ShowReview();
    }
    public void RequestAppReview() {
        m_ButtonShowReview.SetActive(false);
        AppReviewManager.Instance.StartRequestReviewInfo(() => {
            m_ButtonShowReview.SetActive(true);
        });
    }
    public void ActiveBannerAds(bool isActive) {
        //m_PanelBannerAds.gameObject.SetActive(isActive);
        //if (isActive) {
        //    m_PanelContent.sizeDelta = new Vector2(0, -200);
        //    m_PanelContent.anchoredPosition = new Vector2(0, 100);
        //    m_PanelTutorial.sizeDelta = new Vector2(0, 200);
        //    m_PanelTutorial.anchoredPosition = new Vector2(0, -100);
        //} else {
        //    m_PanelContent.sizeDelta = new Vector2(0, 0);
        //    m_PanelContent.anchoredPosition = new Vector2(0, 0);
        //    m_PanelTutorial.sizeDelta = new Vector2(0, 0);
        //    m_PanelTutorial.anchoredPosition = new Vector2(0, 0);
        //}
    }
}
