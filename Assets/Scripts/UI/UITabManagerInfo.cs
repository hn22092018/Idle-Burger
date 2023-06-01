using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabManagerInfo : MonoBehaviour {
    public static UITabManagerInfo instance;
    public Image imgIcon, imgBorder;
    public Text txtLevel;
    public Sprite[] sprBorders;
    public Button btnShow;
    ManagerStaffID currentStaffID;
    private void Awake() {
        instance = this;
        btnShow.onClick.AddListener(OnShow);
    }
     void OnEnable() {
        OnLoadManagerInfo();
    }
    public void OnLoadManagerInfo() {
        currentStaffID = GameManager.instance.selectedRoom.GetManagerStaffID();
        if (currentStaffID == ManagerStaffID.None) return;
        CardManagerSave cardSaveInfo = ProfileManager.PlayerData.GetCardManager().GetCardManager(currentStaffID);
        txtLevel.text = "LV " + cardSaveInfo.level;
        if (cardSaveInfo.level == 0) {
            imgBorder.sprite = sprBorders[0];
            imgIcon.sprite = ProfileManager.Instance.dataConfig.cardData.GetCardManagerInfo(cardSaveInfo.staffID, cardSaveInfo.rarity).sprIconOff;
        } else {
            imgIcon.sprite = ProfileManager.Instance.dataConfig.cardData.GetCardManagerInfo(cardSaveInfo.staffID, cardSaveInfo.rarity).sprIcon;
            imgBorder.sprite = sprBorders[(int)cardSaveInfo.rarity-1];
        }
    }
    private void OnShow() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        UIManager.instance.ShowPanelManagerCardLevelUp();
    }
}
