using SDK;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
public class PanelRoomInfo : UIPanel {
    public static PanelRoomInfo instance;
    [SerializeField] RectTransform _RectGroupButtonTabs;
    [SerializeField] private Button btnClose, btnTabUpgrade, btnTabStaff;
    [SerializeField] private Sprite[] sprBtnTabs;
    [SerializeField] private Color[] clrTxtBtnTabs;
    [SerializeField] private Text[] txtBtnTabs;
    public UITabUpgrade uiTabUpgrade;
    public UITabStaff uiTabStaff;
    [SerializeField] private UITabProfit uiTabProfit;
    [SerializeField] private Text txtRoomName;
    public override void Awake() {
        panelType = UIPanelType.PanelRoomInfo;
        base.Awake();
        instance = this;
        btnClose.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            OnClose();
        });
        btnTabUpgrade.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.TAB_SWITCH);
            OnShowTabUpgrade();
        });
        btnTabStaff.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.TAB_SWITCH);
            OnShowTabStaff();
        });
      
    }

   
    private void OnShowTabUpgrade() {
        Debug.Log("OnShowTabUpgrade");
        btnTabUpgrade.image.sprite = sprBtnTabs[1];
        btnTabStaff.image.sprite = sprBtnTabs[0];
        txtBtnTabs[0].color = clrTxtBtnTabs[1];
        txtBtnTabs[1].color = clrTxtBtnTabs[0];
        uiTabStaff.gameObject.SetActive(false);
        uiTabUpgrade.gameObject.SetActive(true);
        uiTabUpgrade.Setup();
        SetupTabProfit();
        GameManager.instance.selectedRoom.TurnOnSelectedEffectItem(0);
    }
    public void OnShowTabStaff() {
        GameManager.instance.selectedRoom.TurnOffSelectedEffectItem();
        btnTabUpgrade.image.sprite = sprBtnTabs[0];
        btnTabStaff.image.sprite = sprBtnTabs[1];
        txtBtnTabs[0].color = clrTxtBtnTabs[0];
        txtBtnTabs[1].color = clrTxtBtnTabs[1];
        uiTabUpgrade.gameObject.SetActive(false);
        uiTabStaff.gameObject.SetActive(true);
        uiTabStaff.Setup();
        SetupTabProfit();

    }
    public void OnShowInfoItem(int index) {
        GameManager.instance.selectedRoom.TurnOnSelectedEffectItem(index);
        uiTabUpgrade.OnShowInfoItem(index);
        if (Tutorials.instance.IsShow) {
            TutorialStepID step = Tutorials.instance.GetTutorialStep();
            if (step == TutorialStepID.UpgradeTable) {
                Tutorials.instance.OnShow(uiTabUpgrade.btnUpgrade.transform);
            }
        }

    }
    /// <summary>
    /// Load UI by room control
    /// </summary>
    /// <param name="roomManager"></param>
    public void Setup(IRoomController roomManager, bool IsShowUpgradeFirst = true) {
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) AdsManager.Instance.ShowInterstitial(null, null);
        SoundManager.instance.PlaySubMusic(roomManager.GetRoomID());
        btnTabStaff.gameObject.SetActive(false);
        //if (roomManager.GetStaffSetting() == null || roomManager.GetRoomID() == RoomID.Manager) {
        //    btnTabStaff.gameObject.SetActive(false);
        //} else btnTabStaff.gameObject.SetActive(roomManager.GetStaffSetting().GetTotalStaff() > 0);
        txtRoomName.text = ProfileManager.Instance.dataConfig.GameText.RoomIDToString(roomManager.GetRoomID()).ToUpper();
        if (IsShowUpgradeFirst) OnShowTabUpgrade();
        else OnShowTabStaff();
        btnTabStaff.interactable = true;
        btnTabUpgrade.interactable = true;
        StartCoroutine(CheckTutWhenOpenPopup());

    }

    IEnumerator CheckTutWhenOpenPopup() {

        if (Tutorials.instance.IsShow) {
            Tutorials.instance.OffBlocker();
            yield return new WaitForSeconds(0.7f);
            TutorialStepID step = Tutorials.instance.GetTutorialStep();
            if (step == TutorialStepID.UpgradeTable) {
                isBlockClose = true;
                Transform slot = uiTabUpgrade.rootUIItem.GetChild(3).transform;
                Tutorials.instance.OnShow(slot);
            } 
        }
    }

    public void SetupTabProfit() {
        uiTabProfit.Setup();

    }
    public void OnPressUpgradeButton() {
        if (Tutorials.instance.IsShow) {
            TutorialStepID step = Tutorials.instance.GetTutorialStep();
            if (step == TutorialStepID.UpgradeTable) {
                Tutorials.instance.FinishTutorial();
                OnShowTutClosePanel();
            }
        }
    }

    bool isBlockClose;
   public void OnShowTutClosePanel() {
        isBlockClose = false;
        float posX = -GetComponent<RectTransform>().rect.width / 2 + 170f;
        float posY = btnClose.GetComponent<RectTransform>().anchoredPosition.y;
        Tutorials.instance.ChangeBlockerPos(new Vector2(posX, posY));
    }
    public void OnClose() {
        if (isBlockClose) return;
        SoundManager.instance.StopSubMusic();
        CameraMove.instance.ZoomInCamera(20);
        Tutorials.instance.OnCloseTutorial();
        GameManager.instance.StopFocusRoom();
        UIManager.instance.ClosePanelRoomInfo();
    }
    private void OnEnable() {
        AdsManager.Instance.HideBannerAds();
    }
}
