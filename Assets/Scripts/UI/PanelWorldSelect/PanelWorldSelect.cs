using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelWorldSelect : UIPanel {
    [SerializeField] Button exitBtn;
    [SerializeField] WorldInfoItem worldInfoItem;
    [SerializeField] List<WorldInfoItem> worldsInfo;
    [SerializeField] Transform worldItemContainer;
    [SerializeField] Text starCountTxt;

    public override void Awake() {
        panelType = UIPanelType.PanelWorldSelect;
        base.Awake();
        exitBtn.onClick.AddListener(OnClose);
    }
    public void OnClose() {
        UIManager.instance.ClosePanelWorldSelect();
    }
    private void OnEnable() {
        if (worldsInfo.Count == 0) {
            SetUp();
        }
        UpdateWorldsInfo();
    }
    private void Update() {
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep()==TutorialStepID.SelectNewWorld) {
            Tutorials.instance.FinishTutorial();
            Tutorials.instance.OnCloseTutorial();
        }
    }
    public void SetUp() {
        List<WorldBaseData> worldBaseDatas = ProfileManager.Instance.dataConfig.worldDataAsset.worldBaseDatas;
        for (int i = 0; i < worldBaseDatas.Count; i++) {
            WorldInfoItem item = Instantiate(worldInfoItem, worldItemContainer);
            item.Setup(worldBaseDatas[i]);
            worldsInfo.Add(item);
        }
    }
    void UpdateWorldsInfo() {
    }

}
