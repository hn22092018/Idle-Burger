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
            case WareHouseMaterialType.Biscuit:
                return level > ProfileManager.Instance.dataConfig.wareHouseDataConfig.biscuit.Count;
            case WareHouseMaterialType.Melon:
                return level > ProfileManager.Instance.dataConfig.wareHouseDataConfig.melon.Count;
            case WareHouseMaterialType.Candy:
                return level > ProfileManager.Instance.dataConfig.wareHouseDataConfig.candy.Count;
            case WareHouseMaterialType.Sushi:
                return level > ProfileManager.Instance.dataConfig.wareHouseDataConfig.sushi.Count;
            case WareHouseMaterialType.Potato:
                return level > ProfileManager.Instance.dataConfig.wareHouseDataConfig.potato.Count;
            default:
                return false;
        }
    }
    public void SetTargetTime(WareHouseMaterialType wareHouseMaterialType) {
        switch (wareHouseMaterialType) {
            case WareHouseMaterialType.Biscuit:
                biscuitCreateSave.strTimeTarget = DateTime.Now.AddMinutes(3f).ToString();
                break;
            case WareHouseMaterialType.Melon:
                melonCreateSave.strTimeTarget = DateTime.Now.AddMinutes(3f).ToString();
                break;
            case WareHouseMaterialType.Candy:
                candyCreateSave.strTimeTarget = DateTime.Now.AddMinutes(3f).ToString();
                break;
            case WareHouseMaterialType.Sushi:
                sushiCreateSave.strTimeTarget = DateTime.Now.AddMinutes(3f).ToString();
                break;
            case WareHouseMaterialType.Potato:
                potatoCreateSave.strTimeTarget = DateTime.Now.AddMinutes(3f).ToString();
                break;
            default:
                break;
        }
        SaveData();
    }
    public string GetTargetTime(WareHouseMaterialType wareHouseMaterialType) {
        switch (wareHouseMaterialType) {
            case WareHouseMaterialType.Biscuit:
                return biscuitCreateSave.strTimeTarget;
            case WareHouseMaterialType.Melon:
                return melonCreateSave.strTimeTarget;
            case WareHouseMaterialType.Candy:
                return candyCreateSave.strTimeTarget;
            case WareHouseMaterialType.Sushi:
                return sushiCreateSave.strTimeTarget;
            case WareHouseMaterialType.Potato:
                return potatoCreateSave.strTimeTarget;
            default:
                return "";
        }
    }
    public void ResetCountCreate(WareHouseMaterialType wareHouseMaterialType) {
        switch (wareHouseMaterialType) {
            case WareHouseMaterialType.Biscuit:
                biscuitCreateSave.strTimeTarget = "";
                biscuitCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Melon:
                melonCreateSave.strTimeTarget = "";
                melonCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Candy:
                candyCreateSave.strTimeTarget = "";
                candyCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Sushi:
                sushiCreateSave.strTimeTarget = "";
                sushiCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Potato:
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
            case WareHouseMaterialType.Biscuit:
                biscuitCreateSave.countCreate += value;
                if (biscuitCreateSave.countCreate < 0)
                    biscuitCreateSave.countCreate = 0;
                else if (biscuitCreateSave.countCreate > countCreateMax)
                    biscuitCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Melon:
                melonCreateSave.countCreate += value;
                if (melonCreateSave.countCreate < 0)
                    melonCreateSave.countCreate = 0;
                else if (melonCreateSave.countCreate > countCreateMax)
                    melonCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Candy:
                candyCreateSave.countCreate += value;
                if (candyCreateSave.countCreate < 0)
                    candyCreateSave.countCreate = 0;
                else if (candyCreateSave.countCreate > countCreateMax)
                    candyCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Sushi:
                sushiCreateSave.countCreate += value;
                if (sushiCreateSave.countCreate < 0)
                    sushiCreateSave.countCreate = 0;
                else if (sushiCreateSave.countCreate > countCreateMax)
                    sushiCreateSave.countCreate = countCreateMax;
                break;
            case WareHouseMaterialType.Potato:
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
            case WareHouseMaterialType.Biscuit:
                return biscuitCreateSave.countCreate;
            case WareHouseMaterialType.Melon:
                return melonCreateSave.countCreate;
            case WareHouseMaterialType.Candy:
                return candyCreateSave.countCreate;
            case WareHouseMaterialType.Sushi:
                return sushiCreateSave.countCreate;
            case WareHouseMaterialType.Potato:
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
