using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LobbyPosition {
    public Transform transform;
    public Customer customer;
    public LobbyReception staff;
    public void Out() {
        customer = null;
    }
   public bool IsAcceptNewCustomer() {
        if (staff != null && staff.isFree && customer == null) {
            return true;
        }
        return false;
    }
}
public class LobbyRoomManager : MonoBehaviour {
    public static LobbyRoomManager instance;
    public RoomController<LobbyModelType> roomController;
    public List<LobbyPosition> lobbyPositions;
    public List<Transform> positionsQueue;
    public List<Customer> customersInQueue;
    float orderTime = 5;
    int countStaff;

    private void Awake() {
        instance = this;
        AllRoomManager.instance._LobbyManager = this;
    }

    List<Transform> listStaffTrans = new List<Transform>();
    public void UpdateStaffList() {
        listStaffTrans = roomController.staffSetting.listStaffTrans;
        while (countStaff < roomController.staffSetting.GetTotalStaffCurrent()) {
            lobbyPositions[countStaff].staff = listStaffTrans[countStaff].GetComponent<LobbyReception>();
            countStaff++;
        }

    }

    public void ProcessOrder(LobbyPosition lobby) {
        orderTime = (float)roomController.GetTimeServiceCurrent() * GameManager.instance. receptionOrderTimeRate;
        if (Tutorials.instance.IsRunStory) orderTime = 2;
        lobby.staff.StartOrder(orderTime);
    }
    public bool IsOrderFinish(LobbyPosition lobby) {
        return lobby.staff.IsCompletedOrder();
    }
    BigNumber orderValue;
    public void PaymentOrder(LobbyPosition lobby) {
        orderValue = roomController.GetTotalMoneyEarn() * GameManager.instance.GetTotalIncomeRate();
        lobby.staff.FinishOrder();
        lobby.staff.CalculateSkinBuffIncome(ref orderValue);
        GameManager.instance.AddCash(orderValue);
        //UIManager.instance.CreatUIMoneyEff(orderValue, lobby.staff.transform);
        ProfileManager.PlayerData.AddTipReception(orderValue * GameManager.instance.tipBaseRate * GameManager.instance.tipReceptionRate);
    }
    public bool IsHasLobbyQueueEmpty() {
        return customersInQueue.Count < positionsQueue.Count;
    }
    Transform LobbyQueuePos;
    public Transform GetLobbyQueuePos(Customer customer) {
        LobbyQueuePos = null;
        if (customersInQueue.Count >= positionsQueue.Count) return null;
        LobbyQueuePos = positionsQueue[customersInQueue.Count];
        customersInQueue.Add(customer);
        return LobbyQueuePos;
    }

    public LobbyPosition GetLobbyReceptionFreeTime(Customer customer) {
        for (int i = 0; i < lobbyPositions.Count; i++) {
            LobbyPosition lobby = lobbyPositions[i];
            if (lobby.IsAcceptNewCustomer()) {
                lobby.customer = customer;
                return lobby;
            }
        }
        return null;
    }
    public void OutLobby(Customer customer) {
        customer.LobbyTarget.Out();
    }
    public void OutLobbyQueue(Customer customer) {
        // clear customer in lobby position
        customersInQueue.Remove(customer);
        for (int i = 0; i < customersInQueue.Count; i++) {
            customersInQueue[i].customerToLobby.UpdateQueuePos(positionsQueue[i]);
        }
    }
    public float GetWaitingTimeMax() {
        return roomController.GetTimeServiceCurrent() * 2.5f * GameManager.instance.customerWaitTimeRate;
    }

}
