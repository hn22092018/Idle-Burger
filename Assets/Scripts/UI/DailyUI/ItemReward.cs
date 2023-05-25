using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType {
    Gem,
    FreeChest,
    NormalChest,
    AdvancedChest,
    Card,
    Cash,
    PremiumSuit,
    GodenSuit,
    OfflineTime,
    IncreaseProfit,
    RemoveAds,
    TimeSkip_1H,
    TimeSkip_4H,
    TimeSkip_24H,
    ADTicket,
    NormalSkinBox,
    AdvancedSkinBox,
    ExpertSkinBox,
    VIPMarketing,
    Uniform,
    ResearchValue,
    Researcher,
    Reputation,
    BurgerCoin,
    Cheese,
    Pepper,
    Sugar,
    Carot,
    Flour
}
[System.Serializable]
public class ItemReward {
    public ItemType type;
    public Sprite spr;
    public int amount;
    [HideInInspector] public bool skinReward;
    [ConditionalHide("skinReward", true)] public int skinID;
    public void OnEnable()
    {
        skinReward = type == ItemType.Uniform;
    }
    public string AmountToString() {
        return type switch {
            ItemType.Gem => "+" + amount + "",
            ItemType.FreeChest => "+" + amount + "",
            ItemType.NormalChest => "+" + amount + "",
            ItemType.AdvancedChest => "+" + amount + "",
            ItemType.Card => "+" + amount + "",
            ItemType.Cash => "+" + amount + "",
            ItemType.PremiumSuit => "",
            ItemType.GodenSuit => "",
            ItemType.OfflineTime => "+" + amount + "H",
            ItemType.IncreaseProfit => "+" + amount + "%",
            ItemType.RemoveAds => "",
            ItemType.TimeSkip_1H => "+" + amount + "",
            ItemType.TimeSkip_4H => "+" + amount + "",
            ItemType.TimeSkip_24H => "+" + amount + "",
            ItemType.ADTicket => "+" + amount + "",
            ItemType.NormalSkinBox => "+" + amount + "",
            ItemType.AdvancedSkinBox => "+" + amount + "",
            ItemType.ExpertSkinBox => "+" + amount + "",
            ItemType.VIPMarketing => "",
            ItemType.Uniform => "",
            ItemType.ResearchValue => "+" + amount + "",
            ItemType.Reputation => "+" + amount + "",
            ItemType.Researcher => "+" + amount + "",
            ItemType.Cheese => "+" + amount + "",
            ItemType.Pepper => "+" + amount + "",
            ItemType.Sugar => "+" + amount + "",
            ItemType.Carot => "+" + amount + "",
            ItemType.Flour => "+" + amount + "",
            ItemType.BurgerCoin => "+" + amount + "",
            _ => throw new System.NotImplementedException(),
        };
    }
    public string TypeToString() {
        return type switch {
            ItemType.Gem => ProfileManager.Instance.dataConfig.GameText.GetTextByID(160).ToUpper(),
            ItemType.FreeChest => ProfileManager.Instance.dataConfig.GameText.GetTextByID(166).ToUpper(),
            ItemType.NormalChest => ProfileManager.Instance.dataConfig.GameText.GetTextByID(165).ToUpper(),
            ItemType.AdvancedChest => ProfileManager.Instance.dataConfig.GameText.GetTextByID(164).ToUpper(),
            ItemType.Card => "Card".ToUpper(),
            ItemType.Cash => "Cash".ToUpper(),
            ItemType.PremiumSuit => ProfileManager.Instance.dataConfig.GameText.GetTextByID(159).ToUpper(),
            ItemType.GodenSuit => ProfileManager.Instance.dataConfig.GameText.GetTextByID(158).ToUpper(),
            ItemType.OfflineTime => ProfileManager.Instance.dataConfig.GameText.GetTextByID(161).ToUpper(),
            ItemType.IncreaseProfit => ProfileManager.Instance.dataConfig.GameText.GetTextByID(32).ToUpper(),
            ItemType.RemoveAds => ProfileManager.Instance.dataConfig.GameText.GetTextByID(174).ToUpper(),
            ItemType.TimeSkip_1H => ProfileManager.Instance.dataConfig.GameText.GetTextByID(177).ToUpper() + " 1H",
            ItemType.TimeSkip_4H => ProfileManager.Instance.dataConfig.GameText.GetTextByID(177).ToUpper() + " 4H",
            ItemType.TimeSkip_24H => ProfileManager.Instance.dataConfig.GameText.GetTextByID(177).ToUpper() + " 24H",
            ItemType.ADTicket => ProfileManager.Instance.dataConfig.GameText.GetTextByID(175).ToUpper(),
            ItemType.NormalSkinBox => ProfileManager.Instance.dataConfig.GameText.GetTextByID(165).ToUpper(),
            ItemType.AdvancedSkinBox => ProfileManager.Instance.dataConfig.GameText.GetTextByID(164).ToUpper(),
            ItemType.ExpertSkinBox => ProfileManager.Instance.dataConfig.GameText.GetTextByID(413).ToUpper(),
            ItemType.VIPMarketing => ProfileManager.Instance.dataConfig.GameText.GetTextByID(162).ToUpper(),
            ItemType.Uniform => ProfileManager.Instance.dataConfig.GameText.GetTextByID(420).ToUpper(),
            ItemType.ResearchValue => "Research".ToUpper(),
            ItemType.Researcher => ProfileManager.Instance.dataConfig.GameText.GetTextByID(454).ToUpper(),
            ItemType.Reputation => ProfileManager.Instance.dataConfig.GameText.GetTextByID(453).ToUpper(),
            ItemType.BurgerCoin => "Burger Coin".ToUpper(),
            _ => "",
           
        };
    }
}