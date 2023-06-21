using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIResearchSlot : UIEffect {
    public EResearchSlotIndex slot;
    public Button btnSelect;
    public Image imgFood;
    public Text txtTime;
    public GameObject txtRequireVIP, imgLock;
    bool isUnlockSlot;
    ResearchType currentResearchOnSlot;
    private void Awake() {
        btnSelect.onClick.AddListener(OnSelect) ;
    }
    private void OnEnable() {
        //Setup();
    }
    public void Setup() {
        isUnlockSlot = ProfileManager.PlayerData.researchManager.IsUnlockResearchSlot(slot);
        if (isUnlockSlot) {
            currentResearchOnSlot = ProfileManager.PlayerData.researchManager.CheckCurrentResearchOnSlot(slot);
            if (currentResearchOnSlot == ResearchType.None) {
                imgLock.gameObject.SetActive(false);
                imgFood.gameObject.SetActive(false);
                txtTime.gameObject.SetActive(false);
                txtRequireVIP.gameObject.SetActive(false);
            } else {
                imgFood.gameObject.SetActive(true);
                txtTime.gameObject.SetActive(true);
                imgFood.sprite = ProfileManager.Instance.dataConfig.researchDataConfig.GetResearch(currentResearchOnSlot).foodIcon;
            }
        } else {
            currentResearchOnSlot = ResearchType.None;
            txtTime.gameObject.SetActive(false);
            imgFood.gameObject.SetActive(false);
            txtRequireVIP.gameObject.SetActive(slot == EResearchSlotIndex.Slot5 || slot == EResearchSlotIndex.Slot6);
            imgLock.gameObject.SetActive(true);
        }
    }
    private void Update() {
        Setup();
        float timeCoolDown = ProfileManager.PlayerData.researchManager.GetTimeEndResearch(currentResearchOnSlot);
        if (timeCoolDown > 0)
            txtTime.text = TimeUtil.TimeToString2(timeCoolDown);
        else txtTime.text = "";
    }
    private void OnSelect() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ScaleEffectButton(btnSelect);
        if (currentResearchOnSlot != ResearchType.None) PanelResearch.instance.ShowDetail(currentResearchOnSlot);
        else if (slot != EResearchSlotIndex.Slot5 && slot != EResearchSlotIndex.Slot6) PanelResearch.instance.ShowDetailUnlockSlot(slot);
        else UIManager.instance.ShowPanelOfferForProsPack();
    }
}
