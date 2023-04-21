using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class WareHouseSlotMaterial : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerClickHandler {
    public WareHouseMergePoint lastPoint;
    public WareHouseMergePoint currentPoint;
    public WareHouseMaterial myMaterial;
    public Canvas myCanvas;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image imgIcon;
    RectTransform rectTransform;
    int level;
    PanelWareHouse wareHousePanel;
    public bool activate;
    public bool onHint;
    Vector3 vectorScaleDefault = new Vector3(1, 1, 1);
    Vector3 vectorMergePunch = new Vector3(.5f, .5f, 0);
    Vector3 vectorHintPunch = new Vector3(.1f, .1f, 0);

    public int Level { get => level; set => level = value; }

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        myCanvas = UIManager.instance.mainCanvas.GetComponent<Canvas>();
        GameObject go = UIManager.instance.GetPanel(UIPanelType.PanelWareHouse);
        wareHousePanel = go.GetComponent<PanelWareHouse>();
    }
    void Start() {
        InvokeRepeating("ApplyHint", .1f, 1f);
    }
    void ApplyHint() {
        if (onHint)
        {
            imgIcon.transform.localScale = Vector3.one;
            imgIcon.transform.DOPunchScale(vectorHintPunch, .25f).OnComplete(() => {
                imgIcon.transform.localScale = Vector3.one;
            });
        }
    }
    public void InitData(WareHouseMaterial material, int level) {
        imgIcon.sprite = material.icon;
        myMaterial = material;
        this.level = level;
        OnActivate();
    }
    public void ChangePoint(WareHouseMergePoint pointChange) {
        if (currentPoint != null)
            lastPoint = currentPoint;
        currentPoint = pointChange;
        transform.position = pointChange.transform.position;
    }
    public void OnBeginDrag(PointerEventData eventData) {
        transform.SetAsLastSibling();
        transform.localScale = vectorScaleDefault;
        canvasGroup.blocksRaycasts = false;
        if (currentPoint != null)
            lastPoint = currentPoint;
        ShowDetail();
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (currentPoint == lastPoint) {
            transform.position = lastPoint.transform.position;
        }
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta / myCanvas.scaleFactor;
    }

    public void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag != null) {
            WareHouseSlotMaterial wareHouseSlotMaterial = eventData.pointerDrag.GetComponent<WareHouseSlotMaterial>();
            OnAcceptPoint(wareHouseSlotMaterial);
        }
    }
    void OnAcceptPoint(WareHouseSlotMaterial wareHouseSlotMaterial) {
        if (myMaterial.priceSale == wareHouseSlotMaterial.myMaterial.priceSale
            && myMaterial.wareHouseMaterialType == wareHouseSlotMaterial.myMaterial.wareHouseMaterialType)
            OnMergePoint(wareHouseSlotMaterial);
        else OnSwitchPoint(wareHouseSlotMaterial);
    }
    void OnMergePoint(WareHouseSlotMaterial wareHouseSlotMaterial) {
        AnimMerge();
        level++;
        if (IsCheckMaxLevel()) {
            OnMaxLevel();
            wareHouseSlotMaterial.OnDeActivate();
            wareHousePanel.CheckDuplicate();
            return;
        }
        WareHouseMaterial wareHouseMaterial = ProfileManager.Instance.dataConfig.wareHouseDataConfig.GetWareHouseMaterial(myMaterial.wareHouseMaterialType, level);
        InitData(wareHouseMaterial, level);
        wareHousePanel.ShowDetail(this, wareHouseMaterial, level);
        ProfileManager.PlayerData.wareHouseManager.LevelUpWareHouseMaterial(wareHouseMaterial.wareHouseMaterialType, level - 1);
        ProfileManager.PlayerData.wareHouseManager.RemoveWareHouseMaterialSaves(wareHouseMaterial.wareHouseMaterialType, level - 1);
        wareHouseSlotMaterial.OnDeActivate();
        wareHousePanel.CheckDuplicate();
    }
    void OnSwitchPoint(WareHouseSlotMaterial wareHouseSlotMaterial) {
        WareHouseMergePoint myCurrentPoint = currentPoint;
        ChangePoint(wareHouseSlotMaterial.currentPoint);
        currentPoint.able = false;
        wareHouseSlotMaterial.ChangePoint(myCurrentPoint);
        wareHouseSlotMaterial.currentPoint.able = false;
    }
    void OnMaxLevel() {
        wareHousePanel.OnCompleteMaterial(transform.position);
        wareHousePanel.CloseDetail();
        ProfileManager.PlayerData.wareHouseManager.RemoveWareHouseMaterialSaves(myMaterial.wareHouseMaterialType, level - 1);
        ProfileManager.PlayerData.wareHouseManager.RemoveWareHouseMaterialSaves(myMaterial.wareHouseMaterialType, level - 1);
        ProfileManager.PlayerData.wareHouseManager.ChangeWareHouseChest(1);
        OnDeActivate();
    }
    bool IsCheckMaxLevel() {
        return ProfileManager.PlayerData.wareHouseManager.IsMaxLevel(myMaterial.wareHouseMaterialType, level);
    }
    public void OnPointerClick(PointerEventData eventData) {
        ShowDetail();
    }
    void ShowDetail() {
        wareHousePanel.ShowDetail(this, myMaterial, level);
    }
    public void OnActivate() {
        currentPoint.able = false;
        imgIcon.gameObject.SetActive(true);
        activate = true;
    }
    public void OnDeActivate() {
        currentPoint.able = true;
        imgIcon.gameObject.SetActive(false);
        activate = false;
        level = 0;
        myMaterial = null;
    }
    void AnimMerge() {
        Transform go = wareHousePanel.MergeEffect();
        go.transform.position = transform.position;
        imgIcon.transform.localScale = Vector3.one;
        imgIcon.transform.DOPunchScale(vectorMergePunch, .25f);
        Destroy(go.gameObject, 1f);
    }
    public void OnHint() {
        onHint = true;
    }
    public void OffHint() {
        onHint = false;
    }
}
