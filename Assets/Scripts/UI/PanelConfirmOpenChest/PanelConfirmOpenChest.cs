using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using SDK;

public class PanelConfirmOpenChest : UIPanel {
    [SerializeField] Image icon;
    [SerializeField] Text title, priceText, desText;
    [SerializeField] Button confirmButton, cancelButton;
    [SerializeField] Transform objDOTween;
    ItemType toOpenType;
    int chestPrice;
    public override void Awake() {
        panelType = UIPanelType.PanelConfirmOpenChest;
        base.Awake();
        confirmButton.onClick.AddListener(Confirm);
        cancelButton.onClick.AddListener(Cancel);
    }
    public void LoadConfirm(Sprite iconSprite, string offerName, int price, ItemType itemType) {
        title.text = offerName;
        chestPrice = price;
        icon.sprite = iconSprite;
        priceText.text = price.ToString();
        toOpenType = itemType;
        BoxInfo box = ProfileManager.Instance.playerData.boxManager.GetBoxInfo(toOpenType);
        desText.text = box.cardEarnAmount.ToString() + " Cards";
        UIManager.instance.dotweenManager.ShakeObj(objDOTween);
    }
    void Confirm() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        UIManager.instance.CloseConfirmPanel();
        ProfileManager.Instance.playerData.ConsumeGem(chestPrice);
        ProfileManager.Instance.playerData.boxManager.OpenBox(toOpenType);
        ABIAnalyticsManager.Instance.TrackEventGem(GemAction.Spend_OpenBox, chestPrice);
    }
    void Cancel() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        UIManager.instance.CloseConfirmPanel();
    }
    private void Update() {
        confirmButton.interactable = GameManager.instance.IsEnoughGem(chestPrice);
    }

}
