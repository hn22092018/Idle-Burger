using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using DG.Tweening;
using SDK;
public class WareHouseOpenChestPanel : MonoBehaviour {
    [SerializeField] Button btnExit;
    [SerializeField] Button btnOpenX1Chest;
    [SerializeField] Button btnOpenX10Chest;
    [SerializeField] Button btnTabToClose;
    [SerializeField] Text txtChestCount;
    public List<WarehouseSlotReward> warehouseSlotRewardsX1 = new List<WarehouseSlotReward>();
    public List<ItemReward> itemRewards = new List<ItemReward>();
    bool onOpen, onSkip;
    float timeWait;
    [SerializeField] float timeWaitClaim;
    [SerializeField] float timeNormal;
    [SerializeField] WareHousePreviewRewardPanel wareHousePreviewRewardPanel;
    [SerializeField] GameObject objTitle;
    [SerializeField] GameObject objRequire;
    [SerializeField] GameObject objChestCount;
    private void Start() {
        btnExit.onClick.AddListener(Exit);
        btnTabToClose.onClick.AddListener(Exit);
        btnOpenX1Chest.onClick.AddListener(OpenX1Chest);
        btnOpenX10Chest.onClick.AddListener(OpenX10Chest);
    }
    public void InitData() {
        for (int i = 0; i < warehouseSlotRewardsX1.Count; i++)
            warehouseSlotRewardsX1[i].OnTurnOff();
        objRequire.SetActive(!ProfileManager.PlayerData.wareHouseManager.IsHaveEnoughChest(1));
        objTitle.SetActive(true);

    }
    private void OnEnable() {
        btnTabToClose.gameObject.SetActive(!ProfileManager.PlayerData.wareHouseManager.IsHaveEnoughChest(1));
    }
    void Update() {
        txtChestCount.text = ProfileManager.PlayerData.wareHouseManager.wareHouseChest.ToString();
        btnOpenX1Chest.gameObject.SetActive(ProfileManager.PlayerData.wareHouseManager.IsHaveEnoughChest(1) && !onOpen);
        btnOpenX10Chest.gameObject.SetActive(ProfileManager.PlayerData.wareHouseManager.IsHaveEnoughChest(10) && !onOpen);
        objChestCount.SetActive(!onOpen);
    }
    void Exit() {
        btnTabToClose.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    void OpenX1Chest() {
        ABIAnalyticsManager.Instance.TrackEventWareHouse(WareHouseAction.Open1Box);
        ProfileManager.PlayerData.wareHouseManager.ChangeWareHouseChest(-1);
        OpenChest(1);
    }
    void OpenX10Chest() {
        ABIAnalyticsManager.Instance.TrackEventWareHouse(WareHouseAction.Open10Box);
        ProfileManager.PlayerData.wareHouseManager.ChangeWareHouseChest(-10);
        OpenChest(10);
    }
    void OpenChest(int countChestOpen) {
        btnExit.gameObject.SetActive(false);
        for (int i = 0; i < warehouseSlotRewardsX1.Count; i++)
            warehouseSlotRewardsX1[i].OnTurnOff();
        btnOpenX1Chest.gameObject.SetActive(false);
        btnOpenX10Chest.gameObject.SetActive(false);
        onOpen = true;
        timeWait = timeNormal;
        itemRewards.Clear();
        index = 0;

        for (int i = 0; i < countChestOpen * 3; i++) {
            ItemReward itemReward = ProfileManager.Instance.dataConfig.wareHouseDataConfig.GetWareHouseReward();
            ItemReward newItemReward = new ItemReward();
            newItemReward.type = itemReward.type;
            newItemReward.spr = itemReward.spr;
            newItemReward.amount = GameManager.instance.GetRewardValueForWareHouse(itemReward.type);
            SumReward(newItemReward);
            Claim(newItemReward);
        }
        StartCoroutine(StartAnim());
    }
    void SumReward(ItemReward itemReward) {
        for (int i = 0; i < itemRewards.Count; i++) {
            if (itemReward.type == itemRewards[i].type) {
                itemRewards[i].amount += itemReward.amount;
                return;
            }
        }
        itemRewards.Add(itemReward);
    }
    int index;
    IEnumerator StartAnim() {
        yield return new WaitForSeconds(timeWait);
        warehouseSlotRewardsX1[index].gameObject.SetActive(true);
        if (!onSkip) {
            wareHousePreviewRewardPanel.gameObject.SetActive(true);
            wareHousePreviewRewardPanel.InitPanel(itemRewards[index].spr, warehouseSlotRewardsX1[index], ContinuteAnim);
        } else ContinuteAnim();
    }
    void ContinuteAnim() {
        if (index == itemRewards.Count) EndAnim();
        warehouseSlotRewardsX1[index].InitData(itemRewards[index]);
        index++;
        if (index < itemRewards.Count)
            StartCoroutine(StartAnim());
        else EndAnim();
    }
    void EndAnim() {
        btnExit.gameObject.SetActive(true);
        onOpen = false;
        btnTabToClose.gameObject.SetActive(!ProfileManager.PlayerData.wareHouseManager.IsHaveEnoughChest(1));
    }
    void Claim(ItemReward itemReward) {
        switch (itemReward.type) {
            case ItemType.Gem:
                ProfileManager.PlayerData.AddGem(itemReward.amount);
                ABIAnalyticsManager.Instance.TrackEventGem(GemAction.Earn_WareHouse_Box, itemReward.amount);
                break;
            case ItemType.Cash:
                ProfileManager.PlayerData.AddCash(itemReward.amount);
                break;
            case ItemType.Reputation:
                ProfileManager.PlayerData.researchManager.AddResearchValue(itemReward.amount);
                break;
            default:
                break;
        }
    }
}
