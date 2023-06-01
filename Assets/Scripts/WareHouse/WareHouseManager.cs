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
public class WHTabCreateSave {
    public string strTimeTarget;
    public int countCreate;
}
[System.Serializable]
public class WareHouseManager {
    public List<WareHouseMaterialSave> wareHouseMaterialSaves;
    public WHTabCreateSave biscuitCreateSave;
    public WHTabCreateSave melonCreateSave;
    public WHTabCreateSave candyCreateSave;
    public WHTabCreateSave sushiCreateSave;
    public WHTabCreateSave potatoCreateSave;
    private int countCreateMax = 10;
    private int countEnergyMax = 30;
    public int currentEnergy;
    public string targetTimeAddEnergy;
    public float timeRemaining;
    public int wareHouseChest;
    public bool moreThanEnergyMax;
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
            LoadCreateSave(data);
            targetTimeAddEnergy = data.targetTimeAddEnergy;
            wareHouseChest = data.wareHouseChest;
            moreThanEnergyMax = data.moreThanEnergyMax;
            currentEnergy = data.currentEnergy;
            if (string.IsNullOrEmpty(targetTimeAddEnergy))
                currentEnergy = countEnergyMax;
            else if (data.currentEnergy < countEnergyMax) {
                DateTime timeTarget = DateTime.Parse(targetTimeAddEnergy);
                float ftimeTarget = (float)DateTime.Now.Subtract(timeTarget).TotalSeconds;
                ChangeCurrentEnergy((int)(ftimeTarget / 180f));
            }
        } else {
            biscuitCreateSave.countCreate = countEnergyMax;
            melonCreateSave.countCreate = countEnergyMax;
            candyCreateSave.countCreate = countEnergyMax;
            sushiCreateSave.countCreate = countEnergyMax;
            potatoCreateSave.countCreate = countEnergyMax;
            SaveData();
        }
    }
    void LoadCreateSave(WareHouseManager data) {
        biscuitCreateSave = data.biscuitCreateSave;
        melonCreateSave = data.melonCreateSave;
        candyCreateSave = data.candyCreateSave;
        sushiCreateSave = data.sushiCreateSave;
        potatoCreateSave = data.potatoCreateSave;
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
            case WareHouseMaterialType.Flour:
                return level > ProfileManager.Instance.dataConfig.wareHouseDataConfig.flour.Count;
            default:
                return false;
        }
    }
    public void SetTargetTime(WareHouseMaterialType wareHouseMaterialType) {
        switch (wareHouseMaterialType) {
            case WareHouseMaterialType.Cheese:
                biscuitCreateSave.strTimeTarget = DateTime.Now.AddMinutes(3f).ToString();
                break;
            case WareHouseMaterialType.Pepper:
                melonCreateSave.strTimeTarget = DateTime.Now.AddMinutes(3f).ToString();
                break;
            case WareHouseMaterialType.Sugar:
                candyCreateSave.strTimeTarget = DateTime.Now.AddMinutes(3f).ToString();
                break;
            case WareHouseMaterialType.Carot:
                sushiCreateSave.strTimeTarget = DateTime.Now.AddMinutes(3f).ToString();
                break;
            case WareHouseMaterialType.Flour:
                potatoCreateSave.strTimeTarget = DateTime.Now.AddMinutes(3f).ToString();
                break;
            default:
                break;
        }
        SaveData();
    }
    public string GetTargetTime(WareHouseMaterialType wareHouseMaterialType) {
        switch (wareHouseMaterialType) {
            case WareHouseMaterialType.Cheese:
                return biscuitCreateSave.strTimeTarget;
            case WareHouseMaterialType.Pepper:
                return melonCreateSave.strTimeTarget;
            case WareHouseMaterialType.Sugar:
                return candyCreateSave.strTimeTarget;
            case WareHouseMaterialType.Carot:
                return sushiCreateSave.strTimeTarget;
            case WareHouseMaterialType.Flour:
                return potatoCreateSave.strTimeTarget;
            default:
                return "";
        }
    }
    public void ResetCountCreate(WareHouseMaterialType wareHouseMaterialType) {
        switch (wareHouseMaterialType) {
            case WareHouseMaterialType.Cheese:
                biscuitCreateSave.strTimeTarget = "";
                biscuitCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Pepper:
                melonCreateSave.strTimeTarget = "";
                melonCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Sugar:
                candyCreateSave.strTimeTarget = "";
                candyCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Carot:
                sushiCreateSave.strTimeTarget = "";
                sushiCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Flour:
                potatoCreateSave.strTimeTarget = "";
                potatoCreateSave.countCreate = countCreateMax;
                break;
            default:
                break;
        }
        SaveData();
    }
    public void ChangeCountCreate(WareHouseMaterialType wareHouseMaterialType, int value) {
        switch (wareHouseMaterialType) {
            case WareHouseMaterialType.Cheese:
                biscuitCreateSave.countCreate += value;
                if (biscuitCreateSave.countCreate < 0)
                    biscuitCreateSave.countCreate = 0;
                else if (biscuitCreateSave.countCreate > countCreateMax)
                    biscuitCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Pepper:
                melonCreateSave.countCreate += value;
                if (melonCreateSave.countCreate < 0)
                    melonCreateSave.countCreate = 0;
                else if (melonCreateSave.countCreate > countCreateMax)
                    melonCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Sugar:
                candyCreateSave.countCreate += value;
                if (candyCreateSave.countCreate < 0)
                    candyCreateSave.countCreate = 0;
                else if (candyCreateSave.countCreate > countCreateMax)
                    candyCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Carot:
                sushiCreateSave.countCreate += value;
                if (sushiCreateSave.countCreate < 0)
                    sushiCreateSave.countCreate = 0;
                else if (sushiCreateSave.countCreate > countCreateMax)
                    sushiCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Flour:
                potatoCreateSave.countCreate += value;
                if (potatoCreateSave.countCreate < 0)
                    potatoCreateSave.countCreate = 0;
                else if (potatoCreateSave.countCreate > countCreateMax)
                    potatoCreateSave.countCreate = countCreateMax;
                break;
            default:
                break;
        }
        SaveData();
    }
    public int GetCountCreate(WareHouseMaterialType wareHouseMaterialType) {
        switch (wareHouseMaterialType) {
            case WareHouseMaterialType.Cheese:
                return biscuitCreateSave.countCreate;
            case WareHouseMaterialType.Pepper:
                return melonCreateSave.countCreate;
            case WareHouseMaterialType.Sugar:
                return candyCreateSave.countCreate;
            case WareHouseMaterialType.Carot:
                return sushiCreateSave.countCreate;
            case WareHouseMaterialType.Flour:
                return potatoCreateSave.countCreate;
            default:
                return 0;
        }
    }
    public int GetMaxCount() { return countCreateMax; }
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
        targetTimeAddEnergy = DateTime.Now.AddMinutes(3f).ToString();
        SaveData();
    }
    public void UsingEnergy() {
        if (currentEnergy == countEnergyMax)
            SetTimeTargetAddEnergy();
        ChangeCurrentEnergy(-1);
        SaveData();
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
