using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllRoomManager : MonoBehaviour {
    public static AllRoomManager instance;
    public LobbyRoomManager _LobbyManager = null;
    public TableManager _TableManager = null;
    public List<WCManager> _RestroomManagers = new List<WCManager>();
    public ChefManager _ChefManagers = null;
    public List<WaiterManager> _WaiterManagers = new List<WaiterManager>();
    public DeliverRoomManager _DeliverManagers = null;
    private void Awake() {
        instance = this;
    }
    public void UpdateStaffList(int staffID) {
        switch (staffID) {
            case (int)StaffID.Chef:
                _ChefManagers.UpdateStaffList();
                break;
            case (int)StaffID.Waiter:
                foreach (var obj in _WaiterManagers) {
                    obj.UpdateStaffList();
                }
                break;
            case (int)StaffID.Receptionist:
                if (_LobbyManager != null) _LobbyManager.UpdateStaffList();
                break;
            case (int)StaffID.Deliver:
                _DeliverManagers.UpdateStaffList();
                break;
            case (int)StaffID.Manager:
                break;

        }

    }
    public void UpdateStaffListAll() {
        if (_LobbyManager != null) _LobbyManager.UpdateStaffList();
        _ChefManagers.UpdateStaffList();
        foreach (var obj in _WaiterManagers) {
            obj.UpdateStaffList();
        }
        _DeliverManagers.UpdateStaffList();
    }

    public void RegistryRestroomManager(WCManager manager) {
        _RestroomManagers.Add(manager);
    }
    public WCManager GetRestroomManager(RoomID roomID) {
        foreach (var manager in _RestroomManagers) {
            if (manager.currentRoom.GetRoomID() == roomID) return manager;
        }
        return null;
    }
    public void RegistryWaiterManager(WaiterManager manager) {
        _WaiterManagers.Add(manager);
    }
    public WaiterManager GetWaiterManager(int groupID) {
        foreach (var manager in _WaiterManagers) {
            if (manager.GroupID == groupID) return manager;
        }
        return null;
    }

}
