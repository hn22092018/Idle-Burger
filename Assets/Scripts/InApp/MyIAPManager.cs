using SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class MyIAPManager : MonoBehaviour, IStoreListener {

    private IStoreController m_StoreController;
    private IExtensionProvider m_StoreExtensionProvider;
    public static MyIAPManager instance;
    public const string product_Gem1 = "gem1";
    public const string product_Gem2 = "gem2";
    public const string product_Gem3 = "gem3";
    public const string product_Gem4 = "gem4";
    public const string product_Gem5 = "gem5";
    public const string product_Gem6 = "gem6";
    public const string product_Finance1 = "finance1";
    public const string product_Offline1 = "offline1";

    public const string product_vip1pack = "vip_pack1";
    public const string product_vip2pack = "vip_pack2";
    public const string product_vip3pack = "vip_pack3";

    public const string product_advancedChestPack = "advanced_chest";
    public const string product_TimeSkip1Pack = "time_skip1";
    public const string product_adsTicket1 = "ads_ticket_1";
    public const string product_adsTicket2 = "ads_ticket_2";
    public const string product_adsTicket3 = "ads_ticket_3";
    public const string product_noads = "remove_ads";

    public const string product_research_customer_pack = "research_customer_pack";
    public const string product_researcher_pack = "research_researcher_pack";
    public const string product_warehouse_DeliciousPack = "warehouse_pack1";
    public const string product_warehouse_YummyPackage = "warehouse_pack2";
    public const string product_warehouse_SuperTastyPackage = "combo_pack2";
    public const string product_combo_ads = "combo_pack_remove_ads";
    public const string product_order_staff_pack = "orderstaff_pack";

    UnityAction buyFailed, buySuccess;
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    void Start() {
        if (m_StoreController == null) {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }
    public void CheckNonComsumePack() {
        if (m_StoreController == null) return;
        Product product = m_StoreController.products.WithID(product_noads);
        if (product != null && product.hasReceipt) {
            ProfileManager.PlayerData.ResourceSave.SetRemoveAds(true);
        }
        
        Product product1 = m_StoreController.products.WithID(product_Offline1);
        if (product1 != null && product1.hasReceipt) {
            int id = ProfileManager.Instance.dataConfig.shopConfig.GetCardByOfferID(CardIapProductType.OFFLINE_TIME_10).id;
            ProfileManager.PlayerData.GetCardManager().AddCardIAPOneTime(id);
        }
       
        Product product2 = m_StoreController.products.WithID(product_Finance1);
        if (product2 != null && product2.hasReceipt) {
            int id = ProfileManager.Instance.dataConfig.shopConfig.GetCardByOfferID(CardIapProductType.FINANCIAL_MANAGER_50).id;
            ProfileManager.PlayerData.GetCardManager().AddCardIAPOneTime(id);
        }
       
        Debug.Log("On Check Non Comsume Researcher pack");
        Product product3 = m_StoreController.products.WithID(product_researcher_pack);
        if (product3 != null && product3.hasReceipt) {
            ProfileManager.PlayerData.researchManager.OnBoughtResearcherPack();
        } else {
            Debug.Log("No Has Receipt Researcher pack 1");
        }
        Debug.Log("On Check Non Comsume Order Staff pack");
        Product product4 = m_StoreController.products.WithID(product_order_staff_pack);
        if (product4 != null && product4.hasReceipt) {
            ProfileManager.PlayerData.OrderSave.OnBoughtOrderStaffPack();
        } else {
            Debug.Log("No Has Receipt Order Staff pack ");
        }
        Product product5 = m_StoreController.products.WithID(product_combo_ads);
        if (product5 != null && product5.hasReceipt) {
            ProfileManager.PlayerData.ResourceSave.SetRemoveAds(true);
            ProfileManager.PlayerData.researchManager.OnBoughtResearcherPack();
        }
    }

    public void InitializePurchasing() {
        Debug.Log("InitializePurchasing");
        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(product_Gem1, ProductType.Consumable);
        builder.AddProduct(product_Gem2, ProductType.Consumable);
        builder.AddProduct(product_Gem3, ProductType.Consumable);
        builder.AddProduct(product_Gem4, ProductType.Consumable);
        builder.AddProduct(product_Gem5, ProductType.Consumable);
        builder.AddProduct(product_Gem6, ProductType.Consumable);
        builder.AddProduct(product_advancedChestPack, ProductType.Consumable);
        builder.AddProduct(product_adsTicket1, ProductType.Consumable);
        builder.AddProduct(product_adsTicket2, ProductType.Consumable);
        builder.AddProduct(product_adsTicket3, ProductType.Consumable);
        builder.AddProduct(product_TimeSkip1Pack, ProductType.Consumable);
        builder.AddProduct(product_vip1pack, ProductType.Consumable);
        builder.AddProduct(product_vip2pack, ProductType.Consumable);
        builder.AddProduct(product_vip3pack, ProductType.Consumable);
        builder.AddProduct(product_Finance1, ProductType.NonConsumable);
        builder.AddProduct(product_Offline1, ProductType.NonConsumable);
        builder.AddProduct(product_noads, ProductType.NonConsumable);
       
        builder.AddProduct(product_research_customer_pack, ProductType.Consumable);
        builder.AddProduct(product_researcher_pack, ProductType.NonConsumable);
        builder.AddProduct(product_warehouse_DeliciousPack, ProductType.Consumable);
        builder.AddProduct(product_warehouse_YummyPackage, ProductType.Consumable);
        builder.AddProduct(product_warehouse_SuperTastyPackage, ProductType.Consumable);
        builder.AddProduct(product_combo_ads, ProductType.NonConsumable);
        builder.AddProduct(product_order_staff_pack, ProductType.NonConsumable);
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized() {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
        this.m_StoreController = controller;
        this.m_StoreExtensionProvider = extensions;
        CheckNonComsumePack();
    }
    public void OnInitializeFailed(InitializationFailureReason error) {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
        Debug.Log("Process Purchase ........");
        bool validPurchase = true; // Presume valid for platforms with no R.V.
        if (!Application.isEditor) {
            // Unity IAP's validation logic is only included on these platforms.
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
            // Prepare the validator with the secrets we prepared in the Editor
            // obfuscation window.
            var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
                AppleTangle.Data(), Application.identifier);
            try {
                // On Google Play, result has a single product ID.
                // On Apple stores, receipts contain multiple products.
                var result = validator.Validate(args.purchasedProduct.receipt);
                // For informational purposes, we list the receipt(s)
                Debug.Log("Receipt is valid. Contents:");
                foreach (IPurchaseReceipt productReceipt in result) {
                    Debug.Log(productReceipt.productID);
                    Debug.Log(productReceipt.purchaseDate);
                    Debug.Log(productReceipt.transactionID);
                }
            } catch (IAPSecurityException) {
                Debug.Log("Invalid receipt, not unlocking content");
                validPurchase = false;
            }
#endif
        }

        if (validPurchase || Application.isEditor) {
            if (String.Equals(args.purchasedProduct.definition.id, CurrentProductID, StringComparison.Ordinal)) {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
                OnExecutePurchase(args.purchasedProduct.definition.id);
                TrackPurchaseSuccess(args);
            }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else {
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            }
        }

        return PurchaseProcessingResult.Complete;
    }
    private void OnExecutePurchase(string productID) {
        switch (productID) {
            case product_Gem1:
            case product_Gem2:
            case product_Gem3:
            case product_Gem4:
            case product_Gem5:
            case product_Gem6:
            case product_vip1pack:
            case product_vip2pack:
            case product_vip3pack:
            case product_advancedChestPack:
            case product_TimeSkip1Pack:
            case product_adsTicket1:
            case product_adsTicket2:
            case product_adsTicket3:
            case product_noads:
            case product_warehouse_DeliciousPack:
            case product_warehouse_YummyPackage:
            case product_warehouse_SuperTastyPackage:
            case product_combo_ads:
                OfferData offerData = ProfileManager.Instance.dataConfig.shopConfig.GetOfferDataByProductID(productID);
                GameManager.instance.OnCollectRewardIAPPackage(offerData);
                break;
            case product_Finance1:
            case product_Offline1:
                CardIAP cardIAP = ProfileManager.Instance.dataConfig.shopConfig.GetOfferCardIAPDataByProductID(productID);
                ProfileManager.PlayerData.GetCardManager().AddCardIAP(cardIAP.id);
                break;

        }
        if (buySuccess != null) buySuccess();
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        if (buyFailed != null) {
            buyFailed();
        }
    }
    private string CurrentProductID;
    public void Buy(string productId, UnityAction buySuccess, UnityAction buyFailed = null) {
        this.buyFailed = buyFailed;
        this.buySuccess = buySuccess;
        
        // If Purchasing has been initialized ...
        if (IsInitialized()) {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase) {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                CurrentProductID = productId;
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
#if UNITY_EDITOR
        OnExecutePurchase(productId);
#endif
    }
    public string GetProductPriceFromStore(string id) {
        if (m_StoreController != null && m_StoreController.products != null) {
            if (m_StoreController.products.WithID(id) == null) return "";
            return m_StoreController.products.WithID(id).metadata.localizedPriceString;

        } else
            return "";
    }
    public void TrackPurchaseSuccess(PurchaseEventArgs args) {
        string productID = args.purchasedProduct.definition.id;
        decimal cost = args.purchasedProduct.metadata.localizedPrice;
        string currencyCode = args.purchasedProduct.metadata.isoCurrencyCode;
        ABIAppsflyerManager.Instance.TrackAppflyerPurchase(productID, cost, currencyCode);
        string eventName = "iap_" + productID;
        //ABIFirebaseManager.Instance.LogFirebaseEvent(eventName);
        ABIAppsflyerManager.SendEvent(eventName);
    }
    public void RestorePurchases() {
        if (!IsInitialized()) {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer) {
            Debug.Log("RestorePurchases started ...");
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");

            });
        } else {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }
}
