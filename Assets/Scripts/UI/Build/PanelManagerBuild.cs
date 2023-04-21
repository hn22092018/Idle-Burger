using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManagerBuild : UIPanel {
    BuildData data;
    BuildDataSetting buildData;
    [SerializeField] private Image imgBuild;
    [SerializeField] private Text txtBuildName, txtBuildDes, txtEnergyRequire, txtCashRequire, txtStarRequire;
    [SerializeField] private Button btnBuild, btnGoPower, btnClose;
    [SerializeField]
    private GameObject grStarRequire, grEnergyRequire, grEnergyAndMoneyRequire;
    float price;
    int energy;
    [SerializeField] private RectTransform rectTransformRequire;
    [SerializeField] private RectTransform mainContent;
    int GroupID;
    float timeRebuildLayout;
    string sStarRequire = "Require Restaurant Reach {0}";
    public override void Awake() {
        panelType = UIPanelType.PanelManagerBuild;
        base.Awake();
        btnBuild.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            OnBuild();
        });
        btnClose.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            OnClose();
        });
        btnGoPower.onClick.AddListener(GoToPowerRoom);
    }

    public void Show(RoomID buildTarget, int GroupID = 0) {
        this.GroupID = GroupID;
        if (data == null) data = GameManager.instance.buildData;
        buildData = data.GetData(buildTarget);
        imgBuild.sprite = buildData.sprBuild;
        txtBuildName.text = buildData.GetBuildName().ToUpper();
        txtBuildDes.text = buildData.GetDes();
        energy = buildData.energyRequire;
        txtEnergyRequire.text = energy.ToString();
        grEnergyRequire.gameObject.SetActive(energy > 0);
        price = data.GetBuildCashPrice(buildTarget);
        txtCashRequire.text = new BigNumber(price).IntToString();
        sStarRequire = ProfileManager.Instance.dataConfig.GameText.GetTextByID(472);
        txtStarRequire.text = string.Format(sStarRequire, buildData.starRequire);
        if (Tutorials.instance.IsShow) {
            float y = mainContent.anchoredPosition.y + mainContent.rect.height / 2 + btnBuild.GetComponent<RectTransform>().anchoredPosition.y;
            Tutorials.instance.ChangeBlockerPos(new Vector2(0, y));
        }
    }

    private void OnEnable() {
        timeRebuildLayout = 0;
    }
    private void Update() {
        if (timeRebuildLayout <= 1) {
            timeRebuildLayout += Time.deltaTime;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransformRequire);
        }
        btnBuild.interactable = GameManager.instance.IsEnoughCash(price) && GameManager.instance.IsEnoughEnergy(energy) && ProfileManager.PlayerData.GetTotalStarEarned() >= buildData.starRequire;
        if (buildData.starRequire > 0) {
            if (ProfileManager.PlayerData.GetTotalStarEarned() < buildData.starRequire) {
                btnGoPower.gameObject.SetActive(false);
                grEnergyAndMoneyRequire.gameObject.SetActive(false);
                grStarRequire.gameObject.SetActive(true);
            } else {
                grEnergyAndMoneyRequire.gameObject.SetActive(true);
                grStarRequire.gameObject.SetActive(false);
                btnGoPower.gameObject.SetActive(!GameManager.instance.IsEnoughEnergy(energy));
            }
        } else {
            grEnergyAndMoneyRequire.gameObject.SetActive(true);
            grStarRequire.gameObject.SetActive(false);
            btnGoPower.gameObject.SetActive(!GameManager.instance.IsEnoughEnergy(energy));
        }
    }
    void OnBuild() {
        if (Tutorials.instance.IsShow) {
            Tutorials.instance.FinishTutorial();
            Tutorials.instance.OnCloseTutorial();
        }
        if (GameManager.instance.IsEnoughCash(price) && GameManager.instance.IsEnoughEnergy(energy)) {
            float priceUpgradeCache = price;
            GameManager.instance.BuildRoom(buildData.buildTarget, GroupID);
            EventManager.TriggerEvent(EventName.UpdateEnergy.ToString());
            ProfileManager.Instance.playerData.ConsumeCash(priceUpgradeCache);
            OnClose();
        }
    }
    public void GoToPowerRoom() {
        GameManager.instance.GoToPowerRoom();
    }

    public void OnClose() {
        UIManager.instance.ClosePanelManagerBuild();
    }
}
