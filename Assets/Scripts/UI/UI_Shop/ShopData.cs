using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "New shop data", menuName = "ScriptableObjects/New Shop Data")]
public class ShopData : ScriptableObject {
    public Sprite[] sprConfig;
    public List<OfferData> chestOfferDatas = new List<OfferData>();
    public List<OfferData> gemOfferDatas = new List<OfferData>();
    public List<CardIAPConfig> cardIAPList = new List<CardIAPConfig>();
    public List<OfferData> IAPPackages = new List<OfferData>();
    public List<OfferData> adTicketDatas = new List<OfferData>();
    public List<OfferData> timeTravelDatas = new List<OfferData>();
    public List<OfferData> skinBoxDatas = new List<OfferData>();

    public void OnEnable() {
        LoadChest();
        LoadGem();
        LoadCardIAP();
        LoadIAPPackages();
        LoadAdTicket();
        LoadTimeTravel();
        LoadSkinBox();
    }
    void LoadCardIAP() {
        cardIAPList.Clear();
        //======OFFLINE
        CardIAPConfig card_off_1 = new CardIAPConfig();
        card_off_1.name = "Cary Grant";
        card_off_1.id = 2;
        card_off_1.icon = GetSpriteByName("cardiap_3");
        card_off_1.type = CardIAPType.OFFLINE_TIME;
        card_off_1.productType = CardIapProductType.OFFLINE_TIME_10;
        card_off_1.description = "This director knows how to manage a restaurant with a firm hand and will make the restaurant work for quite a long time while you're not playing.";
        card_off_1.DesLocalizeID = 195;
        card_off_1.extraValue = 10;
        card_off_1.price = "9.99$";
        card_off_1.productID = MyIAPManager.product_Offline1;

        //============ FINANCE
        CardIAPConfig card_finance_1 = new CardIAPConfig();
        card_finance_1.name = "ACCOUNTANT";
        card_finance_1.id = 4;
        card_finance_1.icon = GetSpriteByName("cardiap_5");
        card_finance_1.type = CardIAPType.FINANCIAL_MANAGER;
        card_finance_1.productType = CardIapProductType.FINANCIAL_MANAGER_100;
        card_finance_1.description = "This civil servant will use small accounting tricks to permanently increase profits.";
        card_finance_1.DesLocalizeID = 196;
        card_finance_1.extraValue = 100;
        card_finance_1.price = "5.99$";
        card_finance_1.productID = MyIAPManager.product_Finance1;

        cardIAPList.Add(card_off_1);
        cardIAPList.Add(card_finance_1);
    }
    public CardIAPConfig GetCardByID(int cardID) {
        foreach (CardIAPConfig card in cardIAPList) {
            if (card.id == cardID)
                return card;
        }
        return null;
    }
    public CardIAPConfig GetCardByOfferID(CardIapProductType type) {
        foreach (CardIAPConfig card in cardIAPList) {
            if (card.productType == type)
                return card;
        }
        return null;
    }

    void LoadGem() {
        gemOfferDatas.Clear();
        OfferData gem0 = new OfferData();
        gem0.icon = GetSpriteByName("GemPack01");
        gem0.productID = "";
        gem0.isSaleOff = false;
        gem0.titleDeal = "";
        gem0.price = 0f;
        gem0.offerID = OfferID.GemAds;
        gem0.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.Gem, amount=10, spr= GetSpriteByName("GemPack01")}
        };

        OfferData gem1 = new OfferData();
        gem1.icon = GetSpriteByName("GemPack02");
        gem1.productID = MyIAPManager.product_Gem1;
        gem1.isSaleOff = false;
        gem1.titleDeal = "";
        gem1.price = 1.99f;
        gem1.offerID = OfferID.Gem1;
        gem1.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.Gem, amount=50, spr= GetSpriteByName("GemPack02")}
        };

        OfferData gem2 = new OfferData();
        gem2.icon = GetSpriteByName("GemPack03BestDeal");
        gem2.productID = MyIAPManager.product_Gem2;
        gem2.isSaleOff = false;
        gem2.titleDeal = "";
        gem2.price = 4.99f;
        gem2.offerID = OfferID.Gem2;
        gem2.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.Gem, amount=150,spr= GetSpriteByName("GemPack03BestDeal")}
        };

        OfferData gem3 = new OfferData();
        gem3.icon = GetSpriteByName("GemPack04");
        gem3.productID = MyIAPManager.product_Gem3;
        gem3.isSaleOff = false;
        gem3.titleDeal = "";
        gem3.price = 9.99f;
        gem3.offerID = OfferID.Gem3;
        gem3.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.Gem, amount=320,spr= GetSpriteByName("GemPack04")}
        };

        OfferData gem4 = new OfferData();
        gem4.icon = GetSpriteByName("GemPack05");
        gem4.productID = MyIAPManager.product_Gem4;
        gem4.isSaleOff = false;
        gem4.titleDeal = "";
        gem4.price = 19.99f;
        gem4.offerID = OfferID.Gem4;
        gem4.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.Gem, amount=700,spr= GetSpriteByName("GemPack05")}
        };

        OfferData gem5 = new OfferData();
        gem5.icon = GetSpriteByName("GemPack06");
        gem5.productID = MyIAPManager.product_Gem5;
        gem5.isSaleOff = false;
        gem5.titleDeal = "";
        gem5.price = 49.99f;
        gem5.offerID = OfferID.Gem5;
        gem5.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.Gem, amount=2000,spr= GetSpriteByName("GemPack06")}
        };
        gemOfferDatas.Add(gem0);
        gemOfferDatas.Add(gem1);
        gemOfferDatas.Add(gem2);
        gemOfferDatas.Add(gem3);
        gemOfferDatas.Add(gem4);
        gemOfferDatas.Add(gem5);
    }
    void LoadChest() {
        chestOfferDatas.Clear();
        OfferData chest1 = new OfferData();
        chest1.icon = GetSpriteByName("chest_3");
        chest1.titleDeal = "Free";
        chest1.titleDealLocalizeID = 193;
        chest1.price = 0;
        chest1.offerID = OfferID.FreeChest;
        chest1.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.FreeChest, amount=1}
        };

        OfferData chest2 = new OfferData();
        chest2.icon = GetSpriteByName("chest_2");
        chest2.titleDeal = "Normal";
        chest2.titleDealLocalizeID = 192;
        chest2.price = 50;
        chest2.offerID = OfferID.NormalChest;
        chest2.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.NormalChest, amount=1}
        };

        OfferData chest3 = new OfferData();
        chest3.icon = GetSpriteByName("chest_1");
        chest3.titleDeal = "Advanced";
        chest3.titleDealLocalizeID = 191;
        chest3.price = 140;
        chest3.offerID = OfferID.AdvancedChest;
        chest3.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.AdvancedChest, amount=1}
        };

        chestOfferDatas.Add(chest1);
        chestOfferDatas.Add(chest2);
        chestOfferDatas.Add(chest3);
    }

    public void LoadIAPPackages() {
        IAPPackages.Clear();
        OfferData vip1pack = new OfferData();
        vip1pack.titleDeal = "OFFER FOR BEGINNERS";
        vip1pack.offerID = OfferID.OfferForBegginer;
        vip1pack.productID = MyIAPManager.product_vip1pack;
        vip1pack.price = 9.99f;
        List<ItemReward> rewards = new List<ItemReward>();
        // 5$
        rewards.Add(new ItemReward() { type = ItemType.Gem, amount = 100, spr = GetSpriteByName("GemPack06") });
        //5%
        rewards.Add(new ItemReward() {
            type = ItemType.OfflineTime, amount = 2
        , spr = GetSpriteByName("cardiap_1")
        });
        //3$
        rewards.Add(new ItemReward() { type = ItemType.IncreaseProfit, amount = 50, spr = GetSpriteByName("cardiap_5") });
        vip1pack.itemRewards = rewards;
        IAPPackages.Add(vip1pack);


        OfferData vip2pack = new OfferData();
        vip2pack.icon = null;
        vip2pack.offerID = OfferID.OfferForPros;
        vip2pack.productID = MyIAPManager.product_vip2pack;
        vip2pack.isSaleOff = false;
        vip2pack.titleDeal = "";
        vip2pack.price = 4.99f;
        vip2pack.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.Researcher, amount=1, spr = GetSpriteByName("Researcher_Icon")},
        };
        IAPPackages.Add(vip2pack);


        OfferData noAds = new OfferData();
        noAds.titleDeal = "REMOVE ADS";
        noAds.offerID = OfferID.NoAds;
        noAds.productID = MyIAPManager.product_noads;
        noAds.price = 14.99f;
        rewards = new List<ItemReward>();
        rewards.Add(new ItemReward() { type = ItemType.RemoveAds, amount = 1, spr = GetSpriteByName("remove-ads") });
        noAds.itemRewards = rewards;
        IAPPackages.Add(noAds);


        //OfferData timeSkipPack = new OfferData();
        //timeSkipPack.titleDeal = "HYPERSPACE OFFER";
        //timeSkipPack.offerID = OfferID.TimeSkipPack;
        //timeSkipPack.productID = MyIAPManager.product_TimeSkip1Pack;
        //timeSkipPack.price = 19.99f;
        //rewards = new List<ItemReward>();
        //rewards.Add(new ItemReward() { type = ItemType.TimeSkip_4H, amount = 6, spr = GetSpriteByName("timeTravel_2") });
        //rewards.Add(new ItemReward() { type = ItemType.TimeSkip_24H, amount = 9, spr = GetSpriteByName("timeTravel_3") });
        //timeSkipPack.itemRewards = rewards;
        //IAPPackages.Add(timeSkipPack);

        //OfferData noAds2 = new OfferData();
        //noAds2.titleDeal = "REMOVE ADS";
        //noAds2.offerID = OfferID.ComboPack_Ads_Researcher_Order;
        //noAds2.productID = MyIAPManager.product_combo_ads;
        //noAds2.price = 19.99f;
        //rewards = new List<ItemReward>();
        //rewards.Add(new ItemReward() { type = ItemType.RemoveAds, amount = 1, spr = GetSpriteByName("remove-ads") });
        //rewards.Add(new ItemReward() { type = ItemType.Researcher, amount = 2, spr = GetSpriteByName("Researcher_Icon") });
        //noAds2.itemRewards = rewards;
        //IAPPackages.Add(noAds2);

        //LoadWareHousePacks();

    }
    public OfferData GetIAPPackageByOfferID(OfferID id) {
        foreach (var pack in IAPPackages) {
            if (pack.offerID == id) return pack;
        }
        return null;
    }
    void LoadAdTicket() {
        adTicketDatas.Clear();
        OfferData ticket1 = new OfferData();
        ticket1.icon = GetSpriteByName("ticket_1");
        ticket1.productID = MyIAPManager.product_adsTicket1;
        ticket1.isSaleOff = false;
        ticket1.titleDeal = "";
        ticket1.price = 1.99f;
        ticket1.offerID = OfferID.ADTicket1;
        ticket1.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.ADTicket, amount=15, spr = GetSpriteByName("ticket_1") }
        };

        OfferData ticket2 = new OfferData();
        ticket2.icon = GetSpriteByName("ticket_2");
        ticket2.productID = MyIAPManager.product_adsTicket2;
        ticket2.isSaleOff = false;
        ticket2.titleDeal = "";
        ticket2.price = 2.99f;
        ticket2.offerID = OfferID.ADTicket2;
        ticket2.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.ADTicket, amount=25, spr = GetSpriteByName("ticket_2")}
        };

        OfferData ticket3 = new OfferData();
        ticket3.icon = GetSpriteByName("ticket_3");
        ticket3.productID = MyIAPManager.product_adsTicket3;
        ticket3.isSaleOff = false;
        ticket3.titleDeal = "";
        ticket3.price = 4.99f;
        ticket3.offerID = OfferID.ADTicket3;
        ticket3.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.ADTicket, amount=50, spr = GetSpriteByName("ticket_3")}
        };

        adTicketDatas.Add(ticket1);
        adTicketDatas.Add(ticket2);
        adTicketDatas.Add(ticket3);
    }
    void LoadTimeTravel() {
        timeTravelDatas.Clear();
        OfferData timeTravel1 = new OfferData();
        timeTravel1.icon = GetSpriteByName("timeTravel_1");
        timeTravel1.productID = "TimeSkip_30M";
        timeTravel1.isSaleOff = false;
        timeTravel1.titleDeal = "30M";
        timeTravel1.price = 60;
        timeTravel1.offerID = OfferID.TimeTrave_30M;

        OfferData timeTravel2 = new OfferData();
        timeTravel2.icon = GetSpriteByName("timeTravel_2");
        timeTravel2.productID = "TimeSkip_60M";
        timeTravel2.isSaleOff = false;
        timeTravel2.titleDeal = "1H";
        timeTravel2.price = 100;
        timeTravel2.offerID = OfferID.TimeTrave_60M;


        OfferData timeTravel3 = new OfferData();
        timeTravel3.icon = GetSpriteByName("timeTravel_3");
        timeTravel3.productID = "TimeSkip_4H";
        timeTravel3.isSaleOff = false;
        timeTravel3.titleDeal = "2 HOURS";
        timeTravel3.price = 180;
        timeTravel3.offerID = OfferID.TimeTrave_2H;


        timeTravelDatas.Add(timeTravel1);
        timeTravelDatas.Add(timeTravel2);
        timeTravelDatas.Add(timeTravel3);
    }
    void LoadSkinBox() {
        skinBoxDatas.Clear();
        OfferData chest1 = new OfferData();
        chest1.icon = GetSpriteByName("skin_1");
        chest1.titleDeal = "Normal";
        chest1.price = 300;
        chest1.offerID = OfferID.NormalSkinBox;
        chest1.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.NormalSkinBox, amount=1}
        };

        OfferData chest2 = new OfferData();
        chest2.icon = GetSpriteByName("skin_2");
        chest2.titleDeal = "Advanced";
        chest2.price = 700;
        chest2.offerID = OfferID.AdvancedSkinBox;
        chest2.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.AdvancedSkinBox, amount=1}
        };

        OfferData chest3 = new OfferData();
        chest3.icon = GetSpriteByName("skin_3");
        chest3.titleDeal = "Expert";
        chest3.price = 1200;
        chest3.offerID = OfferID.ExpertSkinBox;
        chest3.itemRewards = new List<ItemReward>() {
         new ItemReward(){type= ItemType.FreeSkinBox, amount=1}
        };

        skinBoxDatas.Add(chest1);
        skinBoxDatas.Add(chest2);
        skinBoxDatas.Add(chest3);
    }
    public Sprite GetSpriteByName(string name) {
        foreach (Sprite spr in sprConfig) {
            if (spr.name == name) return spr;
        }
        return null;
    }
    public OfferData GetOfferDataByProductID(string productID) {
        foreach (var offer in chestOfferDatas) {
            if (offer.productID == productID) return offer;
        }
        foreach (var offer in gemOfferDatas) {
            if (offer.productID == productID) return offer;
        }
        foreach (var offer in adTicketDatas) {
            if (offer.productID == productID) return offer;
        }
        foreach (var offer in IAPPackages) {
            if (offer.productID == productID) return offer;
        }
        return null;
    }
    public CardIAPConfig GetOfferCardIAPDataByProductID(string productID) {
        foreach (var offer in cardIAPList) {
            if (offer.productID == productID) return offer;
        }
        return null;
    }
  
    public ItemReward GetItemRewardFromGift(ItemPackage item) {
        ItemReward newItem = new ItemReward();
        newItem.type = item.type;
        newItem.amount = item.amount;

        switch (item.type) {
            case ItemType.Gem:
                if (item.amount <= 100) {
                    newItem.spr = GetSpriteByName("GemPack01");
                } else if (item.amount <= 400) {
                    newItem.spr = GetSpriteByName("GemPack02");
                } else if (item.amount <= 1500) {
                    newItem.spr = GetSpriteByName("GemPack03BestDeal");
                }
                break;
            case ItemType.FreeChest:
                newItem.spr = GetSpriteByName("chest_3");
                break;
            case ItemType.NormalChest:
                newItem.spr = GetSpriteByName("chest_2");
                break;
            case ItemType.AdvancedChest:
                newItem.spr = GetSpriteByName("chest_1");
                break;
            case ItemType.Card:
                break;
            case ItemType.Cash:
                break;
            case ItemType.PremiumSuit:
                break;
            case ItemType.GodenSuit:
                break;
            case ItemType.OfflineTime:
                break;
            case ItemType.IncreaseProfit:
                break;
            case ItemType.RemoveAds:
                break;
            case ItemType.TimeSkip_1H:
                newItem.spr = GetSpriteByName("timeTravel_1");
                break;
            case ItemType.TimeSkip_4H:
                newItem.spr = GetSpriteByName("timeTravel_2");
                break;
            case ItemType.TimeSkip_24H:
                newItem.spr = GetSpriteByName("timeTravel_3");
                break;
            case ItemType.ADTicket:
                if (item.amount <= 15) {
                    newItem.spr = GetSpriteByName("ticket_1");
                } else if (item.amount <= 30) {
                    newItem.spr = GetSpriteByName("ticket_2");
                } else if (item.amount <= 70) {
                    newItem.spr = GetSpriteByName("ticket_3");
                }
                break;
            case ItemType.NormalSkinBox:
                newItem.spr = GetSpriteByName("1");
                break;
            case ItemType.AdvancedSkinBox:
                newItem.spr = GetSpriteByName("2");
                break;
            case ItemType.FreeSkinBox:
                newItem.spr = GetSpriteByName("3");
                break;
            case ItemType.VIPMarketing:
                break;
            default:
                break;
        }

        return newItem;
    }
}

