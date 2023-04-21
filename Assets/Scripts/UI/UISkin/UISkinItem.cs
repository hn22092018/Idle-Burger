using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkinItem : MonoBehaviour {
    [SerializeField] SkinItem skin;
    [SerializeField] Image imgIcon;
    [SerializeField] GameObject selectedObj;
    [SerializeField] GameObject lockObj;
    private void Awake() {
        GetComponent<Button>().onClick.AddListener(OnPreviewSkin);
    }

    private void OnPreviewSkin() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        PanelSkin.instance.OnPreviewSkin(skin);
        SetSelectedEffState(true);
    }
    public void SetupSkinItem(SkinItem s) {
        skin = s;
        imgIcon.sprite = skin.skinIcon;
        LoadLockState();
    }
    public void LoadLockState() {
        bool IsUnlock = ProfileManager.PlayerData.skinManager.IsContainSkin(skin.id);
        bool check = !IsUnlock && skin.skinType != SkinType.Default;
        lockObj.SetActive(check);
    }
    public void SetSelectedEffState(bool isShow) {
        selectedObj.SetActive(isShow);
    }
    [Button]
    void FindImgIcon() {
        imgIcon = transform.GetChild(0).gameObject.GetComponent<Image>();
    }
    [Button]
    void FindSelectedObject() {
        selectedObj = transform.GetChild(1).gameObject;
    }
    [Button]
    void FindLockObject() {
        lockObj = transform.GetChild(2).gameObject;
    }
}
