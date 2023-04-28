using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardIAP : MonoBehaviour {
    CardIAPConfig _CurrentCardIAP;
    [SerializeField] Text nameText;
    [SerializeField] Text descriptionText;
    [SerializeField] Text priceText;
    [SerializeField] Text valueDescription;
    public int cardID;
    [SerializeField] Image icon;
    [SerializeField] Button buyButton;
    [SerializeField] GameObject[] rewardIcons;
    [SerializeField] Sprite[] sprsBtnBuyState;
    string strOwner;
    private void Awake() {
        buyButton.onClick.AddListener(BuyCardOffer);
    }
    TabName _shopName;
    public void InitData(CardIAPConfig cardIAP, TabName shopName) {
        _shopName = shopName;
        _CurrentCardIAP = cardIAP;
        buyButton.gameObject.SetActive(true);
        nameText.text = cardIAP.GetName().ToUpper();
        descriptionText.text = cardIAP.GetDes();
        string priceLocal = MyIAPManager.instance.GetProductPriceFromStore(cardIAP.productID);
        priceText.text = priceLocal != "$0.01" ? priceLocal : cardIAP.price.ToString();
        cardID = cardIAP.id;
        icon.sprite = cardIAP.icon;
        string strHour = " " + ProfileManager.Instance.dataConfig.GameText.GetTextByID(169).ToLower();
        if (shopName == TabName.OfflineTime) {
            valueDescription.text = "<color=#63FF26>+" + cardIAP.extraValue.ToString() + strHour + "</color>";
            rewardIcons[0].SetActive(true);
            rewardIcons[1].SetActive(false);
        } else {
            valueDescription.text = "<color=#63FF26>+" + cardIAP.extraValue.ToString() + "%</color>";
            rewardIcons[0].SetActive(false);
            rewardIcons[1].SetActive(true);
        }
    }
    
    private void OnEnable() {
        strOwner = ProfileManager.Instance.dataConfig.GameText.GetTextByID(183);
        if (_CurrentCardIAP != null) {
            nameText.text = _CurrentCardIAP.GetName().ToUpper();
            descriptionText.text = _CurrentCardIAP.GetDes();
            string strHour = " " + ProfileManager.Instance.dataConfig.GameText.GetTextByID(169).ToLower();
            if (_shopName == TabName.OfflineTime) {
                valueDescription.text = "<color=#63FF26>+" + _CurrentCardIAP.extraValue.ToString() + strHour + "</color>";
            } 
        }
    }
    private void Update() {
        if (ProfileManager.PlayerData.GetCardManager().IsOwnedCardIAP(_CurrentCardIAP.id)) {
            buyButton.interactable = false;
            buyButton.GetComponent<Image>().sprite = sprsBtnBuyState[1];
            priceText.text = strOwner;
        } else {
            buyButton.interactable = true;
            buyButton.GetComponent<Image>().sprite = sprsBtnBuyState[0];
        }
    }
    void BuyCardOffer() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        MyIAPManager.instance.Buy(_CurrentCardIAP.productID, BuyCardOfferSuccess);
    }
    void BuyCardOfferSuccess() {
        SoundManager.instance.PlaySoundEffect(SoundID.IAP);
       
        buyButton.interactable = false;
        buyButton.GetComponent<Image>().sprite = sprsBtnBuyState[1];
        priceText.text = strOwner;
        if (_CurrentCardIAP.type == CardIAPType.FINANCIAL_MANAGER) {
            GameManager.instance.UpdateFinanceRate();
        }
    }

}
