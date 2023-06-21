using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum OfferID {
    NoAds,
    OfferForBegginer,
    OfferForPros,
    FreeChest,
    NormalChest,
    AdvancedChest,
    Gem1,
    Gem2,
    Gem3,
    Gem4,
    Gem5,
    ADTicket1,
    ADTicket2,
    ADTicket3,
    TimeTrave_30M,
    TimeTrave_60M,
    TimeTrave_2H,
    NormalSkinBox,
    AdvancedSkinBox,
    ExpertSkinBox,
    GemAds
}

[System.Serializable]
public class OfferData {
    public Sprite icon;
    public string titleDeal;
    public int titleDealLocalizeID;
    public OfferID offerID;
    public List<ItemReward> itemRewards;
    public float price;
    public string productID;
    public bool isSaleOff;
    public double timeExits;
    public string GetTitleDeal() {
        if (ProfileManager.Instance.dataConfig.GameText.GetTextByID(titleDealLocalizeID) != "") {
            return ProfileManager.Instance.dataConfig.GameText.GetTextByID(titleDealLocalizeID);
        }
        return titleDeal;
    }

}


