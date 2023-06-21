using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[CreateAssetMenu(fileName = "RoomDataAsset", menuName = "ScriptableObjects/RoomDataAsset", order = 1)]
public class RoomDataAsset<Enum> : ScriptableObject {
    public RoomID roomID;
    public float timeService;
    public float profitBase;
    public List<BaseModelData<Enum>> datas;
    public Transform GetModelByType(string type, int level) {
        for (int i = 0; i < datas.Count; i++) {
            if (datas[i].type.ToString() == type) {
                if (level <= 20) return datas[i].models3D[0].transform;
                else if (level <= 50) return datas[i].models3D[1].transform;
                else return datas[i].models3D[2].transform;
            }
        }
        return null;
    }
    public Sprite GetpriteModelByType(string type) {
        for (int i = 0; i < datas.Count; i++) {
            if (datas[i].type.ToString() == type) return datas[i].sprUI;
        }
        return null;
    }
    public int GetLevelMaxByType(string type) {
        for (int i = 0; i < datas.Count; i++) {
            if (datas[i].type.ToString() == type) return datas[i].levelMax;
        }
        return 0;
    }
    public string GetNameByType(string type) {
        for (int i = 0; i < datas.Count; i++) {
            if (datas[i].type.ToString() == type) {
                if (string.IsNullOrEmpty(ProfileManager.Instance.dataConfig.GameText.ItemNameToString(datas[i].type)))
                    return datas[i].name;
                else return ProfileManager.Instance.dataConfig.GameText.ItemNameToString(datas[i].type);
            }
        }
        return "";
    }
    public string GetInfoByType(string type) {
        return "";
    }


    public int GetEnergyRequire(string type, int level) {
        //for (int i = 0; i < datas.Count; i++) {
        //    if (datas[i].type.ToString() == type) {
        //        if (datas[i].energyRequires.Count == 0 || level < 0) return 0;
        //        if (level >= datas[i].energyRequires.Count) return datas[i].energyRequires[datas[i].energyRequires.Count - 1];
        //        return datas[i].energyRequires[level];
        //    }
        //}
        return 0;
    }

    public BigNumber GetUpgradePrice(string type, int level) {
        for (int i = 0; i < datas.Count; i++) {
            if (datas[i].type.ToString() == type) {
                if (datas[i].upgradePrices.Count == 0) return 0;
                if (level >= datas[i].upgradePrices.Count - 1) return datas[i].upgradePrices[datas[i].upgradePrices.Count - 1];
                return datas[i].upgradePrices[level];
            }
        }
        return 0;
    }
    public int GetEnergyEarn(string type, int level) {
        for (int i = 0; i < datas.Count; i++) {
            if (datas[i].type.ToString() == type) {
                if (datas[i].energyEarns.Count == 0) return 0;
                if (level >= datas[i].energyEarns.Count - 1) return datas[i].energyEarns[datas[i].energyEarns.Count - 1];
                return datas[i].energyEarns[level];
            }
        }
        return 0;
    }
}
