using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelIAPPackage : UIPanel
{
    [SerializeField] Button btnClose;
    [SerializeField] Text txtTitle;
    [SerializeField] Text txtPrice;
    [SerializeField] Button btnBuy;
    [SerializeField] GameObject[] objPackages;
    OfferData currentIAPPackage;
    OfferID currentOfferId;
    bool hasSkin = false;
    // Start is called before the first frame update
    public override void Awake()
    {
        panelType = UIPanelType.PanelIAPPackage;
        base.Awake();
        btnBuy.onClick.AddListener(OnBuy);
        btnClose.onClick.AddListener(OnClose);
    }
    public void SetupIAPPackge(OfferID offerID)
    {
        currentOfferId = offerID;
        //foreach (var obj in objPackages)
        //{
        //    obj.SetActive(false);
        //}
        //switch (offerID)
        //{
        //    case OfferID.Vip2Pack:
        //        txtTitle.text = ProfileManager.Instance.dataConfig.GameText.GetTextByID(152);
        //        objPackages[0].SetActive(true);
        //        break;
        //    case OfferID.Vip3Pack:
        //        txtTitle.text = ProfileManager.Instance.dataConfig.GameText.GetTextByID(153);
        //        objPackages[1].SetActive(true);
        //        break;
        //    case OfferID.AdvancedChestPack:
        //        txtTitle.text =  ProfileManager.Instance.dataConfig.GameText.GetTextByID(154);
        //        objPackages[2].SetActive(true);
        //        break;
        //}
        OfferData data = ProfileManager.Instance.dataConfig.shopConfig.GetIAPPackageByOfferID(offerID);
        currentIAPPackage = data;
     
        string priceLocal = MyIAPManager.instance.GetProductPriceFromStore(data.productID);
        txtPrice.text = priceLocal != "$0.01" ? priceLocal : data.price.ToString() + "$";
    }
    public void OnBuy()
    {
        if (currentOfferId == OfferID.Vip2Pack || currentOfferId == OfferID.Vip3Pack)
        {
            if (!ProfileManager.PlayerData.ResourceSave.activeGoldenSuit)
            {
                hasSkin = true;
            }
            else
            {
                hasSkin = false;
            }
        }
        else
        {
            hasSkin = false;
        }
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        MyIAPManager.instance.Buy(currentIAPPackage.productID, OnBuySuccess);
    }
    public void OnBuySuccess()
    {
        SoundManager.instance.PlaySoundEffect(SoundID.IAP);
        UIManager.instance.ShowUIPanelReward(currentIAPPackage.itemRewards, hasSkin);
        ProfileManager.Instance.playerData.SaveData();
        UIManager.instance.ClosePanelIAPPackage();
    }
    public void OnClose()
    {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        UIManager.instance.ClosePanelIAPPackage();
    }
}
