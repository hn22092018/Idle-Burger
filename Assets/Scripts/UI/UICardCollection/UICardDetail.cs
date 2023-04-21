using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardDetail : MonoBehaviour {
    public static UICardDetail instance;
    [SerializeField] Image cardFrame;
    [SerializeField] Button closeButton;
    [SerializeField] Text cardName, cardRarity, cardDescription, cardBuff, cardLevel;
    [SerializeField] Image cardIcon;
    [SerializeField] Text ownedCardText;
    [SerializeField] Sprite frameLock;
    [SerializeField] GameObject upgradeDecore;
    int level, currentAmount;
    private void Awake() {
        instance = this;
        closeButton.onClick.AddListener(OnClose);
    }
    void OnClose() {
        gameObject.SetActive(false);
    }
    public void LoadCardDetail(CardInfo cardInfo) {
        switch (cardInfo.cardType)
        {
            case CardType.NormalCard:
                level = ProfileManager.PlayerData.GetCardManager().GetCardLevelByID(cardInfo.ID);
                currentAmount = ProfileManager.PlayerData.GetCardManager().GetCardAmountByID(cardInfo.ID);
                break;
            default:
                break;
        }
        string colorHexName = "";
        string colorHexRarity = "";
        switch (cardInfo.cardRarity) {
            case Rarity.Common:
                SoundManager.instance.PlaySoundEffect(SoundID.CARD_COMMON);
                colorHexRarity = "#00C95F";
                colorHexName = "#00C95F";
                break;
            case Rarity.Rare:
                SoundManager.instance.PlaySoundEffect(SoundID.CARD_RARE);
                colorHexRarity = "#09A1FF";
                colorHexName = "#09A1FF";
                break;
            case Rarity.Epic:
                SoundManager.instance.PlaySoundEffect(SoundID.CARD_EPIC);
                colorHexRarity = "#A648FD";
                colorHexName = "#A648FD";
                break;
            case Rarity.Legendary:
                SoundManager.instance.PlaySoundEffect(SoundID.CARD_LEGEND);
                colorHexRarity = "#F4B200";
                colorHexName = "#F4B200";
                break;
            default:
                break;
        }
        switch (cardInfo.cardType)
        {
            case CardType.NormalCard:
                cardRarity.text = "<color=" + colorHexName + ">" + cardInfo.RarityToString() + "</color>";
                cardName.text = "<color=" + colorHexRarity + ">" + cardInfo.GetName() + "</color>";
                cardDescription.text = cardInfo.GetDes();
                if (level > 0)
                {
                    cardBuff.text = "+" + cardInfo.cardValues[level - 1].ToString() + "%";
                    cardIcon.sprite = cardInfo.sprOn;
                    cardFrame.sprite = PanelCard.instance.GetCardFrameByRarity(cardInfo.cardRarity);
                }
                else
                {
                    string strNextLevel = ProfileManager.Instance.dataConfig.GameText.GetTextByID(234).ToUpper() + ": +";
                    cardBuff.text = strNextLevel + cardInfo.cardValues[level].ToString() + " % ";
                    cardIcon.sprite = cardInfo.sprOff;
                    cardFrame.sprite = frameLock;
                }
                break;
            default:
                break;
        }
        
        string strLevel = ProfileManager.Instance.dataConfig.GameText.GetTextByID(27).ToUpper() + " ";
        cardLevel.text = strLevel + level.ToString();
        
        ownedCardText.text = currentAmount.ToString();
        CloseLevelUpTitle();
    }
    public void ShowLevelUpTitle() {
        upgradeDecore.SetActive(true);
    }
    public void CloseLevelUpTitle() {
        upgradeDecore.SetActive(false);
    }
}
