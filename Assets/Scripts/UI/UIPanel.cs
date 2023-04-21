using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum UIPanelType {
    PanelRoomInfo,
    PanelConfirmOpenChest,
    PanelFreeCashAds,
    PanelTipProfit,
    PanelDailyReward,
    PanelAdBoost,
    PanelMarketingCampaign,
    PanelManagerStaff,
    PanelManagerBuild,
    PanelQuest,
    PanelBalanceOffline,
    PanelCard,
    //OfferSuggestPanel,
    PanelShop,
    PanelReward,
    CardOpenGetAnimation,
    PanelNegotiation,
    PanelOrderBook,
    PanelFreeGemAds,
    PanelStatistic,
    PanelAdBoostSpeed,
    PanelSetting,
    PanelWorldSelect,
    PanelIAPPackage,
    PanelTechnology,
    PanelRewardItem,
    PanelEvent,
    PanelRate,
    PanelViewProcessEvent,
    PanelSkin,
    PanelRewardSkin,
    PanelConfirmGacha,
    PanelSkipSleepAds,
    PanelStarReward,
    PanelChristmasPack,
    PanelWareHouse,
    PanelPremiumResearcherPack
}
public class UIPanel : MonoBehaviour {
    public bool isRegisterInUI = true;
    protected UIPanelType panelType;
    [SerializeField]  RectTransform Content;
    [SerializeField] bool IsFixContent;
    // Start is called before the first frame update
    public virtual void Awake() {
        if (isRegisterInUI) UIManager.instance.RegisterPanel(panelType, gameObject);
        if (IsFixContent) UpdateSceneRatio();
    }
    void UpdateSceneRatio() {
        float aspect = (float)Screen.height / (float)Screen.width;
        if (aspect >= 1.87) {
            Content.offsetMax = new Vector2(Content.offsetMax.x, -100);
        } else Content.offsetMax = new Vector2(Content.offsetMax.x, 0);
    }
    protected void ScaleEffectButton(Transform btn) {
        btn.transform.localScale = Vector3.one;
        btn.DOScale(new Vector3(1.1f, 1.05f, 1), 0.2f).OnComplete(() => {
            btn.transform.localScale = Vector3.one;
        });
    }
}
