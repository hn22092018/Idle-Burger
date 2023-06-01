using DG.Tweening;
using SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabUpgrade : MonoBehaviour {
    public Transform rootUIItem;
    public Transform prefabUIItem;
    public Text txtItemName;
    public Image imgItemIcon;
    public Text txtItemLevel;
    public Text txtItemUpgradePrice;
    public Text txtUpgrade;
    public Button btnUpgrade;
    public GameObject btnMaxUpgrade;
    public GameObject grMoney;
    public Button btnEvolve;
    public Text txtEvolvePrice;
    public Transform transProcessUpgrade;
    public Transform objGemEff1, objGemEff2;
    public GameObject objCompleteReward1, objCompleteReward2;
    [SerializeField] GameObject uiTabManagerInfo;
    List<UIUpgradeItem> listUpgradeItems = new List<UIUpgradeItem>();
    int selectedItemID;
    BigNumber priceUpgrade = new BigNumber(0);
    int evolvePrice;
    [SerializeField] RectTransform rectTextPrice;
    float timeRebuildLayout = 0;
    IRoomController currentRoom;
    float dtTimeUpgradeHover;
    private void Awake() {
        btnEvolve.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.UPGRADE);
            OnEvolVeItem();
        });
    }
    private void OnEnable() {
        IsUpgradeButtonSelected = false;
    }
    /// <summary>
    /// update status upgrade button & UIUpgradeItems when game money change
    /// </summary>
    private void Update() {
        if (timeRebuildLayout <= 1) {
            timeRebuildLayout += Time.deltaTime;
            LayoutRebuilder.MarkLayoutForRebuild(rectTextPrice);
        }
        btnUpgrade.interactable = GameManager.instance.IsEnoughCash(priceUpgrade);
        if (IsUpgradeButtonSelected && btnUpgrade.gameObject.activeInHierarchy) {
            dtTimeUpgradeHover -= Time.deltaTime;
            if (dtTimeUpgradeHover <= 0) {
                dtTimeUpgradeHover = 0.15f;
                SoundManager.instance.PlaySoundEffect(SoundID.UPGRADE);
                OnUpgradeItem();
            }
        }
    }
    /// <summary>
    /// Load UI by room control
    /// </summary>
    /// <param name="roomManager"></param>
    public void Setup() {
        rootUIItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        priceUpgrade = 0;
        currentRoom = GameManager.instance.selectedRoom;
        uiTabManagerInfo.gameObject.SetActive(currentRoom.GetManagerStaffID() != ManagerStaffID.None);
        int totalModel = currentRoom.GetTotalModel();
        while (rootUIItem.childCount < totalModel) {
            Instantiate(prefabUIItem, rootUIItem);
        }
        listUpgradeItems.Clear();
        for (int i = 0; i < rootUIItem.childCount; i++) {
            Transform t = rootUIItem.GetChild(i);
            if (i < totalModel) {
                t.gameObject.SetActive(true);
                UIUpgradeItem ui = t.GetComponent<UIUpgradeItem>();
                ui.Setup(i, currentRoom.GetSpriteItem(i), currentRoom.GetLevelItem(i), currentRoom.GetLevelMaxItem(i));
                listUpgradeItems.Add(ui);
            } else {
                t.gameObject.SetActive(false);
            }
        }
        selectedItemID = 0;
        OnShowInfoItem(selectedItemID);
        CheckReward();
    }
    void CheckReward() {
        objCompleteReward1.SetActive(false);
        objCompleteReward2.SetActive(false);
        if (currentRoom.GetProcessUpgrade() >= 0.5f && !ProfileManager.PlayerData._RewardRoomManager.IsCollectReward(currentRoom.GetRoomID(), RewardRoomType.Stage_1)) {
            transProcessUpgrade.localScale = new Vector3(0, 1, 1);
            transProcessUpgrade.DOScaleX(0.5f, 0.5f).SetDelay(0.7f).OnComplete(() => {
                OnClaimReward1();
                objCompleteReward1.SetActive(true);
                transProcessUpgrade.DOScaleX(currentRoom.GetProcessUpgrade(), 0.5f).SetDelay(0.5f).OnComplete(() => {
                    if (currentRoom.GetProcessUpgrade() >= 1f) {
                        if (!ProfileManager.PlayerData._RewardRoomManager.IsCollectReward(currentRoom.GetRoomID(), RewardRoomType.Stage_2)) {
                            OnClaimReward2();
                        }
                        objCompleteReward2.SetActive(true);
                    }
                });
            });

        } else {
            if (currentRoom.GetProcessUpgrade() >= 0.5f && ProfileManager.PlayerData._RewardRoomManager.IsCollectReward(currentRoom.GetRoomID(), RewardRoomType.Stage_1)) objCompleteReward1.SetActive(true);
            if (currentRoom.GetProcessUpgrade() >= 1f && ProfileManager.PlayerData._RewardRoomManager.IsCollectReward(currentRoom.GetRoomID(), RewardRoomType.Stage_2)) objCompleteReward2.SetActive(true);
        }
    }
    /// <summary>
    /// Show All Info Model When user click room in game or change other model in UI.
    /// </summary>
    /// <param name="index"></param>

    public void OnShowInfoItem(int index) {
        transProcessUpgrade.transform.localScale = new Vector3(currentRoom.GetProcessUpgrade(), 1, 1);
        listUpgradeItems[selectedItemID].OnSelect(false);
        selectedItemID = index;
        listUpgradeItems[selectedItemID].OnSelect(true);
        txtItemName.text = currentRoom.GetNameItem(index);
        imgItemIcon.sprite = currentRoom.GetSpriteItem(index);
        //txtItemInfo.text = currentRoom.GetInfoItem(index);
        int level = currentRoom.GetLevelItem(index);
        int maxLevel = currentRoom.GetLevelMaxItem(index);
        btnUpgrade.gameObject.SetActive(level != maxLevel);
        btnEvolve.gameObject.SetActive(false);
        btnMaxUpgrade.gameObject.SetActive(level == maxLevel);
        if (level > 0 && level % 25 == 0 && level < maxLevel) {
            IsUpgradeButtonSelected = false;
            btnUpgrade.gameObject.SetActive(false);
            btnEvolve.gameObject.SetActive(true);
            evolvePrice = GameManager.instance.GetEvolvePriceByLevel(level);
            txtEvolvePrice.text = evolvePrice + "";
            btnEvolve.interactable = GameManager.instance.IsEnoughBurgetCoin(evolvePrice);
        }
        if (level == maxLevel) IsUpgradeButtonSelected = false;
        string strLevel = ProfileManager.Instance.dataConfig.GameText.GetTextByID(86).ToUpper() + ": ";
        txtItemLevel.text = strLevel + level.ToString();
        priceUpgrade = currentRoom.GetUpgradePriceItem(index);
        txtItemUpgradePrice.text = new BigNumber(priceUpgrade).ToString();
        string strShow = ProfileManager.Instance.dataConfig.GameText.GetTextByID(135).ToUpper();
        string strUpgrade = ProfileManager.Instance.dataConfig.GameText.GetTextByID(88).ToUpper();
        txtUpgrade.text = level == 0 ? strShow : strUpgrade;
        timeRebuildLayout = 0;
        CameraMove.instance.ChangePosition(-new Vector3(0, 6.5f, 0f) + GameManager.instance.selectedRoom.GetPositionItem(selectedItemID), null);
    }
    /// <summary>
    ///  Callback when user press upgrade button
    /// </summary>
    private void OnUpgradeItem() {
        /// check enough money true => upgrade model
        if (GameManager.instance.IsEnoughCash(priceUpgrade)) {
            btnUpgrade.transform.localScale = Vector3.one;
            btnUpgrade.transform.DOScale(new Vector3(1.15f, 1.15f, 1), 0.2f).SetEase(Ease.Linear).OnComplete(() => {
                btnUpgrade.transform.DOScale(new Vector3(1f, 1, 1), 0.2f);
            });
            BigNumber priceUpgradeCache = priceUpgrade;
            /// call room control action Upgrade
            currentRoom.OnUpgradeItem(selectedItemID);
            /// reload info ui
            UIUpgradeItem ui = listUpgradeItems[selectedItemID];
            ui.SetupNextLevel(currentRoom.GetLevelItem(selectedItemID));
            OnShowInfoItem(selectedItemID);
            PanelRoomInfo.instance.SetupTabProfit();
            PanelRoomInfo.instance.OnPressUpgradeButton();
            ProfileManager.PlayerData.ConsumeCash(priceUpgradeCache);
            if (currentRoom.GetProcessUpgrade() >= 1f && !ProfileManager.PlayerData._RewardRoomManager.IsCollectReward(currentRoom.GetRoomID(), RewardRoomType.Stage_2)) {
                OnClaimReward2();
            } else if (currentRoom.GetProcessUpgrade() >= 0.5f && !ProfileManager.PlayerData._RewardRoomManager.IsCollectReward(currentRoom.GetRoomID(), RewardRoomType.Stage_1)) {
                OnClaimReward1();
            }
        }

    }
    void OnEvolVeItem() {
        if (GameManager.instance.IsEnoughBurgetCoin(evolvePrice)) {
            IsUpgradeButtonSelected = false;
            ProfileManager.PlayerData.ConsumeBCoin(evolvePrice);
            currentRoom.OnUpgradeItem(selectedItemID);
            /// reload info ui
            UIUpgradeItem ui = listUpgradeItems[selectedItemID];
            ui.SetupNextLevel(currentRoom.GetLevelItem(selectedItemID));
            OnShowInfoItem(selectedItemID);
            PanelRoomInfo.instance.SetupTabProfit();
            if (currentRoom.GetProcessUpgrade() >= 1f && !ProfileManager.PlayerData._RewardRoomManager.IsCollectReward(currentRoom.GetRoomID(), RewardRoomType.Stage_2)) {
                OnClaimReward2();
            } else if (currentRoom.GetProcessUpgrade() >= 0.5f && !ProfileManager.PlayerData._RewardRoomManager.IsCollectReward(currentRoom.GetRoomID(), RewardRoomType.Stage_1)) {
                OnClaimReward1();
            }
        }
    }
    void OnClaimReward1() {
        for (int i = 0; i < 10; i++) {
            Transform eff = Instantiate(objGemEff1.transform, transform);
            eff.transform.position = objGemEff1.transform.position;
            eff.DOMove(eff.position + new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), 0), 0.5f).SetDelay(i * 0.01f).OnComplete(() => {
                eff.transform.DOMove(UIManager.instance._UIPanelResourceGem.txtGem.transform.position, 0.5f).OnComplete(() => {
                    Destroy(eff.gameObject);
                });
            });
        }
        objCompleteReward1.SetActive(true);
        ProfileManager.PlayerData._RewardRoomManager.CollectReward(currentRoom.GetRoomID(), RewardRoomType.Stage_1);
        ProfileManager.PlayerData.AddGem(5);
        ABIAnalyticsManager.Instance.TrackEventGem(GemAction.Earn_RoomProcces, 5);
    }
    void OnClaimReward2() {
        for (int i = 0; i < 10; i++) {
            Transform eff = Instantiate(objGemEff2.transform, transform);
            eff.transform.position = objGemEff2.transform.position;
            eff.DOMove(eff.position + new Vector3(UnityEngine.Random.Range(-120, 120), UnityEngine.Random.Range(-120, 120), 0), 0.5f).SetDelay(i * 0.01f).OnComplete(() => {
                eff.transform.DOMove(UIManager.instance._UIPanelResourceGem.txtGem.transform.position, 1f).OnComplete(() => {
                    Destroy(eff.gameObject);
                });
            });
        }
        objCompleteReward2.SetActive(true);
        ProfileManager.PlayerData._RewardRoomManager.CollectReward(currentRoom.GetRoomID(), RewardRoomType.Stage_2);
        ProfileManager.PlayerData.AddGem(10);
        ABIAnalyticsManager.Instance.TrackEventGem(GemAction.Earn_RoomProcces, 10);
    }
    bool IsUpgradeButtonSelected;
    public void OnButtonUpgradePointerClick() {
        IsUpgradeButtonSelected = true;
        dtTimeUpgradeHover = 0;
    }
    public void OnButtonUpgradePointerExit() {
        IsUpgradeButtonSelected = false;
    }
}
