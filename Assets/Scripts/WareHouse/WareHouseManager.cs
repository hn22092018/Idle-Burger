using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class WareHouseMaterialSave {
    public WareHouseMaterialType wareHouseMaterialType;
    public int level;
}

[System.Serializable]
public class WareHouseManager {
    public List<WareHouseMaterialSave> wareHouseMaterialSaves;
    int countEnergyMax = 30;
    public int currentEnergy;
    public string targetTimeAddEnergy;
    public float timeRemaining;
    public int wareHouseChest;
    public bool moreThanEnergyMax;
    float timeRefill = 60f;
    public void Update() {
        if (currentEnergy < countEnergyMax) {
            if (!string.IsNullOrEmpty(targetTimeAddEnergy)) {
                timeRemaining = (float)DateTime.Parse(targetTimeAddEnergy).Subtract(DateTime.Now).TotalSeconds;
            } else currentEnergy = countEnergyMax;
            if (timeRemaining <= 0) {
                ChangeCurrentEnergy(1);
                if (currentEnergy < countEnergyMax)
                    SetTimeTargetAddEnergy();
            }
        } else { timeRemaining = 0; }
    }
    public void AddWareHouseMaterialSaves(WareHouseMaterial wareHouseMaterial, int level = 1) {
        WareHouseMaterialSave newWareHouseMaterialSave = new WareHouseMaterialSave();
        newWareHouseMaterialSave.wareHouseMaterialType = wareHouseMaterial.wareHouseMaterialType;
        newWareHouseMaterialSave.level = level;
        wareHouseMaterialSaves.Add(newWareHouseMaterialSave);
    }
    public void AddWareHouseMaterialSaves(WareHouseMaterialType type, int level = 1) {
        WareHouseMaterialSave newWareHouseMaterialSave = new WareHouseMaterialSave();
        newWareHouseMaterialSave.wareHouseMaterialType = type;
        newWareHouseMaterialSave.level = level;
        wareHouseMaterialSaves.Add(newWareHouseMaterialSave);
    }
    public void RemoveWareHouseMaterialSaves(WareHouseMaterialType materialType, int level) {
        for (int i = 0; i < wareHouseMaterialSaves.Count; i++) {
            if (wareHouseMaterialSaves[i].wareHouseMaterialType == materialType && wareHouseMaterialSaves[i].level == level) {
                wareHouseMaterialSaves.Remove(wareHouseMaterialSaves[i]);
                SaveData();
                break;
            }
        }
    }
    public void LevelUpWareHouseMaterial(WareHouseMaterialType materialType, int level) {

        for (int i = 0; i < wareHouseMaterialSaves.Count; i++) {
            if (wareHouseMaterialSaves[i].wareHouseMaterialType == materialType && wareHouseMaterialSaves[i].level == level) {
                wareHouseMaterialSaves[i].level++;
                SaveData();
                break;
            }
        }
    }
    public void LoadData() {
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData)) {
            WareHouseManager data = JsonUtility.FromJson<WareHouseManager>(jsonData);
            wareHouseMaterialSaves = data.wareHouseMaterialSaves;
            targetTimeAddEnergy = data.targetTimeAddEnergy;
            wareHouseChest = data.wareHouseChest;
            moreThanEnergyMax = data.moreThanEnergyMax;
            currentEnergy = data.currentEnergy;
            if (string.IsNullOrEmpty(targetTimeAddEnergy))
                currentEnergy = countEnergyMax;
            else if (data.currentEnergy < countEnergyMax) {
                DateTime timeTarget = DateTime.Parse(targetTimeAddEnergy);
                float ftimeTarget = (float)DateTime.Now.Subtract(timeTarget).TotalSeconds;
                ChangeCurrentEnergy((int)(ftimeTarget / timeRefill));
            }
        } else {
            SaveData();
        }
    }

    string GetJsonData() { return PlayerPrefs.GetString("WareHouseManager"); }
    public void SaveData() { PlayerPrefs.SetString("WareHouseManager", JsonUtility.ToJson(this).ToString()); }
    public bool IsMaxLevel(WareHouseMaterialType wareHouseMaterialType, int level) {
        switch (wareHouseMaterialType) {
            case WareHouseMaterialType.Cheese:
                return level > ProfileManager.Instance.dataConfig.wareHouseDataConfig.cheese.Count;
            case WareHouseMaterialType.Pepper:
                return level > ProfileManager.Instance.dataConfig.wareHouseDataConfig.pepper.Count;
            case WareHouseMaterialType.Sugar:
                return level > ProfileManager.Instance.dataConfig.wareHouseDataConfig.sugar.Count;
            case WareHouseMaterialType.Carot:
                return level > ProfileManager.Instance.dataConfig.wareHouseDataConfig.carot.Count;
            default:
                return false;
        }
    }

    public void ChangeCurrentEnergy(int value, bool overMax = false) {
        currentEnergy += value;
        if (overMax) moreThanEnergyMax = true;
        if (currentEnergy > countEnergyMax && !moreThanEnergyMax)
            currentEnergy = countEnergyMax;
        if (currentEnergy < countEnergyMax && moreThanEnergyMax)
            moreThanEnergyMax = false;
        if (currentEnergy < 0)
            currentEnergy = 0;
        SaveData();
    }
    public bool IsHaveEnergy() { return currentEnergy > 0; }
    public void SetTimeTargetAddEnergy() {
        targetTimeAddEnergy = DateTime.Now.AddMinutes(timeRefill / 60).ToString();
        SaveData();
    }
    public void UsingEnergy() {
        if (currentEnergy == countEnergyMax)
            SetTimeTargetAddEnergy();
        ChangeCurrentEnergy(-1);
        SaveData();
    }
    public string GetEnergyProcess() {
        return currentEnergy + "/" + countEnergyMax;
    }
    public void ChangeWareHouseChest(int value) { wareHouseChest += value; SaveData(); }
    public bool IsHaveEnoughChest(int value) { return wareHouseChest >= value; }
    public bool IsHasNotify() {
        if (wareHouseChest > 0 || currentEnergy >= 20) return true;
        return false;
    }
    public List<WareHouseMaterialSave> GetListMaterialsSaver() {
        wareHouseMaterialSaves.Sort((a, b) => (b.level - a.level));
        return wareHouseMaterialSaves;
    }
}
