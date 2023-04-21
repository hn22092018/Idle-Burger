using DG.Tweening;
using SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOrderBookInDeivery : UIEffect {
    [SerializeField] Text txtCashProfit, txtBurgerPoint, txtProcessDelivery, txtProcessInfoDelivery;
    [SerializeField] Image imgIcon;
    [SerializeField] Button btnClaim, btnCancle, btnSkipGem;
    [SerializeField] Text txtGemSkipValue;
    [SerializeField] Image processSlider;
    Order currentOrder;
    string sComplete = "COMPLETE";
    int gemSkipValue;
    float lastProcess;
    private void Awake() {
        btnClaim.onClick.AddListener(OnClaimOrder);
        btnCancle.onClick.AddListener(OnCancleOrder);
        btnSkipGem.onClick.AddListener(OnSkipGem);
    }
    public void Setup(Order order) {
        currentOrder = order;
        txtCashProfit.text = order.cashProfit.ToString();
        txtBurgerPoint.text = order.bCoinProfit.ToString();
        imgIcon.sprite = currentOrder.sprOrderStaff;
        //sComplete = ProfileManager.Instance.dataConfig.GameText.GetTextByID(140);

    }
    private void Update() {
        float process = currentOrder.GetProcess();
        if (process >= 1) {
            btnSkipGem.gameObject.SetActive(false);
            btnCancle.gameObject.SetActive(false);
            btnClaim.gameObject.SetActive(true);
            txtProcessInfoDelivery.text = sComplete;
            processSlider.fillAmount = 1;
            txtProcessDelivery.text = currentOrder.bugerRequire + "/" + currentOrder.bugerRequire;

        } else {
            gemSkipValue = currentOrder.GetAmountNeedToCompleteOrder() / 2;
            btnSkipGem.gameObject.SetActive(true);
            btnSkipGem.interactable = GameManager.instance.IsEnoughGem(gemSkipValue);
            btnCancle.gameObject.SetActive(false);
            btnClaim.gameObject.SetActive(false);
            txtGemSkipValue.text = gemSkipValue.ToString();
            txtProcessInfoDelivery.text = (int)(process * 100) + "%";
            if (process != lastProcess) {
                lastProcess = process;
                processSlider.DOFillAmount(process, 0.3f).SetSpeedBased(true);

            }
            txtProcessDelivery.text = currentOrder.bugerCollected + "/" + currentOrder.bugerRequire;
        }
    }

    private void OnCancleOrder() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ScaleEffectButton(btnCancle, () => {
            ProfileManager.PlayerData.GetOrderBookManager().OnCancleOrder(currentOrder);
            PanelOrderBook.instance.RemoveUIInDeivery(this);
            PanelOrderBook.instance.ReloadUIOffer();
            Destroy(this.gameObject);
        });

    }
    private void OnClaimOrder() {
        SoundManager.instance.PlaySoundEffect(SoundID.CASH_COLLECT);
        ScaleEffectButton(btnClaim, () => {
            ProfileManager.PlayerData.GetOrderBookManager().OnClaimOrder(currentOrder);
            PanelOrderBook.instance.RemoveUIInDeivery(this);
            PanelOrderBook.instance.ReloadUIOffer();
            Destroy(this.gameObject);
        });

    }
    private void OnSkipGem() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ScaleEffectButton(btnSkipGem, () => {
            btnSkipGem.gameObject.SetActive(false);
            btnClaim.gameObject.SetActive(true);
            ProfileManager.PlayerData.GetOrderBookManager().OnForceDoneOrder(currentOrder);
        });
        ProfileManager.PlayerData.ConsumeGem(gemSkipValue);
    }
}
