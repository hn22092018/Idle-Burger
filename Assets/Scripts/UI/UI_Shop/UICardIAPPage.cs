using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardIAPPage : UIShopPage {

    public List<UICardIAP> cardBuffOffers;
    public GameObject cardOfferPrefab;
    public Text txtTotalBuff;
    /// <summary>
    /// Init card IAP in shop check owned card first if player has it let's init next data.
    /// </summary>
    /// <param name="cardIAPs"></param>
    public override void InitData() {
        ShopData shopData = ProfileManager.Instance.dataConfig.shopConfig;
        List<CardIAPConfig> cardIApList = shopData.cardIAPList;
        List<CardIAPConfig> cardOfflineTimes = new List<CardIAPConfig>();
        List<CardIAPConfig> cardFinancials = new List<CardIAPConfig>();
        //Get card for each page
        for (int i = 0; i < cardIApList.Count; i++) {
            if (cardIApList[i].type == CardIAPType.FINANCIAL_MANAGER)
                cardFinancials.Add(cardIApList[i]);
            else cardOfflineTimes.Add(cardIApList[i]);
        }
        if (shopName == TabName.OfflineTime) {
            listCardIAPs = cardOfflineTimes;

        } else {
            listCardIAPs = cardFinancials;
        }
        for (int i = 0; i < listCardIAPs.Count; i++) {
            GameObject newGameObjectOfferCard = Instantiate(cardOfferPrefab, transform.position, Quaternion.identity, transform);
            UICardIAP newUICard = newGameObjectOfferCard.GetComponent<UICardIAP>();
            cardBuffOffers.Add(newUICard);
            newUICard.InitData(listCardIAPs[i], shopName);

        }
    }
    string sSub = "";
    private void OnEnable() {
        if (shopName == TabName.OfflineTime) sSub = ProfileManager.Instance.dataConfig.GameText.GetTextByID(181) + ": ";
        else if (shopName == TabName.Financial) sSub = ProfileManager.Instance.dataConfig.GameText.GetTextByID(182) + ": ";
    }
    private void Update() {
        if (shopName == TabName.OfflineTime) txtTotalBuff.text = sSub + ProfileManager.Instance.playerData.cardManager.GetExtraHour_OfflineTimeCardIAP().ToString() + "H";
        else if (shopName == TabName.Financial) txtTotalBuff.text = sSub + (ProfileManager.Instance.playerData.cardManager.GetFinanceRate_CardIAP() * 100).ToString() + "%";
    }

}
