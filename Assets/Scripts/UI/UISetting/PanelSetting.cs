using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

[System.Serializable] 
public class ToggleTransform
{
    public Transform handle;
    public Transform offPos;
    public Transform onPos;
}
public class PanelSetting : UIPanel {
    [SerializeField] Button btnClose, btnActivate;
    [SerializeField] Toggle toggleMusic, toggleSound;
    [SerializeField] ToggleTransform mussicToggle, soundToggle;
    bool firstToggle = false;
    [SerializeField] Text txtTitle, txtMusic, txtSound, txtLanguage;
    [SerializeField] Dropdown _DropdownLanguage;
    List<LanguageInfo> languageInfos = new List<LanguageInfo>();
    bool activate = false;

    [Header("=====GIFT CODE=====")]
    [SerializeField] Button redeemBtn;
    [SerializeField] InputField giftCodeInput;
    [SerializeField] GameObject invalidAlert;
    [SerializeField] GameObject transparnetAlert;
    [SerializeField] GameObject checkingLoad;
    [SerializeField] GameObject placeHolder;

    bool gettingRerward = false;
    float timeCounter = 0;
    [SerializeField] float getRewardDelay = 10f;

    public override void Awake() {
        panelType = UIPanelType.PanelSetting;
        base.Awake();
        btnClose.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            OnClose();
        });
        toggleMusic.onValueChanged.AddListener(delegate { ChangeMusicState(); });
        toggleSound.onValueChanged.AddListener(delegate { ChangeSoundState(); });
        toggleMusic.isOn = ProfileManager.Instance.IsMusicOn();
        toggleSound.isOn = ProfileManager.Instance.IsSoundOn();
        OnLoadLanguage();
        _DropdownLanguage.onValueChanged.AddListener(delegate {
            DropdownValueChanged(_DropdownLanguage);
        });
        btnActivate.onClick.AddListener(OnClick);
        redeemBtn.onClick.AddListener(RedeemCode);
    }
    public void OnClick() {
        activate = !activate;
        if (activate)
            _DropdownLanguage.Show();
        else
            _DropdownLanguage.Hide();
    }

    void ToggleStart()
    {
        if (toggleMusic.isOn)
        {
            mussicToggle.handle.position = mussicToggle.onPos.position;
        }
        else
        {
            mussicToggle.handle.position = mussicToggle.offPos.position;
        }
        if (toggleSound.isOn)
        {
            soundToggle.handle.position = soundToggle.onPos.position;
        }
        else
        {
            soundToggle.handle.position = soundToggle.offPos.position;
        }
        firstToggle = true;
    }
    
    void ChangeMusicState() {
        if(!firstToggle)
        {
            return;
        }
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ProfileManager.Instance.ChangeMusicState(toggleMusic.isOn);
        if (toggleMusic.isOn)
        {
            SoundManager.instance.PlayMusic();
            mussicToggle.handle.DOMove(mussicToggle.onPos.position, 0.2f);
        }
        else
        {
            SoundManager.instance.PauseMusic();
            mussicToggle.handle.DOMove(mussicToggle.offPos.position, 0.2f);
        }
    }
    void ChangeSoundState() {
        if (!firstToggle)
        {
            return;
        }
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ProfileManager.Instance.ChangeSoundState(toggleSound.isOn);
        if (toggleSound.isOn)
        {
            SoundManager.instance.PlaySound();
            soundToggle.handle.DOMove(soundToggle.onPos.position, 0.2f);
        }
        else
        {
            SoundManager.instance.PauseSound();
            soundToggle.handle.DOMove(soundToggle.offPos.position, 0.2f);
        }
    }
    public void OnClose() {
        UIManager.instance.ClosePanelSetting();
    }
    private void OnEnable() {
        ResetTextField();
        ToggleStart();
    }
    void OnLoadLanguage()
    {
        _DropdownLanguage.options.Clear();
        languageInfos = ProfileManager.Instance._LanguageManager._LanguageInfoList;
        LANGUAGE_ID id = ProfileManager.Instance._LanguageManager._CurrentLanguageID;
        for (int i = 0; i < languageInfos.Count; i++) {
            _DropdownLanguage.options.Add(new Dropdown.OptionData() { text = languageInfos[i].name });
            if (languageInfos[i].languageID == id) _DropdownLanguage.value = i;
        }
    }

    void DropdownValueChanged(Dropdown change) {
        activate = false;
        int value = change.value;
        ProfileManager.Instance._LanguageManager.SaveLanguage(languageInfos[value].languageID);
    }

    private void Update()
    {
        CheckInputField();
        CheckGettingCode();
    }

    void CheckInputField()
    {
        if (giftCodeInput.text != "")
        {
            placeHolder.SetActive(false);
            invalidAlert.SetActive(false);
            transparnetAlert.SetActive(false);
            if(!gettingRerward)
            {
                checkingLoad.SetActive(false);
            }
            if (giftCodeInput.text.Length >= 8)
            {
                redeemBtn.interactable = true;
            }
        }
        else
        {
            placeHolder.SetActive(true);
            redeemBtn.interactable = false;
        }
    }

    void CheckGettingCode()
    {
        if(gettingRerward)
        {
            timeCounter -= Time.deltaTime;
            if(timeCounter <= 0f)
            {
                MaintainTransparent();
            }
        }
    }

    async void RedeemCode()
    {
        checkingLoad.SetActive(true);
        gettingRerward = true;
        timeCounter = getRewardDelay;
        string code = giftCodeInput.text;
        RequestStatus redeemStatus = await GiftCodeManager.Instance.ClaimGiftCode(code);
        Debug.Log(redeemStatus.ToString());
        if (redeemStatus == RequestStatus.SUCCESS)
        {
            GotReward();
        }
        else
        {
            InvalidReward();
        }
    }

    void ResetTextField()
    {
        giftCodeInput.text = "";
        gettingRerward = false;
        invalidAlert.SetActive(false);
        transparnetAlert.SetActive(false);
        checkingLoad.SetActive(false);
    }

    void GotReward()
    {
        ResetTextField();
        OnClose();
    }

    void InvalidReward()
    {
        giftCodeInput.text = "";
        gettingRerward = false;
        invalidAlert.SetActive(true);
        transparnetAlert.SetActive(false);
        checkingLoad.SetActive(false);
    }

    void MaintainTransparent()
    {
        giftCodeInput.text = "";
        gettingRerward = false;
        transparnetAlert.SetActive(true);
        invalidAlert.SetActive(false);
        checkingLoad.SetActive(false);
    }
}
