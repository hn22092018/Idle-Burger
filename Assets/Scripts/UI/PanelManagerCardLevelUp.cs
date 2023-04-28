using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManagerCardLevelUp : UIPanel {
    public Button btnLevelUp, btnClose;
    public RectTransform rectRequireResources;
    public Text txtCardName, txtCardManagerFunc, txtCardRarity;
    public Text txtIncomeCurrent, txtProcessingCurrent;
    public Text txtLevel;
    public Text txtLevelUp;
    public Text txtCardLevelUpRequire, txtBurgerPointLevelUpRequire;
    public Image imgCardIcon, imgRoom, imgCardPiece;
    CardManagerSave cardSaveInfo;
    CardManagerConfig cardConfig;
    int requireBurgerCoin, requireCard;
    bool isEnoughConditionToLevelUp;
    string sUnlock, sLevelup;
    public override void Awake() {
        panelType = UIPanelType.PanelManagerCardLevelUp;
        base.Awake();
        btnLevelUp.onClick.AddListener(OnLevelUp);
        btnClose.onClick.AddListener(OnClose);
    }
    private void OnEnable() {
        LoadCardManager();
    }
    void LoadCardManager() {
        sUnlock = "UNLOCK";
        sLevelup = "LEVEL UP";
        cardSaveInfo = ProfileManager.PlayerData.GetCardManager().GetCardManager(GameManager.instance.selectedRoom.GetManagerStaffID());
        cardConfig = ProfileManager.Instance.dataConfig.cardData.GetCardManagerInfo(cardSaveInfo.staffID, cardSaveInfo.rarity);
        LoadProfitsCard();
        LoadRequireLevelUp();


    }
    void LoadProfitsCard() {
        txtCardRarity.text = cardSaveInfo.rarity.ToString();
        txtCardName.text = cardConfig.name;
        imgCardIcon.sprite = cardConfig.sprIcon;
        txtCardManagerFunc.text = StaffIDToManagerFuncName(cardSaveInfo.staffID).ToUpper();
        if (cardSaveInfo.level < 10) {
            float incomeCurrent = ProfileManager.Instance.dataConfig.cardData.GetIncomeRateByLevel(cardSaveInfo.level);
            float incomeNext = ProfileManager.Instance.dataConfig.cardData.GetIncomeRateByLevel(cardSaveInfo.level + 1);
            txtIncomeCurrent.text = "<color=#FFC500>" + "x" + incomeCurrent + "</color>" + " " + "<color=#42FF07> (x" + incomeNext + ")</color>";

            float processingCurrent = ProfileManager.Instance.dataConfig.cardData.GetProcessingRateByLevel(cardSaveInfo.level);
            float processingNext = ProfileManager.Instance.dataConfig.cardData.GetProcessingRateByLevel(cardSaveInfo.level + 1);
            txtProcessingCurrent.text = "<color=#FFC500>" + "+" + processingCurrent + "%</color>" + " " + "<color=#42FF07> (+" + processingNext + "%)</color>";
            txtLevel.text = "LEVEL " + cardSaveInfo.level;

        } else {
            float incomeCurrent = ProfileManager.Instance.dataConfig.cardData.GetIncomeRateByLevel(cardSaveInfo.level);
            float processingCurrent = ProfileManager.Instance.dataConfig.cardData.GetProcessingRateByLevel(cardSaveInfo.level);
            txtIncomeCurrent.text = "<color=#FFC500>" + "x" + incomeCurrent + "</color>";
            txtProcessingCurrent.text = "<color=#FFC500>" + "+" + processingCurrent + "%</color>";
            txtLevel.text = "MAX LEVEL";
            btnLevelUp.gameObject.SetActive(false);
        }
        imgRoom.sprite = GameManager.instance.buildData.GetData(GameManager.instance.selectedRoom.GetRoomID()).sprBuild;
        imgCardPiece.sprite = cardConfig.sprCardPieceIcon;
    }
    void LoadRequireLevelUp() {
        int amount1 = ProfileManager.PlayerData.GetBurgerCoin();
        requireBurgerCoin = ProfileManager.Instance.dataConfig.cardData.GetBurgerCoinRequireLevelUp(cardSaveInfo.level);
        if (amount1 >= requireBurgerCoin) {
            txtBurgerPointLevelUpRequire.text = "<color=#FFFFFF>" + requireBurgerCoin + "</color>";
        } else txtBurgerPointLevelUpRequire.text = "<color=#FF4A25>" + requireBurgerCoin + "</color>";

        int amount2 = cardSaveInfo.cardAmount;
        requireCard = ProfileManager.Instance.dataConfig.cardData.GetCardManagerRequireLevelUp(cardSaveInfo.level);
        if (amount2 >= requireCard) {
            txtCardLevelUpRequire.text = "<color=#FFFFFF>" + amount2 + "/" + requireCard + "</color>";
        } else {
            txtCardLevelUpRequire.text = "<color=#FF4A25>" + amount2 + "</color>" + "<color=#FFFFFF>" + "/" + requireCard + "</color>";
        }
        txtBurgerPointLevelUpRequire.gameObject.SetActive(requireBurgerCoin > 0);
        txtCardLevelUpRequire.gameObject.SetActive(requireCard > 0);
        isEnoughConditionToLevelUp = amount1 >= requireBurgerCoin && amount2 >= requireCard;
        txtLevelUp.text = cardSaveInfo.level == 0 ? sUnlock : sLevelup;
    }

    string StaffIDToManagerFuncName(ManagerStaffID id) {
        return id switch {
            ManagerStaffID.Chef => "Kitchen Manager",
            ManagerStaffID.MainRoom_1 => "Direct Manager",
            ManagerStaffID.Deliver_1 => "Deliver Manager",
            ManagerStaffID.Restroom_1 => "Restroom Manager",
            _ => throw new NotImplementedException(),
        };
    }

    private void OnClose() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ScaleEffectButton(btnClose, () => {
            UIManager.instance.ClosePanelManagerCardLevelUp();
        });

    }

    private void OnLevelUp() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ScaleEffectButton(btnLevelUp, () => {
            if (isEnoughConditionToLevelUp) {
                ProfileManager.PlayerData.GetCardManager().ConsumeCardManager(requireCard, cardSaveInfo.staffID, cardSaveInfo.rarity);
                ProfileManager.PlayerData.GetCardManager().LevelUpCardManager(cardSaveInfo.staffID, cardSaveInfo.rarity);
                ProfileManager.PlayerData.ConsumeBCoin(requireBurgerCoin);
                LoadCardManager();
                UITabManagerInfo.instance.OnLoadManagerInfo();
            } else {
                UIManager.instance.ShowPanelManagerCardBuyResources(cardSaveInfo.staffID);
                UIManager.instance.ClosePanelManagerCardLevelUp();
            }

        });
    }
    private void Update() {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectRequireResources);
    }
}
