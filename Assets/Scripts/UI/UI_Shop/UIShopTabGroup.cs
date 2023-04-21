using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShopTabGroup : MonoBehaviour {
    public List<UIShopTabButton> listTabs;
    UIShopTabButton selectedTab;
    private void Start() {
        OnInit();
    }
    void OnInit() {
        for (int i = 0; i < listTabs.Count; i++)
            listTabs[i].OnInit();
        if (selectedTab == null)
            OnSelected(listTabs[0], false);
    }
    public void OnSelected(UIShopTabButton tab, bool scroll) {
        if (tab != null && tab.gameObject.activeInHierarchy == false)
            return;
        if (selectedTab)
            selectedTab.Deselected();
        selectedTab = tab;
        if (selectedTab) selectedTab.Selected();
        if (scroll)
            UIShopManager.instance.scrollManager.ChangeTarget(tab.tabName);
        ResetAllTab();
    }
    public void OnDeselected() {
        ResetAllTab();
    }
    void ResetAllTab() {
        foreach (UIShopTabButton tab in listTabs) {
            if (selectedTab != null && tab == selectedTab)
                continue;
            tab.Deselected();
        }
    }
    public UIShopTabButton GetTabButton(TabName _tabName) {
        for (int i = 0; i < listTabs.Count; i++) {
            if (listTabs[i].tabName == _tabName) {
                return listTabs[i];
            }
        }
        return null;
    }
}
