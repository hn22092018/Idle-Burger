using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
public enum TabName {
    Normal,
    OfflineTime,
    Financial,
    Chest,
    Gems,
    Sale,
    Suggest,
    None,
    Tickets,
    TimeTravel, 
    SkinBox
}
public class UIShopTabButton : MonoBehaviour, IPointerClickHandler, IPointerExitHandler {
    public TabName tabName;
    public Color[] colorsIcon;
    public Sprite bgDeselect, bgSelect;
    public Image icon, backGround;
    [SerializeField] GameObject notify;
    public UnityEvent onSelect, onDeselect;
    UIShopTabGroup uITabGroup;
    public void OnInit() {
        if (transform.GetSiblingIndex() == UIShopManager.instance.scrollManager.GetIndexSelected(tabName)) {
            backGround.sprite = bgSelect;
            icon.color = colorsIcon[0];
            return;
        }
        backGround.sprite = bgDeselect;
        icon.color = colorsIcon[1];
    }
    public void ChangeIcon(Sprite whatIcon) {
        icon.sprite = whatIcon;
    }
    public void Selected() {
        backGround.sprite = bgSelect;
        icon.color = colorsIcon[0];
        if (onSelect != null)
            onSelect.Invoke();
    }
    public void Deselected() {
        backGround.sprite = bgDeselect;
        icon.color = colorsIcon[1];
        if (onDeselect != null)
            onDeselect.Invoke();
    }
    public void OnPointerClick(PointerEventData eventData) {
        SoundManager.instance.PlaySoundEffect(SoundID.TAB_SWITCH);
        uITabGroup = UIShopManager.instance.tabGroup;
        uITabGroup.OnSelected(this, true);
    }
    public void OnPointerExit(PointerEventData eventData) {
        uITabGroup = UIShopManager.instance.tabGroup;
        uITabGroup.OnDeselected();
    }
    public void OnClose() {
        gameObject.SetActive(false);
    }
    public void OnOpen() {
        gameObject.SetActive(true);
    }
    void Update() {
        if (tabName == TabName.Chest) CheckFreeChestNotify();
    }
    void CheckFreeChestNotify() {
        bool isHasFreeChest = ProfileManager.PlayerData.boxManager.IsHasFreeChest();
        if (notify) notify.SetActive(isHasFreeChest);
    }
}
