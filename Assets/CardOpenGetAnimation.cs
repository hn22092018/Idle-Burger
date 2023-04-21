using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardOpenGetAnimation : UIPanel
{
    [Header("Animation")]
    [SerializeField] Animator animator;

    [Header("Card Info")]
    CardAmount card;
    Sprite sprFrame;
    [SerializeField] Image frameImg;
    [SerializeField] Image iconImg;
    [SerializeField] Text cardTitleTxt;
    [SerializeField] Text cardRarityTxt;
    [SerializeField] Text cardOpenedTxt;
    [SerializeField] Text cardLevelTxt;
    [SerializeField] Text cardUpgaredTxt;
    [SerializeField] Text cardDescTxt;
    [SerializeField] Text cardAmountTxt;
    [SerializeField] GameObject upgradeAlert;
    [SerializeField] Slider amountSlider;
    CardSaveInfor cardSaveInfo;
    CardInfo openingCard;
    int cardAmount;
    bool sliderRun;
    float sliderValue;

    [SerializeField] GameObject continueBtn;
    [SerializeField] float autoOpenCoolDown;
    float autoOpenCounter;

    [SerializeField] Button skipBtn;
    bool skipAnim;

    public override void Awake()
    {
        panelType = UIPanelType.CardOpenGetAnimation;
        base.Awake();
        continueBtn.GetComponent<Button>().onClick.AddListener(ContinueShowCard);
        skipBtn.onClick.AddListener(SkipAnimation);
    }
    public void SetUp(CardAmount inCard, Sprite inSprFrame)
    {
        card = inCard;
        sprFrame = inSprFrame;
        autoOpenCounter = 100f;
    }
    public void ShowInfoFlag()
    {
        amountSlider.value = 0;
        sliderRun = false;
        openingCard = card.card;
        frameImg.sprite = sprFrame;
        iconImg.sprite = card.card.sprOn;
        cardTitleTxt.text = card.card.GetName();
        cardRarityTxt.text = card.card.RarityToString();
        switch (card.card.cardRarity)
        {
            case Rarity.Common:
                cardRarityTxt.color = new Color(219f / 255f, 248f / 255f, 255f / 255f, 1f);
                break;
            case Rarity.Rare:
                cardRarityTxt.color = new Color(0f / 255f, 166f / 255f, 235f / 255f, 1f);
                break;
            case Rarity.Epic:
                cardRarityTxt.color = new Color(150f / 255f, 38f / 255f, 236f / 255f, 1f);//
                break;
            case Rarity.Legendary:
                cardRarityTxt.color = new Color(254f / 255f, 168f / 255f, 46f / 255f, 1f);
                break;
            default:
                break;
        }
        cardTitleTxt.color = cardRarityTxt.color;
        cardOpenedTxt.text = card.amount.ToString() +" "+ ProfileManager.Instance.dataConfig.GameText.GetTextByID(26);
        cardLevelTxt.text = "";
        cardUpgaredTxt.text = "";
        cardDescTxt.text = card.card.GetDes();
        // Get saved info
        FindCardInCardSave(card.card.ID);
        autoOpenCounter = autoOpenCoolDown;
        skipAnim = false;
    }
    public void StartPopupCard()
    {
        continueBtn.SetActive(false);
        ShowEntireCard();
    }

    void ShowEntireCard()
    {
        animator.Play(0);
    }

    void FindCardInCardSave(int id)
    {
        cardSaveInfo = ProfileManager.PlayerData.GetCardManager().GetOwnerCardByID(id);
        cardLevelTxt.text = ProfileManager.Instance.dataConfig.GameText.GetTextByID(27)+" "+ cardSaveInfo.level.ToString();
        cardAmount = cardSaveInfo.currentAmount;
        CheckUpgradeable();
        
    }
    void CheckUpgradeable()
    {
        if(cardSaveInfo.level == openingCard.cardAmountLevel.Count)
        {
            cardUpgaredTxt.text = ProfileManager.Instance.dataConfig.GameText.GetTextByID(28);
            cardAmountTxt.text = "";
            sliderRun = false;
            amountSlider.value = 1;
            return;
        }
        if (cardAmount >= openingCard.cardAmountLevel[cardSaveInfo.level])
        {
            cardUpgaredTxt.text = ProfileManager.Instance.dataConfig.GameText.GetTextByID(29);
            cardAmountTxt.text = cardAmount.ToString() + "/" + openingCard.cardAmountLevel[cardSaveInfo.level].ToString();
            sliderRun = false;
            amountSlider.value = 1;
            upgradeAlert.SetActive(true);
        }
        else
        {
            cardUpgaredTxt.text = "";
            cardAmountTxt.text = cardAmount.ToString() + "/" + openingCard.cardAmountLevel[cardSaveInfo.level].ToString();
            sliderValue = (float)((float)cardAmount / (float)openingCard.cardAmountLevel[cardSaveInfo.level]);
            upgradeAlert.SetActive(false);
        }
    }

    /// <summary>
    /// This function called from show card animation(AcrdOpenAnimPanel)
    /// </summary>
    public void CardShowed()
    {
        sliderRun = true;
        continueBtn.SetActive(true);
    }

    public void ContinueShowCard()
    {
        continueBtn.SetActive(false);
        autoOpenCounter = 100f;
        if (!UIManager.instance.GetPanel(UIPanelType.PanelReward).GetComponent<PanelReward>().ContinueShowCard())
        {
            SkipAnimation();
        }
    }
    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(sliderRun)
        {
            if(amountSlider.value < sliderValue)
            {
                amountSlider.value += 5f * Time.deltaTime;
            }
        }
        autoOpenCounter -= Time.deltaTime;
        if(autoOpenCounter <= 0f)
        {
            ContinueShowCard();
        }
    }

    void SkipAnimation()
    {
        skipAnim = true;
        UIManager.instance.GetPanel(UIPanelType.PanelReward).GetComponent<PanelReward>().ShowAllCard();
        OnClose();
    }

}
