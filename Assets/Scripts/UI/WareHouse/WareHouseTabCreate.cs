using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Spine.Unity;
using PathologicalGames;
using DG.Tweening;

public class WareHouseTabCreate : MonoBehaviour {
    [SerializeField] Button btnCreate;
    [SerializeField] PanelWareHouse myWareHousePanel;
    [SerializeField] Text txtEnergyCount;
    [SerializeField] Text txtTimeEnergyCoolDown;
    public SkeletonGraphic spine;
    public PanelWareHouse controller;
    [SpineAnimation]
    public string spinAnimation;
    public Transform spawnerEff;
    private void Awake() {
        btnCreate.onClick.AddListener(CreateMaterial);
    }

    private void OnEnable() {
        spine.AnimationState.SetAnimation(0, "idle", true);
    }

    void CreateMaterial() {
        if (controller.GetAbleSlotWareHouse() < 0) return;
        spine.AnimationState.SetAnimation(0, spinAnimation, false);
        if (!ProfileManager.PlayerData.wareHouseManager.IsHaveEnergy()) {
            MoreEnergy();
            return;
        }
        ProfileManager.PlayerData.wareHouseManager.UsingEnergy();
        SpawnerMat();
       
    }
    void SpawnerMat() {
        Transform t = PoolManager.Pools["WareHouseEffect"].Spawn(spawnerEff);
        t.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-1100);
        Vector3 target = controller.GetAnchorAbleSlotWareHouse();
        t.DOMove(target, 0.8f).OnComplete(()=> {
            WareHouseMaterialType materialType = (WareHouseMaterialType)UnityEngine.Random.Range(0, 4);
            myWareHousePanel.AddMaterial(materialType);
            PoolManager.Pools["WareHouseEffect"].Despawn(t);
        });
    }
    void MoreEnergy() {
        controller.AddEnergy();
    }
    private void Update() {
        if (ProfileManager.PlayerData.wareHouseManager.timeRemaining > 0) {
            txtTimeEnergyCoolDown.gameObject.SetActive(true);
            txtTimeEnergyCoolDown.text = TimeUtil.RemainTimeToString4(ProfileManager.PlayerData.wareHouseManager.timeRemaining);
        } else txtTimeEnergyCoolDown.gameObject.SetActive(false);
        txtEnergyCount.text = ProfileManager.PlayerData.wareHouseManager.GetEnergyProcess();
    }

}
