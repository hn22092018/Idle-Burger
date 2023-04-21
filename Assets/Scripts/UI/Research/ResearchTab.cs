using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ResearchTab : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    public bool IsBlock;
    public PanelResearch panelResearch;
    [SerializeField] Text txtName;
    [SerializeField] Image imgBGTab;
    [SerializeField] Color c_Select;
    [SerializeField] Color c_Deselect;
    [SerializeField] Color c_NameTextSelect;
    [SerializeField] Color c_NameTextDeSelect;
    [SerializeField] RectTransform rect;
    public int index;
    public UnityEvent onSelect, onDeselect;
    UnityAction action;
    public void InitData(int index, GroupResearchName tabName, PanelResearch panelResearch) {
        this.index = index;
        txtName.text = tabName.ToString();
        this.panelResearch = panelResearch;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsBlock) return;
        panelResearch.OnSelectTab(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsBlock) return;
        panelResearch.OnDeSelectTab();
    }
    public void OnSelect() {
        if (onSelect != null)
            onSelect.Invoke();
        imgBGTab.color = c_Select;
        txtName.color = c_NameTextSelect;
        rect.DOSizeDelta(new Vector2(250, 120), 0.2f, true);
    }
    public void OnDeselect(UnityAction action = null) {
        if (onDeselect != null)
            onDeselect.Invoke();
        this.action = action;
        imgBGTab.color = c_Deselect;
        txtName.color = c_NameTextDeSelect;
        rect.DOSizeDelta(new Vector2(200, 100), 0.15f, true).OnComplete(ActiveSelect);
    }
    void ActiveSelect() {
        if (action != null)
        {
            action();
            action = null;
        }
    }
}
