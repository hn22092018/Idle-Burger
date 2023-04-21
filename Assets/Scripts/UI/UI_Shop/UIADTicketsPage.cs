using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIADTicketsPage : UIShopPage {
    public List<UIShopSlot> listSlots;
    public GameObject uiPrefab;
  
    public override void InitData() {
        ShopData shopData = ProfileManager.Instance.dataConfig.shopConfig;
        listOfferData = shopData.adTicketDatas;
        for (int i = 0; i < listOfferData.Count; i++) {
            GameObject newObj= Instantiate(uiPrefab, transform.position, Quaternion.identity, transform);
            UIShopSlot newUI = newObj.GetComponent<UIShopSlot>();
            listSlots.Add(newUI);
            newUI.InitData(listOfferData[i]);
        }
    }
}
