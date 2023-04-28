using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManagerCardBuyResources : UIPanel {
    public Button btnLevelUp, btnClose;
    public Text txtCard, txtBurger, txtGemPrice;
    public Image imgCardPieceIcon;
    int cardMissing, burgerMissing, gemPrice;
    public RectTransform rectBtnLevelUp, rectResources;
    ManagerStaffID currentStaffID;
    public override void Awake() {
        panelType = UIPanelType.PanelManagerCardBuyResources;
        base.Awake();
        btnLevelUp.onClick.AddListener(OnLevelUp);
        btnClose.onClick.AddListener(OnClose);
    }
    CardManagerSave cardSaveInfo;
    private void OnEnable() {
        InitResource();
    }
    public void InitResource() {
        currentStaffID = GameManager.instance.selectedRoom.GetManagerStaffID();
        cardSaveInfo = ProfileManager.PlayerData.GetCardManager().GetCardManager(currentStaffID);
        CardManagerConfig cardConfig = ProfileManager.Instance.dataConfig.cardData.GetCardManagerInfo(cardSaveInfo.staffID, cardSaveInfo.rarity);
        cardMissing = ProfileManager.Instance.dataConfig.cardData.GetCardManagerRequireLevelUp(cardSaveInfo.level) - cardSaveInfo.cardAmount;
        burgerMissing = ProfileManager.Instance.dataConfig.cardData.GetBurgerCoinRequireLevelUp(cardSaveInfo.level) - ProfileManager.PlayerData.GetBurgerCoin();
        txtCard.gameObject.SetActive(cardMissing > 0);
        txtBurger.gameObject.SetActive(burgerMissing > 0);
        txtCard.text = cardMissing.ToString();
        txtBurger.text = burgerMissing.ToString();
        gemPrice = ProfileManager.Instance.dataConfig.cardData.GetGemPriceExchangeCard(cardMissing, cardSaveInfo.rarity) + ProfileManager.Instance.dataConfig.cardData.GetGemPriceExchangeBurgerCoin(burgerMissing);
        txtGemPrice.text = gemPrice.ToString();
        btnLevelUp.interactable = GameManager.instance.IsEnoughGem(gemPrice);
        imgCardPieceIcon.sprite = cardConfig.sprCardPieceIcon;
    }
    private void Update() {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectBtnLevelUp);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectResources);
    }

    private void OnClose() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ScaleEffectButton(btnClose, () => {
            UIManager.instance.ShowPanelManagerCardLevelUp();
            UIManager.instance.ClosePanelManagerCardBuyResources();
        });

    }

    private void OnLevelUp() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ScaleEffectButton(btnLevelUp, () => {
            if (burgerMissing > 0) ProfileManager.PlayerData.AddBCoin(burgerMissing);
            if (cardMissing > 0) ProfileManager.PlayerData.cardManager.AddCardManager(cardMissing, currentStaffID, cardSaveInfo.rarity);
            ProfileManager.PlayerData.ConsumeGem(gemPrice);
            UIManager.instance.ShowPanelManagerCardLevelUp();
            UIManager.instance.ClosePanelManagerCardBuyResources();
        });
    }
}
