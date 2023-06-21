using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopIAPPackage : MonoBehaviour {
    [SerializeField] Text txtTitle;
    [SerializeField] Text txtPrice;
    [SerializeField] Button btnBuy;
    [SerializeField] Text txtTimeSale;
    public OfferID offerID;
    bool hasSkin = false;
    OfferData currentIAPPackage;
    void Awake() {
        btnBuy.onClick.AddListener(OnBuy);
    }
    // Start is called before the first frame update
    void OnEnable() {
        OfferData data = ProfileManager.Instance.dataConfig.shopConfig.GetIAPPackageByOfferID(offerID);
        currentIAPPackage = data;
        txtTitle.text = data.titleDeal;
        string priceLocal = MyIAPManager.instance.GetProductPriceFromStore(data.productID);
        txtPrice.text = priceLocal != "$0.01" ? priceLocal : data.price.ToString() + "$";
    }
    private void Update() {
        if (ProfileManager.PlayerData.IsBoughtIAPPackage(offerID)) gameObject.SetActive(false);
    }
    public void OnBuy() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        //if (offerID == OfferID.Vip1Pack) {
        //    if (!ProfileManager.PlayerData.ResourceSave.activePremiumSuit) {
        //        hasSkin = true;
        //    } else {
        //        hasSkin = false;
        //    }
        //} else {
        //    hasSkin = false;
        //}
        MyIAPManager.instance.Buy(currentIAPPackage.productID, OnBuySuccess);
    }
    public void OnBuySuccess() {
        SoundManager.instance.PlaySoundEffect(SoundID.IAP);
        UIManager.instance.ShowUIPanelReward(currentIAPPackage.itemRewards, hasSkin);
        if (ProfileManager.PlayerData.IsBoughtIAPPackage(offerID)) this.gameObject.SetActive(false);
    }
}
