using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum UIResearchCustomerPackType {
    ADS, Pack1, Pack2
}
public class UICustomerResearchPack : MonoBehaviour {
    public UIResearchCustomerPackType type;
    public GameObject notifyADS;
    public GameObject BlockADS;
    public GameObject TicketADS;
    public GameObject btnFree;
    public Text txtFreeADS;
    public Button btnBuy,subBtnBuy;
    public Text txtPrice,txtTitle;
    int Max_Free_Ads = 5;
    int Customer_Ads_Earn = 20;
    int Customer_Pack1_Earn = 100;
    int Customer_Pack2_Earn = 300;
    int Price_Pack1 = 50;
    private void Awake() {
        btnBuy.onClick.AddListener(OnBuy);

    }
    private void OnEnable() {
        LoadInfo();
    }
    private void LoadInfo() {

        switch (type) {
            case UIResearchCustomerPackType.ADS:
                txtTitle.text = "+" + Customer_Ads_Earn;
                TicketADS.SetActive(ProfileManager.PlayerData.ResourceSave.GetADTicket() > 0);
                BlockADS.gameObject.SetActive(ProfileManager.PlayerData.researchManager.Free_Customer_Ads_Watched >= Max_Free_Ads);
                notifyADS.gameObject.SetActive(ProfileManager.PlayerData.researchManager.Free_Customer_Ads_Watched < Max_Free_Ads || ProfileManager.PlayerData.researchManager.Free_Customer_NonAds > 0);
                txtFreeADS.text = "" + (Max_Free_Ads - ProfileManager.PlayerData.researchManager.Free_Customer_Ads_Watched) + "/" + Max_Free_Ads;
                btnFree.gameObject.SetActive(ProfileManager.PlayerData.researchManager.Free_Customer_NonAds > 0);
                break;
            case UIResearchCustomerPackType.Pack1:
                txtTitle.text = "+" + Customer_Pack1_Earn;
                if (subBtnBuy != null) subBtnBuy.interactable = GameManager.instance.IsEnoughGem(Price_Pack1);
                txtPrice.text = Price_Pack1.ToString();

                break;
            case UIResearchCustomerPackType.Pack2:
                txtTitle.text = "+" + Customer_Pack2_Earn;
                txtPrice.text = MyIAPManager.instance.GetProductPriceFromStore(MyIAPManager.product_research_customer_pack);
                break;
        }
    }
    void OnBuy() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        switch (type) {
            case UIResearchCustomerPackType.ADS:
                if (ProfileManager.PlayerData.researchManager.Free_Customer_NonAds > 0) {
                    ProfileManager.PlayerData.researchManager.Free_Customer_NonAds--;
                    ProfileManager.PlayerData.researchManager.AddResearchValue(Customer_Ads_Earn);
                } else if (ProfileManager.PlayerData.researchManager.Free_Customer_Ads_Watched < Max_Free_Ads) {
                    if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) {
                        if (ProfileManager.PlayerData.ResourceSave.GetADTicket() > 0) {
                            ProfileManager.PlayerData.researchManager.Free_Customer_Ads_Watched++;
                            ProfileManager.PlayerData.ResourceSave.ConsumeADTicket();
                            ProfileManager.PlayerData.researchManager.AddResearchValue(Customer_Ads_Earn);
                            TicketADS.SetActive(ProfileManager.PlayerData.ResourceSave.GetADTicket() > 0);
                        } else {
                            AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.Research_FreeCustomer.ToString(), () => {
                                ProfileManager.PlayerData.researchManager.Free_Customer_Ads_Watched++;
                                ProfileManager.PlayerData.researchManager.AddResearchValue(Customer_Ads_Earn);
                            });
                            ABIAnalyticsManager.Instance.TrackEventResearch(ResearchAction.WatchAds_Repulation);
                        }
                    } else {
                        ProfileManager.PlayerData.researchManager.Free_Customer_Ads_Watched++;
                        ProfileManager.PlayerData.researchManager.AddResearchValue(Customer_Ads_Earn);
                    }
                }
                notifyADS.gameObject.SetActive(ProfileManager.PlayerData.researchManager.Free_Customer_Ads_Watched < Max_Free_Ads || ProfileManager.PlayerData.researchManager.Free_Customer_NonAds > 0);
                txtFreeADS.text = "" + (Max_Free_Ads - ProfileManager.PlayerData.researchManager.Free_Customer_Ads_Watched) + "/" + Max_Free_Ads;
                BlockADS.gameObject.SetActive(ProfileManager.PlayerData.researchManager.Free_Customer_Ads_Watched >= Max_Free_Ads);
                btnFree.gameObject.SetActive(ProfileManager.PlayerData.researchManager.Free_Customer_NonAds > 0);
                break;
            case UIResearchCustomerPackType.Pack1:
                if (GameManager.instance.IsEnoughGem(Price_Pack1)) {
                    ProfileManager.PlayerData.researchManager.AddResearchValue(Customer_Pack1_Earn);
                    ProfileManager.PlayerData.ConsumeGem(Price_Pack1);
                    ABIAnalyticsManager.Instance.TrackEventResearch(ResearchAction.UseGem_Repulation);
                    ABIAnalyticsManager.Instance.TrackEventGem(GemAction.Spend_BuyReputation, Price_Pack1);

                }
                if (subBtnBuy != null) subBtnBuy.interactable = GameManager.instance.IsEnoughGem(Price_Pack1);
                break;
            case UIResearchCustomerPackType.Pack2:
                MyIAPManager.instance.Buy(MyIAPManager.product_research_customer_pack, () => {
                    ProfileManager.PlayerData.researchManager.AddResearchValue(Customer_Pack2_Earn);
                    ABIAnalyticsManager.Instance.TrackEventResearch(ResearchAction.BuyIAP_Repulation);
                });
                break;
        }
    }
}
