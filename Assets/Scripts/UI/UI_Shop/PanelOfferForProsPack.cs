using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelOfferForProsPack : UIPanel {
    //6$
    public Text txtPrice;
    public Button btnBuy;
    public Button btnClose;
    public override void Awake() {
        panelType = UIPanelType.PanelOfferForProsPack;
        base.Awake();
        btnBuy.onClick.AddListener(OnBuy);
        btnClose.onClick.AddListener(OnClose);
    }
    private void OnEnable() {
        if (ProfileManager.PlayerData.ResourceSave.isBoughtOfferProsPack) {
            gameObject.SetActive(false);
        }
        txtPrice.text = MyIAPManager.instance.GetProductPriceFromStore(MyIAPManager.product_vip2pack);
    }
    void OnBuy() {
        OfferData offerData = ProfileManager.Instance.dataConfig.shopConfig.GetIAPPackageByOfferID(OfferID.OfferForPros);
        MyIAPManager.instance.Buy(MyIAPManager.product_vip2pack, () => {
            GameManager.instance.OnCollectRewardIAPPackage(offerData);
            ProfileManager.PlayerData.researchManager.OnBoughtResearcherPack();
            UIManager.instance.ShowUIPanelReward(offerData.itemRewards);
            UIManager.instance.ClosePanelOfferForProsPack();

        });

    }
    public void OnClose() {
        UIManager.instance.ClosePanelOfferForProsPack();
    }
}
