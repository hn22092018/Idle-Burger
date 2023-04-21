using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGemSlot : UIShopSlot {
    int reward;
    string productID;
    OfferData currentOfferData;
    public GameObject btnWatchAds;
    public GameObject notifyADS;
    public GameObject BlockADS;
    public GameObject TicketADS;
    public override void InitData(OfferData offerData) {
        base.InitData(offerData);
        currentOfferData = offerData;
        productID = offerData.productID;
        reward = offerData.itemRewards[0].amount;
        titleText.text = reward.ToString();
        if (currentOfferData.offerID != OfferID.GemAds) {
            string priceLocal = MyIAPManager.instance.GetProductPriceFromStore(productID);
            priceText.text = priceLocal != "$0.01" ? priceLocal : offerData.price.ToString() + "$";
            TicketADS.gameObject.SetActive(false);
            BlockADS.gameObject.SetActive(false);
            notifyADS.gameObject.SetActive(false);
            btnWatchAds.gameObject.SetActive(false);
        } else
            LoadInfoFreeGemAdsState();
    }
    private void OnEnable() {
        LoadInfoFreeGemAdsState();
    }
    void LoadInfoFreeGemAdsState() {
        if (currentOfferData != null && currentOfferData.offerID == OfferID.GemAds) {
            TicketADS.SetActive(ProfileManager.PlayerData.ResourceSave.GetADTicket() > 0);
            BlockADS.gameObject.SetActive(ProfileManager.PlayerData.ResourceSave.isWatchedFreeGemAds);
            notifyADS.gameObject.SetActive(!ProfileManager.PlayerData.ResourceSave.isWatchedFreeGemAds);
            btnWatchAds.gameObject.SetActive(!ProfileManager.PlayerData.ResourceSave.isWatchedFreeGemAds);
        }
    }
    public override void OnBuy() {
        base.OnBuy();
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        if (currentOfferData != null && currentOfferData.offerID == OfferID.GemAds) {
            OnClaimFreeCheck();

        } else MyIAPManager.instance.Buy(productID, OnBuySuccess);
    }
    void OnClaimFreeCheck() {
        if (!ProfileManager.PlayerData.ResourceSave.isWatchedFreeGemAds) {
            if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) {
                if (ProfileManager.PlayerData.ResourceSave.GetADTicket() > 0) {
                    ProfileManager.PlayerData.ResourceSave.ConsumeADTicket();
                    OnClaimedFreeGemAds();
                } else {
                    AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.FreeGemDaily.ToString(), () => {
                        OnClaimedFreeGemAds();
                    });
                }
            } else {
                OnClaimedFreeGemAds();
            }
        }
        LoadInfoFreeGemAdsState();
    }
    void OnClaimedFreeGemAds() {
        ProfileManager.PlayerData.ResourceSave.WatchedFreeGemAds();
        ProfileManager.PlayerData.AddGem(reward);
        OnBuySuccess();
        ABIAnalyticsManager.Instance.TrackEventGem(GemAction.Earn_Ads_Daily, reward);
    }
    void OnBuySuccess() {
        SoundManager.instance.PlaySoundEffect(SoundID.IAP);
        UIManager.instance.ShowUIPanelReward(currentOfferData.itemRewards);
    }

}
