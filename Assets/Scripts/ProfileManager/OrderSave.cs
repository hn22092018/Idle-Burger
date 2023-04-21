using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class OrderSave 
{
    public OrderBookManager orderManager_1= new OrderBookManager();
    public OrderBookManager orderManager_2 = new OrderBookManager();
    public OrderBookManager orderManager_3 = new OrderBookManager();
    public int maxOrder;
    public bool isBoughtOrderStaffPack;
    bool isChangeData;

    public void IsMarkChangeData() {
        isChangeData = true;
    }
    public void LoadData() {
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData)) {
            OrderSave dataSave = JsonUtility.FromJson<OrderSave>(jsonData);
            orderManager_1 = dataSave.orderManager_1;
            orderManager_2 = dataSave.orderManager_2;
            orderManager_3 = dataSave.orderManager_3;
            isBoughtOrderStaffPack = dataSave.isBoughtOrderStaffPack;
        }
        LoadOrderMax();
    }
    public void Update() {
        GetOrderBookManager(ProfileManager.PlayerData.selectedWorld).Update();
    }
    public void SaveData() {
        if (!isChangeData) return;
        isChangeData = false;
        PlayerPrefs.SetString("OrderSave", JsonUtility.ToJson(this).ToString());
    }
    string GetJsonData() {
        return PlayerPrefs.GetString("OrderSave");
    }
    public OrderBookManager GetOrderBookManager(int world) {
        switch (world) {
            case 1:
                return orderManager_1;
            case 2:
                return orderManager_2;
            case 3:
                return orderManager_3;
        }
        return orderManager_1;
    }
    void LoadOrderMax() {
        if (isBoughtOrderStaffPack) maxOrder = 3;
        else maxOrder = 2;
    }
    public void OnBoughtOrderStaffPack() {
        isBoughtOrderStaffPack = true;
        LoadOrderMax();
        SaveData();
    }
}
