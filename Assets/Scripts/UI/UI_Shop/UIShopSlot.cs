using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using System;

public class UIShopSlot : MonoBehaviour {
    public Image icon;
    public Button buyButton;
    public Text priceText;
    public Text titleText;
    protected OfferID offerID;
    public virtual void InitData(OfferData offerData) {
        icon.sprite = offerData.icon;
        priceText.text = offerData.price.ToString();
        titleText.text = offerData.titleDeal.ToString();
        offerID = offerData.offerID;
    }
    public virtual void InitData(OfferData offerData, bool haveTicket) { }
    public virtual void Awake() {
        buyButton.onClick.AddListener(OnBuy);
    }

    public virtual void OnBuy() {
    }

    public virtual void ChangeTimeTextOffer(string timeText) { }
}
