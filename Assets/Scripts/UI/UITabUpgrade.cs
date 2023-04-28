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
    public Text txtItemInfo;
    public Text txtItemLevel;
    public Text txtItemUpgradePrice;
    public Text txtUpgrade;
    public Button btnUpgrade;
    public GameObject btnMaxUpgrade;
    public GameObject grMoney;
    //public Text txtItemMoneyEarn;
    //public Text txtTimeReduce;
    //public GameObject grTimeReduce;
    //public Text txtItemEnergy;
    //public GameObject grEnergy;
    public Transform transProcessUpgrade;
    public Transform objGemEff1, objGemEff2;
    public GameObject objCompleteReward1, objCompleteReward2;
    List<UIUpgradeItem> listUpgradeItems = new List<UIUpgradeItem>();
    int selectedItemID;
    BigNumber priceUpgrade;
    public int energyRequire;
    public RectTransform rectTransform;
    [SerializeField] RectTransform rectTextPrice;
    float timeRebuildLayout = 0;
    IRoomController currentRoom;
    float dtTimeUpgradeHover;
    private void Awake() {
        //btnUpgrade.onClick.AddListener(() => {
        //    SoundManager.instance.PlaySoundEffect(SoundID.UPGRADE);
        //    OnUpgradeItem();
        //});
    }
    /// <summary>
    /// update status upgrade button & UIUpgradeItems when game money change
    /// </summary>
    private void Update() {
        if (timeRebuildLayout <= 1) {
            timeRebuildLayout += Time.deltaTime;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            LayoutRebuilder.MarkLayoutForRebuild(rectTextPrice);
        }
        btnUpgrade.interactable = IsEnoughConditionUpgrade();
        if (IsUpgradeButtonSelected && btnUpgrade.gameObject.activeInHierarchy) {
            dtTimeUpgradeHover -= Time.deltaTime;
            if (dtTimeUpgradeHover <= 0) {
                dtTimeUpgradeHover = 0.15f;
                SoundManager.instance.PlaySoundEffect(SoundID.UPGRADE);
                OnUpgradeItem();
            }
        }
    }

    public bool IsEnoughConditionUpgrade() {
        return GameManager.instance.IsEnoughCash(priceUpgrade) && GameManager.instance.IsEnoughEnergy(energyRequire);
    }
    /// <summary>
    /// Load UI by room control
    /// </summary>
    /// <param name="roomManager"></param>
    public void Setup() {
        rootUIItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        priceUpgrade = 0;
        energyRequire = 0;
        currentRoom = GameManager.instance.selectedRoom;
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
    string subColor1 = " <color=red>(+";
    string subColor2_1 = " <color=yellow>(+";
    string subColor2_2 = " <color=yellow>(-";
    string subColor3 = ") </color>";
    int energyRequire1;
    int energyRequire2;
    float timeReduce;
    float timeReduce2;
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
        btnMaxUpgrade.gameObject.SetActive(level == maxLevel);
        if (level == maxLevel) IsUpgradeButtonSelected = false;
        string strLevel = ProfileManager.Instance.dataConfig.GameText.GetTextByID(27).ToUpper() + ": ";
        txtItemLevel.text = strLevel + level.ToString();
        priceUpgrade = currentRoom.GetUpgradePriceItem(index);
        txtItemUpgradePrice.text = new BigNumber(priceUpgrade).ToString2();
        string strShow = ProfileManager.Instance.dataConfig.GameText.GetTextByID(109).ToUpper();
        string strUpgrade = ProfileManager.Instance.dataConfig.GameText.GetTextByID(149).ToUpper();
        txtUpgrade.text = level == 0 ? strShow : strUpgrade;
        if (currentRoom.GetRoomID() == RoomID.Power) {
            energyRequire = 0;
            //grEnergy.gameObject.SetActive(true);
            //grTimeReduce.gameObject.SetActive(false);
            //grMoney.gameObject.SetActive(false);
            //if (level == maxLevel) txtItemEnergy.text = currentRoom.GetEnergyEarnItem(index).ToString();
            //else txtItemEnergy.text = currentRoom.GetEnergyEarnItem(index).ToString() + " <color=yellow>(+" + currentRoom.GetEnergyEarnIncreaseInNextLevel(index).ToString() + ") </color>";

        } else {
            //grEnergy.gameObject.SetActive(true);
            //grTimeReduce.gameObject.SetActive(true);
            //grMoney.gameObject.SetActive(true);
            /*
            if (level == maxLevel) {
                txtItemMoneyEarn.text = currentRoom.GetMoneyEarnItem(index).ToString("0.00");
                txtItemEnergy.text = currentRoom.GetEnergyRequireItem(index).ToString();
                txtTimeReduce.text = "-" + currentRoom.GetTimeReduce(index).ToString() + "s";
                grTimeReduce.gameObject.SetActive(currentRoom.GetTimeReduce(index) != 0);
                grEnergy.gameObject.SetActive(currentRoom.GetEnergyRequireItem(index) > 0);
            } else {
                txtItemMoneyEarn.text = currentRoom.GetMoneyEarnItem(index).ToString("0.00") + subColor2_1 + currentRoom.GetMoneyIncreaseInNextLevel(index).ToString("0.00") + subColor3;
                energyRequire1 = currentRoom.GetEnergyRequireItem(index);
                energyRequire2 = currentRoom.GetEnergyRequirePreviorLevel(index);
                energyRequire = energyRequire1 - energyRequire2;
                grEnergy.gameObject.SetActive(energyRequire1 != 0 || energyRequire2 != 0);
                if (GameManager.instance.IsEnoughEnergy(energyRequire)) {
                    txtItemEnergy.text = energyRequire2.ToString() + subColor2_1 + energyRequire + subColor3;
                } else {
                    txtItemEnergy.text = energyRequire2.ToString() + subColor1 + energyRequire + subColor3;
                }
                timeReduce = currentRoom.GetTimeReduce(index);
                timeReduce2 = currentRoom.GetTimeReduceIncreaseInNextLevel(index);
                grTimeReduce.gameObject.SetActive(timeReduce != 0 || timeReduce2 != 0);
                txtTimeReduce.text = "-" + timeReduce + "s" + subColor2_2 + timeReduce2.ToString() + "s" + subColor3;
            }
             */
        }

        timeRebuildLayout = 0;
        CameraMove.instance.ChangePosition(-new Vector3(0, 6.5f, 0f) + GameManager.instance.selectedRoom.GetPositionItem(selectedItemID), null);
    }
    /// <summary>
    ///  Callback when user press upgrade button
    /// </summary>
    private void OnUpgradeItem() {
        /// check enough money true => upgrade model
        if (GameManager.instance.IsEnoughCash(priceUpgrade) && GameManager.instance.IsEnoughEnergy(energyRequire)) {
            btnUpgrade.transform.localScale = Vector3.one;
            btnUpgrade.transform.DOScale(new Vector3(1.15f, 1.15f, 1), 0.2f).SetEase(Ease.Linear).OnComplete(() => {
                btnUpgrade.transform.DOScale(new Vector3(1f, 1, 1), 0.2f);
            });
            BigNumber priceUpgradeCache = priceUpgrade;
            int energyRequireCache = energyRequire;
            /// call room control action Upgrade
            currentRoom.OnUpgradeItem(selectedItemID);
            /// reload info ui
            UIUpgradeItem ui = listUpgradeItems[selectedItemID];
            ui.SetupNextLevel(currentRoom.GetLevelItem(selectedItemID));
            OnShowInfoItem(selectedItemID);
            PanelRoomInfo.instance.SetupTabProfit();
            PanelRoomInfo.instance.OnPressUpgradeButton();
            EventManager.TriggerEvent(EventName.UpdateEnergy.ToString());
            ProfileManager.PlayerData.ConsumeCash(priceUpgradeCache);
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
