using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChestPage : UIShopPage {
    public GameObject uiPrefab;
    public override void InitData() {
        ShopData shopData = ProfileManager.Instance.dataConfig.shopConfig;
        listOfferData = shopData.chestOfferDatas;
        for (int i = 0; i < listOfferData.Count; i++) {
            GameObject newGameObjectOfferCard = Instantiate(uiPrefab, transform.position, Quaternion.identity, transform);
            UIChestSlot newUI = newGameObjectOfferCard.GetComponent<UIChestSlot>();
            newUI.InitData(listOfferData[i]);
        }
    }
}
