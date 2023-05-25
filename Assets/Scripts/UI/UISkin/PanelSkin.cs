using SDK;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnClosePoup();
public class PanelSkin : UIPanel {
    public static PanelSkin instance;
    public OnClosePoup _OnClosePopup;
    [SerializeField] Button btnClose, btnBuyCash, btnBuyGem, btnUnlockVideo;
    [SerializeField] Button btnSelect, btnRemove;
    [SerializeField] Text txtCash, txtGem, txtVideo;
    [SerializeField] Text txtSkinName, txtDiligence, txtSkinWorkEfficiency, txtSkinIncome, txtSkinSpecialSkill;
    [SerializeField] Text txtSkinState;
    [SerializeField] Image imgSkinIcon;
    [SerializeField] Transform rootUISkin;
    [SerializeField] List<SkinItem> _SkinItems = new List<SkinItem>();
    [SerializeField] List<UISkinItem> _UISkinItems = new List<UISkinItem>();
    [SerializeField] RectTransform _RectTransformLayout;
    [SerializeField] int _SelectedIndexStaff;
    string sDefault = "Default";
    string sUsing = "Using";
    string sRequire_PremiumSuit = "Require Premeium Suit";
    string sRequire_GoldenSuit = "Require Golden Suit";
    string sWorkEfficiency = "Work Efficiency: {0}% faster";
    string sIncome = "Income Bonus: {0}% chance X2";
    string sSpecialSkill = "Special Skills: {0}% chance to trigger work is 200% faster for 30s";
    string sDiligence = "Diligence";
    string sNormalWolking = "Normal Working";
    string sHardWolking = "Hard Working";
    string sCrazyWolking = "Crazy Working";
    SkinItem _SelectedSkin;
    int _AdsNeedToUnlock;
    bool isCheckPrice;
    float timeRebuildLayout = 0;
    public override void Awake() {
        panelType = UIPanelType.PanelSkin;
        base.Awake();
        instance = this;
        btnClose.onClick.AddListener(OnClose);
        btnBuyCash.onClick.AddListener(OnBuyCash);
        btnBuyGem.onClick.AddListener(OnBuyGem);
        btnUnlockVideo.onClick.AddListener(OnWatchVideoToUnlock);
        btnSelect.onClick.AddListener(OnSelectSkin);
        btnRemove.onClick.AddListener(OnRemoveSkin);
    }
    private void OnEnable() {
        sDefault = ProfileManager.Instance.dataConfig.GameText.GetTextByID(464);
        sUsing = ProfileManager.Instance.dataConfig.GameText.GetTextByID(463);
        sRequire_PremiumSuit = ProfileManager.Instance.dataConfig.GameText.GetTextByID(461);
        sRequire_GoldenSuit = ProfileManager.Instance.dataConfig.GameText.GetTextByID(462);
        sWorkEfficiency = ProfileManager.Instance.dataConfig.GameText.GetTextByID(466);
        sIncome = ProfileManager.Instance.dataConfig.GameText.GetTextByID(467);
        sSpecialSkill = ProfileManager.Instance.dataConfig.GameText.GetTextByID(468);
        sDiligence = ProfileManager.Instance.dataConfig.GameText.GetTextByID(465);
        sNormalWolking = ProfileManager.Instance.dataConfig.GameText.GetTextByID(469);
        sHardWolking = ProfileManager.Instance.dataConfig.GameText.GetTextByID(470);
        sCrazyWolking = ProfileManager.Instance.dataConfig.GameText.GetTextByID(471);
        timeRebuildLayout = 0;
    }
    bool isShowTut;
    private void Update() {
        if (timeRebuildLayout < 0.8f) {
            timeRebuildLayout += Time.deltaTime;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_RectTransformLayout);
            if (!isShowTut && Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() == TutorialStepID.HireStaff) {
                isShowTut = true;
                Tutorials.instance.OnShow(btnBuyCash.transform);
            }
        }

        if (isCheckPrice) btnBuyCash.interactable = GameManager.instance.IsEnoughCash(_SelectedSkin.skinPrice);
    }

    /// <summary>
    /// Load ALl Skin with staff type selected
    /// </summary>
    /// <param name="staffType"></param>
    public void LoadListSkin(StaffID staffType) {
        _SelectedIndexStaff = PanelRoomInfo.instance.uiTabStaff._SelectedIndexStaff;
        _SkinItems = ProfileManager.PlayerData.skinManager.GetListSkinConfigByStaffType(staffType);
        for (int i = 0; i < _UISkinItems.Count; i++) {
            if (i < _SkinItems.Count) {
                _UISkinItems[i].gameObject.SetActive(true);
                _UISkinItems[i].SetupSkinItem(_SkinItems[i]);
            } else _UISkinItems[i].gameObject.SetActive(false);
        }
        // If Has skin in current select index. Show info it.
        // else show default skin info
        int skinIDUsing = ProfileManager.PlayerData.skinManager.GetSkinIDInIndex(_SelectedIndexStaff, staffType);
        if (skinIDUsing >= 0) {
            for (int i = 0; i < _SkinItems.Count; i++) {
                if (_SkinItems[i].id == skinIDUsing) {
                    OnPreviewSkin(_SkinItems[i]);
                    _UISkinItems[i].SetSelectedEffState(true);
                    break;
                }
            }
        } else {
            OnPreviewSkin(_SkinItems[0]);
            _UISkinItems[0].SetSelectedEffState(true);
        }
    }
    /// <summary>
    /// Show Skin Item Info.
    /// </summary>
    /// <param name="selectSkin"></param>
    public void OnPreviewSkin(SkinItem selectSkin) {
        for (int i = 0; i < _UISkinItems.Count; i++) {
            _UISkinItems[i].SetSelectedEffState(false);
        }
        _SelectedSkin = selectSkin;
        imgSkinIcon.sprite = selectSkin.skinIcon;
        txtSkinName.text = selectSkin.skinName;
        txtDiligence.text = sDiligence + ": " + EnumDiliencyWorkToString(selectSkin.diligenceWork) + " " + SleepInfoBySkinType(selectSkin.skinType);
        txtSkinWorkEfficiency.text = string.Format(sWorkEfficiency, selectSkin.workEfficiency);
        txtSkinIncome.text = string.Format(sIncome, selectSkin.incomeX2Chance);
        txtSkinSpecialSkill.text = string.Format(sSpecialSkill, selectSkin.specialSkillChance);
        btnBuyCash.gameObject.SetActive(false);
        btnBuyGem.gameObject.SetActive(false);
        btnUnlockVideo.gameObject.SetActive(false);
        btnSelect.gameObject.SetActive(false);
        btnRemove.gameObject.SetActive(false);
        txtSkinState.gameObject.SetActive(true);
        isCheckPrice = false;
        // check skin unlocked or not ?
        if (ProfileManager.PlayerData.skinManager.IsContainSkin(selectSkin.id)) {
            // check skin using or not.
            // if using => off select button & show state is using
            if (selectSkin.skinType == SkinType.Default) {
                // Skin Default For All Staff
                txtSkinState.text = sUsing;
                // default
                if (PanelRoomInfo.instance.uiTabStaff.isHiring) {
                    txtSkinState.text = "";
                    isCheckPrice = true;
                    selectSkin.skinPrice = GameManager.instance.GetSkinDefaultPrice(_SelectedIndexStaff);
                    txtCash.text = selectSkin.skinPrice.ToString();
                    btnBuyCash.gameObject.SetActive(true);
                }
            } else if (ProfileManager.PlayerData.skinManager.IsSkinUsed(selectSkin.id)) {

                txtSkinState.text = sUsing;
                // if selected skin is match with saver skin in selected index.
                // enable remove skin
                int skinIDUsingSaver = ProfileManager.PlayerData.skinManager.GetSkinIDInIndex(_SelectedIndexStaff, _SelectedSkin.staffType);
                if (skinIDUsingSaver == selectSkin.id) {
                    txtSkinState.text = "";
                    if (PanelRoomInfo.instance.uiTabStaff.isHiring) {
                        btnRemove.gameObject.SetActive(false);
                        btnSelect.gameObject.SetActive(true);
                    } else {
                        btnRemove.gameObject.SetActive(true);
                        btnSelect.gameObject.SetActive(false);
                    }
                }
            } else {
                txtSkinState.text = "";
                btnSelect.gameObject.SetActive(true);
            }

        } else {
            // if skin not unlock. show select button or require with vip skin
            txtSkinState.text = "";
            if (selectSkin.skinType == SkinType.Default) {
                txtSkinState.text = sDefault;
                // default
                if (PanelRoomInfo.instance.uiTabStaff.isHiring) {
                    txtSkinState.text = "";
                    isCheckPrice = true;
                    selectSkin.skinPrice = GameManager.instance.GetSkinDefaultPrice(_SelectedIndexStaff);
                    txtCash.text = selectSkin.skinPrice.ToString();
                    btnBuyCash.gameObject.SetActive(true);
                }
            } else if (selectSkin.skinType == SkinType.Cash) {
                isCheckPrice = true;
                txtCash.text = selectSkin.skinPrice.ToString();
                btnBuyCash.gameObject.SetActive(true);
            } else if (selectSkin.skinType == SkinType.Gem) {
                txtGem.text = selectSkin.skinPrice.ToString();
                btnBuyGem.gameObject.SetActive(true);
                btnBuyGem.interactable = GameManager.instance.IsEnoughGem(selectSkin.skinPrice);
            } else if (selectSkin.skinType == SkinType.Video) {
                _AdsNeedToUnlock = ProfileManager.PlayerData.skinManager.GetAdsNeedToUnlock(selectSkin);
                txtVideo.text = _AdsNeedToUnlock.ToString() + "/" + selectSkin.skinPrice;
                btnUnlockVideo.gameObject.SetActive(_AdsNeedToUnlock > 0);
            }
            if (selectSkin.skinType == SkinType.PremiumSuit) txtSkinState.text = sRequire_PremiumSuit;
            else if (selectSkin.skinType == SkinType.GoldenSuit) txtSkinState.text = sRequire_GoldenSuit;
        }
    }
    string EnumDiliencyWorkToString(DiligenceWork diligenceWork) {
        return diligenceWork switch {
            DiligenceWork.Normal => sNormalWolking,
            DiligenceWork.Hard => sHardWolking,
            DiligenceWork.Crazy => sCrazyWolking,
            _ => throw new NotImplementedException(),
        };
    }
    string SleepInfoBySkinType(SkinType type) {
        return type switch {
            SkinType.Default => "(6% Sleep)",
            SkinType.Video => "(3% Sleep)",
            SkinType.Gem => "(Never Sleep)",
            SkinType.PremiumSuit => "(Never Sleep)",
            SkinType.GoldenSuit => "(Never Sleep)",
            _ => throw new NotImplementedException(),
        };
    }
    private void OnWatchVideoToUnlock() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        if (ProfileManager.PlayerData.ResourceSave.activeRemoveAds) {
            ProfileManager.PlayerData.skinManager.WatchedAdsToGetSkin(_SelectedSkin.id);
            _AdsNeedToUnlock--;
            txtVideo.text = _AdsNeedToUnlock.ToString() + "/" + _SelectedSkin.skinPrice;
            btnUnlockVideo.gameObject.SetActive(_AdsNeedToUnlock > 0);
            if (_AdsNeedToUnlock <= 0) {
                OnSelectSkin();
                ReloadUI();
            }
        } else {
            AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.UnlockSkin.ToString(), () => {
                ProfileManager.PlayerData.skinManager.WatchedAdsToGetSkin(_SelectedSkin.id);
                _AdsNeedToUnlock--;
                txtVideo.text = _AdsNeedToUnlock.ToString() + "/" + _SelectedSkin.skinPrice;
                btnUnlockVideo.gameObject.SetActive(_AdsNeedToUnlock > 0);
                if (_AdsNeedToUnlock <= 0) {
                    OnSelectSkin();
                    ReloadUI();
                }
            });
        }

    }
    private void OnBuyGem() {
        if (GameManager.instance.IsEnoughGem(_SelectedSkin.skinPrice)) {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            ProfileManager.PlayerData.ConsumeGem(_SelectedSkin.skinPrice);
            ProfileManager.PlayerData.skinManager.AddSkin(_SelectedSkin.id);
            btnBuyGem.gameObject.SetActive(false);
            OnSelectSkin();
            ReloadUI();
            ABIAnalyticsManager.Instance.TrackEventGem(GemAction.Spend_Skin, _SelectedSkin.skinPrice);
        }
    }

    private void OnBuyCash() {
        if (GameManager.instance.IsEnoughCash(_SelectedSkin.skinPrice)) {
            if (isShowTut) {
                Tutorials.instance.FinishTutorial();
                Tutorials.instance.OnShow(btnClose.transform);
            }
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            ProfileManager.PlayerData.ConsumeCash(_SelectedSkin.skinPrice);
            ProfileManager.PlayerData.skinManager.AddSkin(_SelectedSkin.id);
            btnBuyCash.gameObject.SetActive(false);
            OnSelectSkin();
            ReloadUI();
            isCheckPrice = false;
        }
    }
    private void OnSelectSkin() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        // check to remove old skin
        int skinIDUsingInCurrentIndex = ProfileManager.PlayerData.skinManager.GetSkinIDInIndex(_SelectedIndexStaff, _SelectedSkin.staffType);
        if (skinIDUsingInCurrentIndex >= 0 && skinIDUsingInCurrentIndex != _SelectedSkin.id) {
            ProfileManager.PlayerData.skinManager.UnUseSkin(_SelectedIndexStaff, _SelectedSkin.staffType);
        }
        // update new skin
        ProfileManager.PlayerData.skinManager.UpdateSkin(_SelectedSkin.id, _SelectedIndexStaff);
        btnSelect.gameObject.SetActive(false);
        txtSkinState.text = sUsing;
        PanelRoomInfo.instance.uiTabStaff.CheckOnHire();
        PanelRoomInfo.instance.uiTabStaff.UpdateSkinUI(_SelectedIndexStaff);
        EventManager.TriggerEvent(EventName.UpdateStaffSuit.ToString(), _SelectedSkin.staffType);
    }
    private void OnRemoveSkin() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ProfileManager.PlayerData.skinManager.UnUseSkin(_SelectedIndexStaff, _SelectedSkin.staffType);
        btnRemove.gameObject.SetActive(false);
        btnSelect.gameObject.SetActive(true);
        txtSkinState.text = "";
        PanelRoomInfo.instance.uiTabStaff.UpdateSkinUI(_SelectedIndexStaff);
        EventManager.TriggerEvent(EventName.UpdateStaffSuit.ToString(), _SelectedSkin.staffType);
    }
    void ReloadUI() {
        for (int i = 0; i < _UISkinItems.Count; i++) {
            _UISkinItems[i].LoadLockState();
        }
    }
    private void OnClose() {
        if (isShowTut) {
            isShowTut = false;
            Tutorials.instance.OnCloseTutorial();
        }
        if (_OnClosePopup != null) _OnClosePopup();
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        UIManager.instance.ClosePanelSkin();
    }
    [Button]
    void LoadDefaultUIListSkin() {
        _UISkinItems.Clear();
        for (int i = 0; i < rootUISkin.childCount; i++) {
            _UISkinItems.Add(rootUISkin.GetChild(i).GetComponent<UISkinItem>());
        }
    }
}
