using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class SaleOfferManager {
    public List<TimeOfferLimit> timeOfferLimitsShop = new List<TimeOfferLimit>();
    int countUnable = 0;
    /// <summary>
    /// check time of offer: if it become zero and offer cansale ==> unsale and set time end = now + time exists
    ///                                        and offer unsalce ==> resale and set time end = now + time exists                 
    /// </summary>
    /// hide limit shop so return
    public void UpdateTime() {
        return;
        //foreach (TimeOfferLimit offerLimit in timeOfferLimitsShop)
        //{
        //    UIShopPage uiShopPage = UIShopManager.instance.GetUIShopPage(TabName.Limit);
        //    UIShopSlot slot = uiShopPage.GetOfferUISlot(offerLimit.offerId);
        //    if (slot == null)
        //        continue;
        //    if (Convert.ToDateTime(offerLimit.timeEnd) - DateTime.Now <= TimeSpan.Zero)
        //    {
        //        if (offerLimit.able)
        //            slot.OnClose();
        //        else
        //            slot.gameObject.SetActive(true);
        //        offerLimit.able = !offerLimit.able;
        //        offerLimit.timeEnd = DateTime.Now.AddHours(offerLimit.timeExists).ToString();
        //    }
        //}
        //if (CheckCountOfLimitTimeOffer())
        //{
        //    UIShopManager.instance.tabGroup.GetTabButton(TabName.Limit).OnClose();
        //    UIShopPage limitShopPage = UIShopManager.instance.GetUIShopPage(TabName.Limit);
        //    limitShopPage.GetComponent<RectTransform>().SetAsLastSibling();
        //    limitShopPage.OnClose();
        //}
        //else
        //{
        //    UIShopManager.instance.tabGroup.GetTabButton(TabName.Limit).OnOpen();
        //    UIShopPage limitShopPage = UIShopManager.instance.GetUIShopPage(TabName.Limit);
        //    limitShopPage.GetComponent<RectTransform>().SetAsFirstSibling();
        //    limitShopPage.OnOpen();
        //}
    }
    /// <summary>
    /// init data if it's the first time player play game
    /// </summary>
    public void InitData()
    {
        //PageData limitOfferData = ProfileManager.Instance.dataConfig.shopConfig.GetPageData(TabName.Limit);
        //foreach (OfferData offer in limitOfferData.offerDatas)
        //{
        //    TimeOfferLimit newOfferLimit = new TimeOfferLimit();
        //    newOfferLimit.offerId = offer.offerID;
        //    newOfferLimit.able = true;
        //    newOfferLimit.timeExists = offer.timeExits;
        //    newOfferLimit.timeEnd = DateTime.Now.AddHours(offer.timeExits).ToString();
        //    timeOfferLimitsShop.Add(newOfferLimit);
        //}
    }
    /// <summary>
    /// check if that offer able ==>return timeEnd
    /// else that offer will be off.
    /// </summary>
    /// <param name="offerID"></param>
    /// <returns></returns>
    public string GetOfferEndTime(OfferID offerID)
    {
        foreach (TimeOfferLimit offerLimit in timeOfferLimitsShop)
        {
            if (offerID == offerLimit.offerId)
                return offerLimit.timeEnd;
        }
        return "";
    }
    /// <summary>
    /// player buy or this offer end of time
    /// </summary>
    /// <param name="offerID"></param>
    public void BuyOrEndOfTime(OfferID offerID)
    {
        //foreach (TimeOfferLimit offerLimit in timeOfferLimitsShop)
        //{
        //    if (offerLimit.offerId == offerID)
        //    {
        //        offerLimit.able = false;
        //        UIShopPage uiShopPage = UIManager.instance.GetPanel(UIPanelType.PanelShop).GetComponent<UIShopManager>(). GetUIShopPage(TabName.Limit);
        //        UIShopSlot slot = uiShopPage.GetOfferUISlot(offerLimit.offerId);
        //        slot.OnClose();
        //        offerLimit.timeEnd = DateTime.Now.AddHours(offerLimit.timeExists).ToString();
        //        return;
        //    }
        //}
    }
    /// <summary>
    /// if player buy all of offer return true else return false
    /// </summary>
    /// <returns></returns>
    public bool CheckCountOfLimitTimeOffer() {
        countUnable = 0;
        for (int i = 0; i < timeOfferLimitsShop.Count; i++)
        {
            if (!timeOfferLimitsShop[i].able)
                countUnable++;
        }
        //if (countUnable >= ProfileManager.Instance.dataConfig.shopConfig.GetPageData(TabName.Limit).offerDatas.Count)
        //{
        //    return true;
        //}
        return false;
    }
    /// <summary>
    /// return true if time end and change able;
    /// </summary>
    /// <param name="offerID"></param>
    /// <returns></returns>
    public bool CheckTimeOffer(OfferID offerID) {
        foreach (TimeOfferLimit offerLimit in timeOfferLimitsShop)
        {
            if (offerID == offerLimit.offerId && 
                Convert.ToDateTime(offerLimit.timeEnd) - DateTime.Now <= TimeSpan.Zero)
                {
                    offerLimit.able = !offerLimit.able;
                    offerLimit.timeEnd = DateTime.Now.AddHours(offerLimit.timeExists).ToString();
                    return true;
                }
        }
        return false;
    }
}
