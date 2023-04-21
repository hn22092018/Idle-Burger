using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class PanelReward : UIPanel {

    public static PanelReward instance;
    public bool rewardCardsInstanceLoad;
    [SerializeField] List<Sprite> cardFrames;
    [SerializeField] Sprite charCardFrame;
    [SerializeField] Image chestOpenImg;
    [SerializeField] Sprite[] chestOpensprs;
    [SerializeField] GameObject shiningEffect;
    [SerializeField] List<CardAmount> earnedList;
    
    float instantTimer = 0;
    bool finishShow = false;
    [SerializeField] Button btnClose;

    [SerializeField] GameObject rewardPanel;
    [SerializeField] GameObject listCardPanel;
    [SerializeField] Button btnContinue;
    [SerializeField] List<CardAmount> earnedListBackUp;
    [SerializeField] List<CardInBox> cardInBoxes;
    int halfAmount;
    int showedCount;

    [SerializeField] SkeletonGraphic valiAnimUI;
    [SerializeField] readonly string ANIM_OPEN_IDLE = "idle-open";
    [SerializeField] readonly string ANIM_OPEN = "open";
    [SerializeField] readonly string FREECHEST_SKIN = "vali1";
    [SerializeField] readonly string NORMALCHEST_SKIN = "vali2";
    [SerializeField] readonly string ADVANCEDCHEST_SKIN = "vali3";

    public void PlayChestFirstOpen()
    {
        if (valiAnimUI != null)
        {
            valiAnimUI.AnimationState.SetAnimation(0, ANIM_OPEN, loop: false);
        }
        StartCoroutine(WaitToIdle());
    }
    IEnumerator WaitToIdle()
    {
        yield return new WaitForSeconds(1f);
        PlayChestIdle();
    }

    void PlayChestIdle()
    {
        if (valiAnimUI != null)
        {
            valiAnimUI.AnimationState.SetAnimation(0, ANIM_OPEN_IDLE, loop: true);
        }
    }

    public void SetChestSkin(string chestSkin)
    {
        if(valiAnimUI != null)
        {
            valiAnimUI.Skeleton.SetSkin(chestSkin);
        }
    }

    public override void Awake() {
        panelType = UIPanelType.PanelReward;
        base.Awake();
        instance = this;
        btnClose.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            OnClose();
        });
        btnContinue.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            ContinueShowList();
        });
    }

    private void Update() {
        if (finishShow) {
            instantTimer += Time.deltaTime;
            if (instantTimer >= 5f) {
                OnClose();
            }
        }
    }

    public void SetOpenBoxType(ItemType type) {
        switch (type) {
            case ItemType.FreeChest:            
                SetChestSkin(FREECHEST_SKIN);
                break;
            case ItemType.NormalChest:        
                SetChestSkin(NORMALCHEST_SKIN);
                break;
            case ItemType.AdvancedChest:
                SetChestSkin(ADVANCEDCHEST_SKIN);
                break;
        }
    }
    public void ShowBoxReward(ItemType type) {
        SetOpenBoxType(type);
        rewardPanel.SetActive(true);
        listCardPanel.SetActive(false);
        finishShow = false;
        instantTimer = 0f;
        rewardCardsInstanceLoad = false;
        btnClose.gameObject.SetActive(false);
        earnedList = ProfileManager.Instance.playerData.boxManager.GetEarnedCardList();
        FillListBackUp();
        shiningEffect.SetActive(true);
        PlayChestFirstOpen();
        ShowCard();
    }

    public bool ContinueShowCard()
    {
        if (earnedList.Count > 0)
        {
            PlayChestIdle();
            ShowCard();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// This function called from popup box animation(in PanelReward animation)
    /// </summary>
    public void ShowCard()
    {
        UIManager.instance.GetPanel(UIPanelType.CardOpenGetAnimation).SetActive(true);
        UIManager.instance.GetPanel(UIPanelType.CardOpenGetAnimation).GetComponent<CardOpenGetAnimation>().StartPopupCard();
        switch (earnedList[0].card.cardType)
        {
            case CardType.NormalCard:
                UIManager.instance.GetPanel(UIPanelType.CardOpenGetAnimation).GetComponent<CardOpenGetAnimation>().SetUp(earnedList[0], cardFrames[(int)earnedList[0].card.cardRarity]);
                break;
            case CardType.CharacterCard:
                UIManager.instance.GetPanel(UIPanelType.CardOpenGetAnimation).GetComponent<CardOpenGetAnimation>().SetUp(earnedList[0], charCardFrame);
                break;
            default:
                break;
        }
        
        earnedList.Remove(earnedList[0]);
        if (earnedList.Count == 0)
        {
            shiningEffect.SetActive(false);
        }
    }
    public void OnClose() {
        UIManager.instance.ClosePanelReward();
    }

    public Sprite GetFrameByRarity(Rarity r, CardType t = CardType.NormalCard)
    {
        switch (t)
        {
            case CardType.NormalCard:
                return cardFrames[(int)r];
            case CardType.CharacterCard:
                return charCardFrame;
                break;
            default:
                return cardFrames[0];
                break;
        }
    }

    void FillListBackUp()
    {
        foreach(CardAmount c in earnedList)
        {
            earnedListBackUp.Add(c);
        }
    }

    public void ShowAllCard()
    {
        rewardPanel.SetActive(false);
        listCardPanel.SetActive(true);
        if(earnedListBackUp.Count < 24)
        {
            halfAmount = (int)(0.2f + earnedListBackUp.Count / 2);
            if (earnedListBackUp.Count <= 12)
            {
                halfAmount = 12;
            }
        }
        else
        {
            halfAmount = (int)(0.2f + earnedListBackUp.Count / 3);
        }
        ContinueShowList();
    }
    void ContinueShowList()
    {
        if (earnedListBackUp.Count > 0)
        {
            showedCount = 0;
            foreach (CardInBox c in cardInBoxes)
            {
                c.gameObject.SetActive(false);
            }
            StartCoroutine(ShowCardInBox());
        }
        else
        {
            OnClose();
        }  
    }
    IEnumerator ShowCardInBox()
    {
        yield return new WaitForSeconds(0.05f);
        if (earnedListBackUp.Count > 0 && showedCount <= halfAmount)
        {
            cardInBoxes[showedCount].gameObject.SetActive(true);
            cardInBoxes[showedCount].SetUp(earnedListBackUp[0]);
            earnedListBackUp.Remove(earnedListBackUp[0]);
            showedCount++;
            StartCoroutine(ShowCardInBox());
        }
    }

}
