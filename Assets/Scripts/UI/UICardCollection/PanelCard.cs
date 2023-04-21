using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCard : UIPanel {
    public static PanelCard instance;
    public UICardDetail cardDetailPanel;
    [SerializeField] GameObject CardPrefab;
    [SerializeField] Button closeButton, goToShopButton;
    [SerializeField] Transform cardsCommon, cardsRare, cardsEpic, cardsLegendery;
    [SerializeField] Sprite commonFrame, rareFrame, epicFrame, legendaryFrame;
    public RectTransform contentNeedRefresh;
    [SerializeField] List<UICard> uICards;
    [SerializeField] GameObject cardNotify;
    public override void Awake() {
        panelType = UIPanelType.PanelCard;
        base.Awake();
        instance = this;
        closeButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            OnClose();
        });
        goToShopButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            GoToShopButton();
        });
    }
    private void OnEnable() {
        cardDetailPanel.gameObject.SetActive(false);
    }
    private void Start() {
        LoadCards();
    }
    private void Update() {
        CheckFreeChestNotify();
    }
    void CheckFreeChestNotify() {
      bool  isHasFreeChest = ProfileManager.PlayerData.boxManager.IsHasFreeChest();
        cardNotify.SetActive(isHasFreeChest);
    }
    public void LoadCards() {
        List<CardInfo> allCards = ProfileManager.Instance.dataConfig.cardData.cardList;
        for (int i = 0; i < allCards.Count; i++) {
            GameObject card = null;
            switch (allCards[i].cardRarity) {
                case Rarity.Common:
                    card = Instantiate(CardPrefab, cardsCommon);
                    break;
                case Rarity.Rare:
                    card = Instantiate(CardPrefab, cardsRare);
                    break;
                case Rarity.Epic:
                    card = Instantiate(CardPrefab, cardsEpic);
                    break;
                case Rarity.Legendary:
                    card = Instantiate(CardPrefab, cardsLegendery);
                    break;
                default:
                    break;
            }
            UICard uICard = card.GetComponent<UICard>();
            uICards.Add(uICard);
            uICard.LoadCard(allCards[i], GetCardFrameByRarity(allCards[i].cardRarity));
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentNeedRefresh);
        }
    }

    public Sprite GetCardFrameByRarity(Rarity r = Rarity.Common)
    {
        switch (r)
        {
            case Rarity.Common:
                return commonFrame;
                break;
            case Rarity.Rare:
                return rareFrame;
                break;
            case Rarity.Epic:
                return epicFrame;
                break;
            case Rarity.Legendary:
                return legendaryFrame;
                break;
            default:
                return commonFrame;
                break;
        }
    }

    public void OpenCardDetail() {
        cardDetailPanel.gameObject.SetActive(true);
    }
    public void OnOpen() {
        if (uICards.Count > 0)
            ReloadAllCard();
    }
    public void OnClose() {
        UIManager.instance.ClosePanelCard();
    }
    void GoToShopButton() {
        OnClose();
        UIManager.instance.GotoShopPage(TabName.Chest);
    }
    public void ReloadCard(UICard uiCard) {
        switch (uiCard.cardInfo.cardType)
        {
            case CardType.NormalCard:
                uiCard.LoadCard(uiCard.cardInfo, GetCardFrameByRarity(uiCard.cardInfo.cardRarity));
                break;
            default:
                break;
        }
        

    }
    public void ReloadAllCard() {
        foreach (UICard ui in uICards)
        {
            ui.LoadCard(ui.cardInfo, GetCardFrameByRarity(ui.cardInfo.cardRarity));
        }
    }

}
