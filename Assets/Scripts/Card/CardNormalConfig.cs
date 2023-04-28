using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Rarity {
    Common, Rare, Epic, Legendary
}
public enum CardIAPType {

    OFFLINE_TIME,
    FINANCIAL_MANAGER
}
public enum CardIapProductType {

    OFFLINE_TIME_2,
    OFFLINE_TIME_10,
    FINANCIAL_MANAGER_50,
    FINANCIAL_MANAGER_100
}
public enum ManagerStaffID {
    Chef,
    Deliver_1,
    MainRoom_1,
    Restroom_1
}
[System.Serializable]
public class CardNormalConfig {
    public string name;
    public int NameLocalizeID;
    public int ID;
    public Rarity cardRarity;
    public List<float> cardValues;
    public string description;
    public int DesLocalizeID;
    public Sprite sprOn, sprOff;
    public List<int> cardAmountLevel;
    public CardType cardType;
    public string RarityToString() {
        switch (cardRarity) {
            case Rarity.Common:
                return ProfileManager.Instance.dataConfig.GameText.GetTextByID(22);
            case Rarity.Rare:
                return ProfileManager.Instance.dataConfig.GameText.GetTextByID(23);
            case Rarity.Epic:
                return ProfileManager.Instance.dataConfig.GameText.GetTextByID(24);
            case Rarity.Legendary:
                return ProfileManager.Instance.dataConfig.GameText.GetTextByID(25);
        }
        return "";
    }
    public string GetName() {
        if (!string.IsNullOrEmpty(ProfileManager.Instance.dataConfig.GameText.GetTextByID(NameLocalizeID))) {
            return ProfileManager.Instance.dataConfig.GameText.GetTextByID(NameLocalizeID);
        }
        return name;
    }
    public string GetDes() {
        if (!string.IsNullOrEmpty(ProfileManager.Instance.dataConfig.GameText.GetTextByID(DesLocalizeID))) {
            return ProfileManager.Instance.dataConfig.GameText.GetTextByID(DesLocalizeID);
        }
        return description;
    }

}

[System.Serializable]
public class CardIAPConfig {
    public string name;
    public int NameLocalizeID;
    public int id;
    public Sprite icon;
    public CardIAPType type;
    public CardIapProductType productType;
    public string description;
    public int DesLocalizeID;
    public int extraValue;
    public string price;
    public string productID;
    public string GetDes() {
        if (!string.IsNullOrEmpty(ProfileManager.Instance.dataConfig.GameText.GetTextByID(DesLocalizeID))) {
            return ProfileManager.Instance.dataConfig.GameText.GetTextByID(DesLocalizeID);
        }
        return description;
    }
    public string GetName() {
        if (NameLocalizeID>0&& !string.IsNullOrEmpty(ProfileManager.Instance.dataConfig.GameText.GetTextByID(NameLocalizeID))) {
            return ProfileManager.Instance.dataConfig.GameText.GetTextByID(NameLocalizeID);
        }
        return name;
    }
}
[System.Serializable]
public class CardManagerConfig {
    public string name;
    public ManagerStaffID staffType;
    public Rarity rarity;
    [PreviewField(70, ObjectFieldAlignment.Left)]
    public Mesh mesh;
    [PreviewField(70, ObjectFieldAlignment.Left)]
    public Sprite sprIcon;
    public Sprite sprCardPieceIcon;
}

