using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelPremiumResearcherPack : UIPanel {
    //6$
    public Text txtPrice;
    public Button btnBuy;
    public Button btnClose;
    public override void Awake() {
        panelType = UIPanelType.PanelPremiumResearcherPack;
        base.Awake();
        btnBuy.onClick.AddListener(OnBuy);
        btnClose.onClick.AddListener(OnClose);
    }
    private void OnEnable() {
        if (ProfileManager.PlayerData.researchManager.IsBoughtResearcherPack()) {
            gameObject.SetActive(false);
        }
        txtPrice.text = MyIAPManager.instance.GetProductPriceFromStore(MyIAPManager.product_researcher_pack);
    }
    void OnBuy() {
        OfferData offerData = ProfileManager.Instance.dataConfig.shopConfig.GetResearcherPackByProductId(MyIAPManager.product_researcher_pack);
        MyIAPManager.instance.Buy(MyIAPManager.product_researcher_pack, () => {
            GameManager.instance.OnCollectRewardIAPPackage(offerData);
            ProfileManager.PlayerData.researchManager.OnBoughtResearcherPack();
            UIManager.instance.ShowUIPanelReward(offerData.itemRewards);
            UIManager.instance.ClosePanelPremiumResearcherPack();

        });

    }
    public void OnClose() {
        UIManager.instance.ClosePanelPremiumResearcherPack();
    }
}
