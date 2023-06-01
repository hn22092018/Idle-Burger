using DG.Tweening;
using SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOrderBookOffer : UIEffect {
    public Button[] btnOffers;
    public Button btnAccept, btnAcceptX2Ads, btnAcceptGem, btnReject, btnNextOffer, bntBackOffer;
    public Transform imgOfferSelected;
    public Image imgIconCharacterOffer;
    public Text txtRequire, txtOrderValue, txtBurgerPoint;
    public GameObject objReject, objGoodDeal;
    public List<Order> offers = new List<Order>();
    public Order currentOffer;
    int indexOrder;
    int gemPriceBestDeal = 10;
    private void Awake() {
        btnAccept.onClick.AddListener(OnAccept);
        btnAcceptX2Ads.onClick.AddListener(OnAcceptX2AdsOffer);
        btnReject.onClick.AddListener(OnRejectOffer);
        btnAcceptGem.onClick.AddListener(OnAcceptGem);
        btnNextOffer.onClick.AddListener(OnViewNextOffer);
        bntBackOffer.onClick.AddListener(OnViewBackOffer);
        btnOffers[0].onClick.AddListener(() => OnViewOffer(0));
        btnOffers[1].onClick.AddListener(() => OnViewOffer(1));
        btnOffers[2].onClick.AddListener(() => OnViewOffer(2));
    }
    public void InitOffer() {
        ProfileManager.PlayerData.GetOrderBookManager().CheckCreatOffers(PanelOrderBook.instance.sprCharacters);
        LoadListOffer(ProfileManager.PlayerData.GetOrderBookManager().currentOffers);
    }
    public void LoadListOffer(List<Order> list) {
        offers = list;
        currentOffer = offers[indexOrder];
        btnOffers[0].GetComponent<Image>().sprite = PanelOrderBook.instance.GetSpriteByName(offers[0].sprOrderStaffName);
        btnOffers[1].GetComponent<Image>().sprite = PanelOrderBook.instance.GetSpriteByName(offers[1].sprOrderStaffName);
        btnOffers[2].GetComponent<Image>().sprite = PanelOrderBook.instance.GetSpriteByName(offers[2].sprOrderStaffName);
        LoadOfferToUI();
    }
    void LoadOfferToUI() {
        txtRequire.text = currentOffer.bugerRequire + "";
        txtOrderValue.text = currentOffer.cashProfit.IntToString();
        txtBurgerPoint.text = currentOffer.bCoinProfit + "";
        objReject.SetActive(currentOffer.isReject);
        btnReject.gameObject.SetActive(!currentOffer.isReject);
        btnAccept.gameObject.SetActive(!currentOffer.isReject && currentOffer.isFreeAccept);
        btnAcceptX2Ads.gameObject.SetActive(!currentOffer.isReject && currentOffer.isFreeAccept);
        btnAcceptGem.gameObject.SetActive(!currentOffer.isFreeAccept && !currentOffer.isReject);
        btnAcceptGem.interactable = GameManager.instance.IsEnoughGem(gemPriceBestDeal);
        imgOfferSelected.SetParent(btnOffers[indexOrder].transform);
        imgOfferSelected.localPosition = Vector3.zero;
        imgIconCharacterOffer.sprite = PanelOrderBook.instance.GetSpriteByName(currentOffer.sprOrderStaffName);
        objGoodDeal.SetActive(!currentOffer.isFreeAccept);

    }
    public void OnViewOffer(int index) {
        ScaleEffectButton(btnOffers[index]);
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        indexOrder = index;
        currentOffer = offers[indexOrder];
        LoadOfferToUI();
    }
    private void OnViewBackOffer() {
        ScaleEffectButton(bntBackOffer);
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        indexOrder--;
        if (indexOrder < 0) indexOrder = 2;
        currentOffer = offers[indexOrder];
        LoadOfferToUI();
    }

    private void OnViewNextOffer() {
        ScaleEffectButton(btnNextOffer);
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        indexOrder++;
        if (indexOrder > 2) indexOrder = 0;
        currentOffer = offers[indexOrder];
        LoadOfferToUI();
    }

    private void OnRejectOffer() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ScaleEffectButton(btnReject, () => {
            ProfileManager.PlayerData.GetOrderBookManager().OnRejectOffer(currentOffer);
            btnReject.gameObject.SetActive(false);
            objReject.SetActive(true);
            PanelOrderBook.instance.ReloadUIOffer();
        });
    }

    private void OnAcceptX2AdsOffer() {
        ScaleEffectButton(btnAcceptX2Ads);
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.X2ValueOffer.ToString(), () => {
            ProfileManager.PlayerData.GetOrderBookManager().OnAcceptOffer(currentOffer, 2);
            PanelOrderBook.instance.ReloadUIInDelivery();
            PanelOrderBook.instance.ReloadUIOffer();
        });
        else {
            ProfileManager.PlayerData.GetOrderBookManager().OnAcceptOffer(currentOffer, 2);
            PanelOrderBook.instance.ReloadUIOffer();
        }
    }

    private void OnAccept() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ScaleEffectButton(btnAccept, () => {
            ProfileManager.PlayerData.GetOrderBookManager().OnAcceptOffer(currentOffer, 1);
            PanelOrderBook.instance.ReloadUIInDelivery();
            PanelOrderBook.instance.ReloadUIOffer();
        });

    }
    private void OnAcceptGem() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ScaleEffectButton(btnAcceptGem, () => {
            ProfileManager.PlayerData.GetOrderBookManager().OnAcceptOffer(currentOffer, 1);
            ProfileManager.PlayerData.ConsumeGem(gemPriceBestDeal);
            PanelOrderBook.instance.ReloadUIInDelivery();
            PanelOrderBook.instance.ReloadUIOffer();
        });

    }


}
