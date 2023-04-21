using SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOrderBookWaiting : UIEffect {
    public Button btnGetAds, btnGetGem, btnGetTicket;
    public Text txtTimeBlock;
    public Text txtGemPriceSkip;
    string sWaitToNewOrder = "New orders avaible in";
    string sMax = "Current orders is reached max";
    int gemPriceSkip = 10;
    // Start is called before the first frame update
    void Awake() {
        btnGetAds.onClick.AddListener(OnGetOrderAds);
        btnGetTicket.onClick.AddListener(OnGetOrderAdTicket);
        btnGetGem.onClick.AddListener(OnGetOrderGem);
    }

    private void OnEnable() {
        txtGemPriceSkip.text = gemPriceSkip.ToString();
        int adTicket = ProfileManager.PlayerData.ResourceSave.GetADTicket();
        btnGetTicket.gameObject.SetActive(adTicket > 0);
        btnGetAds.gameObject.SetActive(adTicket <= 0);
        btnGetGem.interactable = GameManager.instance.IsEnoughGem(gemPriceSkip);
    }
    private void OnGetOrderGem() {
        ScaleEffectButton(btnGetGem,()=> {
            ProfileManager.PlayerData.ResourceSave.ConsumeGem(gemPriceSkip);
            ProfileManager.PlayerData.GetOrderBookManager().SkipWaitToNewOffer();
            PanelOrderBook.instance.ReloadUIOffer();
        });
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
    }

    private void OnGetOrderAds() {
        ScaleEffectButton(btnGetGem);
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.GetNewOffer.ToString(), () => {
            ProfileManager.PlayerData.GetOrderBookManager().SkipWaitToNewOffer();
            PanelOrderBook.instance.ReloadUIOffer();
        });
        else {
            ProfileManager.PlayerData.GetOrderBookManager().SkipWaitToNewOffer();
            PanelOrderBook.instance.ReloadUIOffer();
        }
    }
    private void OnGetOrderAdTicket() {
        ScaleEffectButton(btnGetTicket, () => {
            ProfileManager.PlayerData.ResourceSave.ConsumeADTicket();
            ProfileManager.PlayerData.GetOrderBookManager().SkipWaitToNewOffer();
            PanelOrderBook.instance.ReloadUIOffer();
        });
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
    }

    // Update is called once per frame
    void Update() {
        if (ProfileManager.PlayerData.GetOrderBookManager().IsMaxOrder()) {
            txtTimeBlock.text = sMax;
            btnGetAds.gameObject.SetActive(false);
            btnGetGem.gameObject.SetActive(false);
            btnGetTicket.gameObject.SetActive(false);
        } else {
            txtTimeBlock.text = sWaitToNewOrder + " " + ProfileManager.PlayerData.GetOrderBookManager().GetTimeToNewOffer() + "s";
            int adTicket = ProfileManager.PlayerData.ResourceSave.GetADTicket();
            btnGetTicket.gameObject.SetActive(adTicket > 0);
            btnGetAds.gameObject.SetActive(adTicket <= 0);
            btnGetGem.gameObject.SetActive(true);
            btnGetGem.interactable = GameManager.instance.IsEnoughGem(gemPriceSkip);
        }

    }
}
