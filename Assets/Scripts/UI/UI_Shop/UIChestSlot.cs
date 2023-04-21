using SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIChestSlot : MonoBehaviour {
    [SerializeField] Image icon;
    [SerializeField] Button btnBuy;
    [SerializeField] GameObject btnOff;
    [SerializeField] Text priceText;
    [SerializeField] Text titleText;
    [SerializeField] GameObject adsGroup;
    [SerializeField] GameObject btnWatchAds;
    [SerializeField] GameObject btnOpenFree;
    [SerializeField] GameObject btnOutOfBox;
    [SerializeField] GameObject btnFreeBoxNotify;
    [SerializeField] Button btnSkipBlockTicket;
    [SerializeField] GameObject objChestCount;
    [SerializeField] Text txtChestCount;
    [SerializeField] Text txtCooldownAds;

    public OfferData currentOffer;
    int price;
    ItemReward chestReward;
    UnityAction onClickAction;


    public void InitData(OfferData offerData) {
        currentOffer = offerData;
        btnBuy.onClick.RemoveAllListeners();
        btnBuy.onClick.AddListener(OnButtonClick);
        txtCooldownAds.gameObject.SetActive(false);
        icon.sprite = offerData.icon;
        priceText.text = offerData.price.ToString();
        titleText.text = offerData.GetTitleDeal();
        price = (int)offerData.price;
        chestReward = offerData.itemRewards[0];
        if (offerData.offerID == OfferID.FreeChest) {
            objChestCount.SetActive(true);
            adsGroup.gameObject.SetActive(true);
        } else {
            onClickAction = OnBuyGem;
            adsGroup.gameObject.SetActive(false);
        }

    }
    private void OnEnable() {
        if (currentOffer != null) {
            titleText.text = currentOffer.GetTitleDeal();
        }
    }
    private void Update() {
        btnOff.SetActive(!GameManager.instance.IsEnoughGem(price));
        if (currentOffer.offerID == OfferID.FreeChest) {
            float blockTime = ProfileManager.PlayerData.boxManager.timeBlockFreeChest;
            int boxAvailable = ProfileManager.PlayerData.boxManager.freeChestAvaible;
            txtChestCount.text = ProfileManager.PlayerData.boxManager.freeChestAvaible.ToString();
            if (boxAvailable > 0) {
                btnFreeBoxNotify.SetActive(true);
                btnOutOfBox.SetActive(false);
                btnWatchAds.SetActive(blockTime > 0);
                btnOpenFree.SetActive(blockTime <= 0);
                txtCooldownAds.text = TimeUtil.RemainTimeToString2(blockTime);
                txtCooldownAds.gameObject.SetActive(blockTime > 0);
                onClickAction = blockTime > 0 ? OnWatchAdsChest : OnOpenFreeChest;
                btnSkipBlockTicket.gameObject.SetActive(blockTime > 0 && ProfileManager.PlayerData.ResourceSave.GetADTicket() > 0);
            } else {
                btnFreeBoxNotify.SetActive(false);
                btnWatchAds.SetActive(false);
                btnOutOfBox.SetActive(true);
                btnOpenFree.SetActive(false);
                txtCooldownAds.gameObject.SetActive(false);
                btnSkipBlockTicket.gameObject.SetActive(false);
                onClickAction = null;
            }
        } else if (currentOffer.offerID == OfferID.NormalChest) {
            int freeChest = ProfileManager.PlayerData.ResourceSave.GetNormalChest();
            objChestCount.SetActive(freeChest > 0);
            btnOpenFree.SetActive(freeChest > 0);
            txtChestCount.text = freeChest + "";
            onClickAction = freeChest > 0 ? OnOpenFreeChestNormal : OnBuyGem;
            if(freeChest > 0)
            {
                btnFreeBoxNotify.SetActive(true);
            }
            else
            {
                btnFreeBoxNotify.SetActive(false);
            }

        } else if (currentOffer.offerID == OfferID.AdvancedChest) {
            int freeChest = ProfileManager.PlayerData.ResourceSave.GetAdvancedChest();
            objChestCount.SetActive(freeChest > 0);
            btnOpenFree.SetActive(freeChest > 0);
            txtChestCount.text = freeChest + "";
            onClickAction = freeChest > 0 ? OnOpenFreeChestAdvanced: OnBuyGem;
            if (freeChest > 0)
            {
                btnFreeBoxNotify.SetActive(true);
            }
            else
            {
                btnFreeBoxNotify.SetActive(false);
            }
        }
    }
    public void OnButtonClick() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        if (onClickAction != null) onClickAction();
    }
    public void OnBuyGem() {
        UIManager.instance.ShowConfirmPanel();
        UIManager.instance.GetPanel(UIPanelType.PanelConfirmOpenChest).GetComponent<PanelConfirmOpenChest>().LoadConfirm(icon.sprite, titleText.text, price, chestReward.type);
    }
    void OnWatchAdsChest() {
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.SkipBlockChest.ToString(), OnWatchAdsChestSuccess);
        else OnWatchAdsChestSuccess();
    }
    void OnWatchAdsChestSuccess() {
        ProfileManager.PlayerData.boxManager.ForceSkipBlockTimeFreeChest();
    }
    public void OnSkipBlockTicket() {
        ProfileManager.PlayerData.ResourceSave.ConsumeADTicket();
        ProfileManager.PlayerData.boxManager.ForceSkipBlockTimeFreeChest();
        btnSkipBlockTicket.gameObject.SetActive(false);
    }
    void OnOpenFreeChest() {
        ProfileManager.Instance.playerData.boxManager.OpenBox(ItemType.FreeChest);
        ProfileManager.PlayerData.boxManager.ClaimFreeChest();
    }
    void OnOpenFreeChestNormal() {
        ProfileManager.PlayerData.ResourceSave.ConsumeNormalChest();
        ProfileManager.Instance.playerData.boxManager.OpenBox(ItemType.NormalChest);
    }
    void OnOpenFreeChestAdvanced() {
        ProfileManager.PlayerData.ResourceSave.ConsumeAdvancedChest();
        ProfileManager.Instance.playerData.boxManager.OpenBox(ItemType.AdvancedChest);
    }
}
