using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterManager : MonoBehaviour {
    [HideInInspector] public int GroupID;
    public BaseStaffSetting staffSetting;
    public List<TablePosition> tablePositionsNeedWaiter = new List<TablePosition>();
    public List<Waiter> HiredWaiter = new List<Waiter>();
    int countHiredWaiter;
    ChefManager ChefManager;

    private void Awake() {
        GroupID = staffSetting.config.GroupID;
        AllRoomManager.instance.RegistryWaiterManager(this);
    }
    private void Start() {
        ChefManager = AllRoomManager.instance._ChefManagers;
    }
    private void Update() {
        CallWaiter();
    }

    List<Transform> listStaffTrans = new List<Transform>();
    public void UpdateStaffList() {
        listStaffTrans = staffSetting.listStaffTrans;
        while (countHiredWaiter < listStaffTrans.Count) {
            HiredWaiter.Add(listStaffTrans[countHiredWaiter].GetComponent<Waiter>());
            countHiredWaiter++;
        }
        countHiredWaiter = listStaffTrans.Count;
    }
    void CallWaiter() {

        if (tablePositionsNeedWaiter.Count == 0) return;
        for (int i = HiredWaiter.Count - 1; i >= 0; i--) {
            if (HiredWaiter[i].IsEnableStartWork()) {
                if (tablePositionsNeedWaiter.Count == 0) break;
                TablePosition tableToServe = tablePositionsNeedWaiter[0];
                // Call waiter to Serve
                HiredWaiter[i].OnReceiveTable(tableToServe);
                tablePositionsNeedWaiter.Remove(tableToServe);
                ChefManager.RegistryWaiter(HiredWaiter[i]);
            }
        }
    }

    public void AddTablePositionCallWaiter(TablePosition tablePosition) {
        tablePositionsNeedWaiter.Add(tablePosition);
        OnCheckWork();
    }
    BigNumber foodValue;
    BigNumber tip;
    public void PaymentFoodOrder(Waiter waiter) {
        if (waiter.servingTable.customer == null) return;
        foodValue = waiter.servingTable.customer.GetOrderFoodValue() * GameManager.instance.GetTotalIncomeRate();
        waiter.CalculateSkinBuffIncome(ref foodValue);
        tip = foodValue * GameManager.instance.tipWaiterRateCard * GameManager.instance.tipBaseRate;
        ProfileManager.PlayerData.AddTipWaiter(tip);
        GameManager.instance.AddCash(foodValue);
        UIManager.instance.CreatUIMoneyEff(foodValue, waiter.servingTable.customer.GetTransform());
    }

    public void OnCheckWork() {
        CallWaiter();
    }
}
