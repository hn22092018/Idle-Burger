using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIADTicketsSlot : UIShopSlot {
    int reward;
    string productID;
    OfferData currentOfferData;
    public override void InitData(OfferData offerData) {
        base.InitData(offerData);
        currentOfferData = offerData;
        reward = offerData.itemRewards[0].amount;
        titleText.text = reward.ToString() + " " + ProfileManager.Instance.dataConfig.GameText.GetTextByID(168).ToUpper();
        productID = offerData.productID;
        string priceLocal = MyIAPManager.instance.GetProductPriceFromStore(productID);
        priceText.text = priceLocal != "$0.01" ? priceLocal : offerData.price.ToString() + "$";
    }
    private void OnEnable() {
        if (currentOfferData == null) return;
        titleText.text = reward.ToString() + " " + ProfileManager.Instance.dataConfig.GameText.GetTextByID(168).ToUpper();
    }
    public override void OnBuy() {
        base.OnBuy();
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        MyIAPManager.instance.Buy(productID, OnBuySuccess);
    }
    void OnBuySuccess() {
        SoundManager.instance.PlaySoundEffect(SoundID.IAP);
        UIManager.instance.ShowUIPanelReward(currentOfferData.itemRewards);
    }
}
