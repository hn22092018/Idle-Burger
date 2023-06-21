using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelWareHouse : UIPanel {
    public List<WareHouseSlotMaterial> wareHouseSlotMaterials;
    public List<WareHouseMergePoint> wareHouseMergePoints;
    [SerializeField] WareHouseDetailPanel wareHouseDetailPanel;
    [SerializeField] WareHouseSlotMaterial wareHouseSlotMaterial;
    [SerializeField] WareHouseMaterialCompletePanel completePanel;
    [SerializeField] WareHouseOpenChestPanel wareHouseOpenChestPanel;
    [SerializeField] WareHouseWatchVideoPanel wareHouseWatchVideoPanel;
    [SerializeField] Text txtWareHouseChestCount;
    [SerializeField] Button btnExit;
    [SerializeField] Button btnShowOpenChestPanel;
    [SerializeField] Transform objMergeEffect;
    [SerializeField] Transform wareHouseSlotParent;
    [SerializeField] Transform wareHouseEffectParent;
    [SerializeField] Transform noticeParent;
    [SerializeField] ImageEffect chestEffect, noticeEffect;
    [SerializeField] float timeCheckDuplicateSetting;
    WareHouseSlotMaterial duplicateSlot1;
    WareHouseSlotMaterial duplicateSlot2;
    public override void Awake() {
        panelType = UIPanelType.PanelWareHouse;
        base.Awake();
        btnExit.onClick.AddListener(Close);
        btnShowOpenChestPanel.onClick.AddListener(ShowOpenChest);
        wareHouseOpenChestPanel.gameObject.SetActive(false);
        wareHouseWatchVideoPanel.gameObject.SetActive(false);
    }
    private void Update() {
        txtWareHouseChestCount.text = ProfileManager.PlayerData.wareHouseManager.wareHouseChest.ToString();
        if (ProfileManager.Instance.playerData.wareHouseManager.IsHaveEnoughChest(1)) {
            chestEffect.OnActive();
            noticeEffect.gameObject.SetActive(true);
            noticeEffect.OnActive();
        } else noticeEffect.gameObject.SetActive(false);

    }
    public void OnOpen() {
        completePanel.gameObject.SetActive(false);
        wareHouseDetailPanel.gameObject.SetActive(false);
        wareHouseOpenChestPanel.gameObject.SetActive(false);
        StartCoroutine(IE_WaitRebuild());
    }
    IEnumerator IE_WaitRebuild() {
        yield return new WaitForSeconds(.1f);
        InitData(ProfileManager.PlayerData.wareHouseManager.GetListMaterialsSaver());
    }
    void InitData(List<WareHouseMaterialSave> wareHouseMaterialSaves) {
        for (int i = 0; i < wareHouseMergePoints.Count; i++) {
            if (i > 0) {
                if (i % 6 == 0)
                    wareHouseMergePoints[i].colorDark = wareHouseMergePoints[i - 1].colorDark;
                else wareHouseMergePoints[i].colorDark = !wareHouseMergePoints[i - 1].colorDark;
            }
            wareHouseMergePoints[i].ChangeColor();
            if (i >= wareHouseMaterialSaves.Count) {
                wareHouseSlotMaterials[i].ChangePoint(wareHouseMergePoints[i]);
                wareHouseSlotMaterials[i].OnDeActivate();
                wareHouseMergePoints[i].able = true;
                continue;
            }
            WareHouseMaterial material = ProfileManager.Instance.dataConfig.wareHouseDataConfig.GetWareHouseMaterial(wareHouseMaterialSaves[i].wareHouseMaterialType, wareHouseMaterialSaves[i].level);
            wareHouseSlotMaterials[i].ChangePoint(wareHouseMergePoints[i]);
            wareHouseSlotMaterials[i].InitData(material, wareHouseMaterialSaves[i].level);
            wareHouseMergePoints[i].able = false;
        }
        CheckDuplicate();
    }
    public void AddMaterial(WareHouseMaterialType materialType) {
        for (int i = 0; i < wareHouseMergePoints.Count; i++) {
            if (wareHouseMergePoints[i].able) {
                int index = GetAbleSlotWareHouse();
                if (index == -1) {
                    return;
                }
                WareHouseMaterial wareHouseMaterial = ProfileManager.Instance.dataConfig.wareHouseDataConfig.GetWareHouseMaterial(materialType, 1);
                ProfileManager.Instance.playerData.wareHouseManager.AddWareHouseMaterialSaves(wareHouseMaterial);
                wareHouseSlotMaterials[index].ChangePoint(wareHouseMergePoints[i]);
                wareHouseSlotMaterials[index].InitData(wareHouseMaterial, 1);
                wareHouseMergePoints[i].able = false;
                CheckDuplicate();
                return;
            }
        }
    }
  public  int GetAbleSlotWareHouse() {
        for (int i = 0; i < wareHouseSlotMaterials.Count; i++) {
            if (!wareHouseSlotMaterials[i].activate)
                return i;
        }
        return -1;
    }
    public Vector3   GetAnchorAbleSlotWareHouse() {
        for (int i = 0; i < wareHouseSlotMaterials.Count; i++) {
            if (!wareHouseSlotMaterials[i].activate && !wareHouseSlotMaterials[i].isSelect) {
                wareHouseSlotMaterials[i].isSelect = true;
                return wareHouseSlotMaterials[i].transform.position;
            }
        }
        return Vector3.zero;
    }
        void Close() { UIManager.instance.ClosePanelWareHouse(); }
    public void ShowDetail(WareHouseSlotMaterial wareHouseSlotMaterial, WareHouseMaterial wareHouseMaterial, int level) {
        wareHouseDetailPanel.gameObject.SetActive(true);
        wareHouseDetailPanel.ShowDetail(wareHouseSlotMaterial, wareHouseMaterial, level);
    }

    public void OnCompleteMaterial(Vector3 startPoint) {
        completePanel.gameObject.SetActive(true);
        completePanel.PrepareAnim(startPoint, btnShowOpenChestPanel.transform.position);
    }

    public void AddEnergy() {
        wareHouseWatchVideoPanel.gameObject.SetActive(true);
    }
    public Transform MergeEffect() {
        return Instantiate(objMergeEffect, wareHouseEffectParent);
    }

    public void CheckDuplicate() {
        OffHint();
        for (int i = 0; i < wareHouseMergePoints.Count - 1; i++) {
            if (wareHouseSlotMaterials[i].myMaterial == null)
                continue;
            for (int j = i + 1; j < wareHouseMergePoints.Count; j++) {
                if (wareHouseSlotMaterials[j].myMaterial == null)
                    continue;
                if (IsMatchMaterial(wareHouseSlotMaterials[i], wareHouseSlotMaterials[j])) {
                    duplicateSlot1 = wareHouseSlotMaterials[i];
                    duplicateSlot1.OnHint();
                    duplicateSlot2 = wareHouseSlotMaterials[j];
                    duplicateSlot2.OnHint();
                    return;
                }
            }
        }
    }
    bool IsMatchMaterial(WareHouseSlotMaterial a, WareHouseSlotMaterial b) {
        return a.myMaterial.wareHouseMaterialType == b.myMaterial.wareHouseMaterialType && a.Level == b.Level;
    }
    void OffHint() {
        if (duplicateSlot1 != null)
            duplicateSlot1.OffHint();
        if (duplicateSlot2 != null)
            duplicateSlot2.OffHint();
        duplicateSlot1 = null;
        duplicateSlot2 = null;
    }

    void ShowOpenChest() {
        wareHouseOpenChestPanel.InitData();
        wareHouseOpenChestPanel.gameObject.SetActive(true);
    }

    public void MergeUpdate() {
        if (ProfileManager.PlayerData.wareHouseManager.GetListMaterialsSaver().Count <= wareHouseMergePoints.Count) return;
        List<WareHouseMaterialSave> wareHouseMaterialSaves = ProfileManager.PlayerData.wareHouseManager.GetListMaterialsSaver();
        for (int i = 0; i < wareHouseMergePoints.Count; i++) {
            if (wareHouseMergePoints[i].able) {
                int index = GetAbleSlotWareHouse();
                WareHouseMaterial wareHouseMaterial = ProfileManager.Instance.dataConfig.wareHouseDataConfig.GetWareHouseMaterial(wareHouseMaterialSaves[wareHouseMaterialSaves.Count - 1].wareHouseMaterialType, wareHouseMaterialSaves[wareHouseMaterialSaves.Count - 1].level);
                wareHouseSlotMaterials[index].ChangePoint(wareHouseMergePoints[i]);
                wareHouseSlotMaterials[index].InitData(wareHouseMaterial, wareHouseMaterialSaves[wareHouseMaterialSaves.Count - 1].level);
                wareHouseMergePoints[i].able = false;
                return;
            }
        }
    }
}