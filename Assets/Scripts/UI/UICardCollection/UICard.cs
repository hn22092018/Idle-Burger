using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour {
    public CardInfo cardInfo;
    [SerializeField] Sprite frameSprite;
    [SerializeField] Image cardIcon;
    [SerializeField] Image cardFrame;
    [SerializeField] Sprite frameLock;
    [SerializeField] Text cardName;
    [SerializeField] Text cardLevel;
    [SerializeField] Text cardCount;
    [SerializeField] Button btnDetail;
    [SerializeField] Button btnUpgrade;
    int level, currentAmount;
    CardType cardType;
    private void Awake() {
        btnDetail.onClick.AddListener(CardPress);
        btnUpgrade.onClick.AddListener(UpgradeCard);
    }
    public void LoadCard(CardInfo cardIn, Sprite frame) {
        cardInfo = cardIn;
        cardType = cardInfo.cardType;
        switch (cardType)
        {
            case CardType.NormalCard:
                level = ProfileManager.PlayerData.GetCardManager().GetCardLevelByID(cardInfo.ID);
                currentAmount = ProfileManager.PlayerData.GetCardManager().GetCardAmountByID(cardInfo.ID);
                cardName.text = cardInfo.GetName();
                break;
            default:
                break;
        }
        
        frameSprite = frame;
        if (level == 0) {
            cardFrame.sprite = frameLock;
            cardIcon.sprite = cardInfo.sprOff;
        } else {
            cardFrame.sprite = frameSprite;
            cardIcon.sprite = cardInfo.sprOn;
        }
        
        cardLevel.text = "Lv."+level.ToString();
        if (level < 5) {
            if (currentAmount >= cardInfo.cardAmountLevel[level]) {
                btnUpgrade.gameObject.SetActive(true);
                cardCount.text = currentAmount + "/" + cardInfo.cardAmountLevel[level];
            } else {
                cardCount.text = currentAmount + "/" + cardInfo.cardAmountLevel[level];
                cardCount.transform.parent.gameObject.SetActive(true);
                btnUpgrade.gameObject.SetActive(false);
            }
        } else {
            cardCount.transform.parent.gameObject.SetActive(true);
            btnUpgrade.gameObject.SetActive(false);
        }
    }
    void CardPress() {
        if (cardInfo != null) {
            PanelCard.instance.OpenCardDetail();
            UICardDetail cardDetail = UICardDetail.instance;
            cardDetail.LoadCardDetail(cardInfo);
        }
    }
    void UpgradeCard() {
        SoundManager.instance.PlaySoundEffect(SoundID.UPGRADE);
        switch (cardType)
        {
            case CardType.NormalCard:
                ProfileManager.PlayerData.GetCardManager().UpgradeCard(cardInfo.ID);
                break;
            default:
                break;
        }
        PanelCard.instance.OpenCardDetail();
        UICardDetail cardDetail = UICardDetail.instance;
        cardDetail.LoadCardDetail(cardInfo);
        PanelCard.instance.ReloadCard(this);
        cardDetail.ShowLevelUpTitle();
        btnUpgrade.gameObject.SetActive(false);
    }
    
    private void Update() {
        switch (cardType)
        {
            case CardType.NormalCard:
                if (ProfileManager.Instance.playerData.cardManager.GetCardLevelByID(cardInfo.ID) < cardInfo.cardAmountLevel.Count)
                {
                    btnUpgrade.gameObject.SetActive(currentAmount >= cardInfo.cardAmountLevel[level]);
                }
                break;
            default:
                break;
        }
    }

    public CardType GetCardType()
    {
        return cardType;
    }
}
