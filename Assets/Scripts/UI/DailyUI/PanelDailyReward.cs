using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelDailyReward : UIPanel {
    
    public Button buttonCollectReward, closeButton;
    public List<UIDailyRewardTab> uiDailyRewardList;
    public List<DailyRewards> dailyRewards;
    public override void Awake() {
        panelType = UIPanelType.PanelDailyReward;
        base.Awake();
        buttonCollectReward.onClick.AddListener(OnCollectReward);
        closeButton.onClick.AddListener(OnClose);
    }
    private void Update() {
        bool IsEnableCollect = !ProfileManager.Instance.playerData.dailyRewardManager.IsCollectedDay();
        buttonCollectReward.interactable = IsEnableCollect;
    }
    public void InitUI() {
        dailyRewards = ProfileManager.Instance.dataConfig.dailyRewardConfig.rewards;
        for (int i=0; i< uiDailyRewardList.Count; i++) {
            if(i< dailyRewards.Count) {
                uiDailyRewardList[i].Setup(dailyRewards[i]);
            }
        }
    }
    void OnCollectReward() {
        ProfileManager.Instance.playerData.dailyRewardManager.CollectNow();
        InitUI();
    }

    void OnClose() {
        UIManager.instance.ClosePanelDailyReward();
    }
}
