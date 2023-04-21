using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomSave {
    [Header("=======Room Data========")]
    public RoomSaveData roomManager_a = new RoomSaveData();
    public void LoadData(int world) {
        string jsonData = GetJsonData(world);
        if (!string.IsNullOrEmpty(jsonData)) {
            RoomSave dataSave = JsonUtility.FromJson<RoomSave>(jsonData);
            roomManager_a = dataSave.roomManager_a;
        }
    }
    bool IsChangeData;
    public void IsMarkChangeData() {
        IsChangeData = true;
    }
    public void SaveData(int world) {
        if (!IsChangeData) return;
        IsChangeData = false;
        PlayerPrefs.SetString("RoomSave_" + world, JsonUtility.ToJson(this).ToString());
    }
    string GetJsonData(int world) {
        return PlayerPrefs.GetString("RoomSave_" + world);
    }
    public RoomSaveData GetRoomSaveData() {
        return roomManager_a;
    }
    public void SaveProcessUpgrade(int process) {
        RoomSaveData data = GetRoomSaveData();
        data.processUpgrade = process;
    }
    public int GetProcessUpgrade() {
        RoomSaveData data = GetRoomSaveData();
        return data.processUpgrade;
    }
}
[System.Serializable]
public class RoomSaveData {
    public List<BaseRoom<SmallTableModelType>> smallTableRoomDatas = new List<BaseRoom<SmallTableModelType>>();
    public List<BaseRoom<BigTableModelType>> bigTableRoomDatas = new List<BaseRoom<BigTableModelType>>();
    public List<BaseRoom<KitchenModelType>> kitchenRoomDatas = new List<BaseRoom<KitchenModelType>>();
    public List<BaseRoom<LobbyModelType>> lobbyRoomDatas = new List<BaseRoom<LobbyModelType>>();
    public List<BaseRoom<ManagerModelType>> managerRoomDatas = new List<BaseRoom<ManagerModelType>>();
    public List<BaseRoom<CleanModelType>> cleanRoomDatas = new List<BaseRoom<CleanModelType>>();
    public List<BaseRoom<PowerModelType>> powerRoomDatas = new List<BaseRoom<PowerModelType>>();
    public List<BaseRoom<RestroomModelType>> restroomDatas = new List<BaseRoom<RestroomModelType>>();
    public List<BaseRoom<DeliverModelType>> deliverRoomDatas = new List<BaseRoom<DeliverModelType>>();
    public List<BaseRoom<ConveyorModelType>> conveyorRoomDatas = new List<BaseRoom<ConveyorModelType>>();
    public List<StaffConfig> staffConfigData = new List<StaffConfig>();
    public int processUpgrade;

    const string SmallTableModelType = "SmallTableModelType";
    const string BigTableModelType = "BigTableModelType";
    const string LobbyModelType = "LobbyModelType";
    const string ManagerModelType = "ManagerModelType";
    const string KitchenModelType = "KitchenModelType";
    const string CleanModelType = "CleanModelType";
    const string PowerModelType = "PowerModelType";
    const string RestroomModelType = "RestroomModelType";
    const string DeliverModelType = "DeliverModelType";

    public BaseRoom<T> GetRoomData<T>(RoomID roomID, int GroupID) {
        switch (roomID) {
            case RoomID.Lobby:
                for (int i = 0; i < lobbyRoomDatas.Count; i++) {
                    if (lobbyRoomDatas[i].roomID == roomID && lobbyRoomDatas[i].GroupID == GroupID) {
                        return lobbyRoomDatas[i] as BaseRoom<T>;
                    }
                }
                break;
            case RoomID.Table1:
            case RoomID.Table2:
            case RoomID.Table3:
            case RoomID.Table4:
            case RoomID.Table5:
            case RoomID.Table6:
                for (int i = 0; i < smallTableRoomDatas.Count; i++) {
                    if (smallTableRoomDatas[i].roomID == roomID && smallTableRoomDatas[i].GroupID == GroupID) {
                        return smallTableRoomDatas[i] as BaseRoom<T>;
                    }
                }
                break;
            case RoomID.BigTable1:
            case RoomID.BigTable2:
            case RoomID.BigTable3:
            case RoomID.BigTable4:
            case RoomID.BigTable5:
            case RoomID.BigTable6:
            case RoomID.BigTable7:
            case RoomID.BigTable8:
            case RoomID.BigTable9:
            case RoomID.BigTable10:
            case RoomID.BigTable11:
            case RoomID.BigTable12:
            case RoomID.BigTable13:
            case RoomID.BigTable14:
                for (int i = 0; i < bigTableRoomDatas.Count; i++) {
                    if (bigTableRoomDatas[i].roomID == roomID && bigTableRoomDatas[i].GroupID == GroupID) {
                        return bigTableRoomDatas[i] as BaseRoom<T>;
                    }
                }
                break;
            case RoomID.Kitchen:
                for (int i = 0; i < kitchenRoomDatas.Count; i++) {
                    if (kitchenRoomDatas[i].roomID == roomID && kitchenRoomDatas[i].GroupID == GroupID) {
                        return kitchenRoomDatas[i] as BaseRoom<T>;
                    }
                }
                break;
            case RoomID.Manager:
                for (int i = 0; i < managerRoomDatas.Count; i++) {
                    if (managerRoomDatas[i].roomID == roomID && managerRoomDatas[i].GroupID == GroupID) {
                        return managerRoomDatas[i] as BaseRoom<T>;
                    }
                }
                break;
            case RoomID.Clean:
                for (int i = 0; i < cleanRoomDatas.Count; i++) {
                    if (cleanRoomDatas[i].roomID == roomID && cleanRoomDatas[i].GroupID == GroupID) {
                        return cleanRoomDatas[i] as BaseRoom<T>;
                    }
                }
                break;
            case RoomID.Power:
                for (int i = 0; i < powerRoomDatas.Count; i++) {
                    if (powerRoomDatas[i].roomID == roomID && powerRoomDatas[i].GroupID == GroupID) {
                        return powerRoomDatas[i] as BaseRoom<T>;
                    }
                }
                break;
            case RoomID.Restroom:
            case RoomID.Restroom2:
                for (int i = 0; i < restroomDatas.Count; i++) {
                    if (restroomDatas[i].roomID == roomID && restroomDatas[i].GroupID == GroupID) {
                        return restroomDatas[i] as BaseRoom<T>;
                    }
                }
                break;
           
            case RoomID.DeliverRoom:
                for (int i = 0; i < deliverRoomDatas.Count; i++) {
                    if (deliverRoomDatas[i].roomID == roomID && deliverRoomDatas[i].GroupID == GroupID) {
                        return deliverRoomDatas[i] as BaseRoom<T>;
                    }
                }
                break;
          
        }
        return null;
    }
    public void SaveRoomData<T>(BaseRoom<T> room, bool isOverideData = true) {
        bool IsHasData = false;
        switch (typeof(T).ToString()) {
            case SmallTableModelType:
                for (int i = 0; i < smallTableRoomDatas.Count; i++) {
                    if (smallTableRoomDatas[i].roomID == room.roomID && smallTableRoomDatas[i].GroupID == room.GroupID) {
                        if (isOverideData) smallTableRoomDatas[i] = room as BaseRoom<SmallTableModelType>;
                        IsHasData = true;
                    }
                }
                if (!IsHasData) smallTableRoomDatas.Add(room as BaseRoom<SmallTableModelType>);
                break;
            case BigTableModelType:
                for (int i = 0; i < bigTableRoomDatas.Count; i++) {
                    if (bigTableRoomDatas[i].roomID == room.roomID && bigTableRoomDatas[i].GroupID == room.GroupID) {
                        if (isOverideData) bigTableRoomDatas[i] = room as BaseRoom<BigTableModelType>;
                        IsHasData = true;
                    }
                }
                if (!IsHasData) bigTableRoomDatas.Add(room as BaseRoom<BigTableModelType>);
                break;
            case LobbyModelType:
                for (int i = 0; i < lobbyRoomDatas.Count; i++) {
                    if (lobbyRoomDatas[i].roomID == room.roomID && lobbyRoomDatas[i].GroupID == room.GroupID) {
                        if (isOverideData) lobbyRoomDatas[i] = room as BaseRoom<LobbyModelType>;
                        IsHasData = true;
                    }
                }
                if (!IsHasData) lobbyRoomDatas.Add(room as BaseRoom<LobbyModelType>);
                break;

            case KitchenModelType:
                for (int i = 0; i < kitchenRoomDatas.Count; i++) {
                    if (kitchenRoomDatas[i].roomID == room.roomID && kitchenRoomDatas[i].GroupID == room.GroupID) {
                        if (isOverideData) kitchenRoomDatas[i] = room as BaseRoom<KitchenModelType>;
                        IsHasData = true;
                    }
                }
                if (!IsHasData) kitchenRoomDatas.Add(room as BaseRoom<KitchenModelType>);
                break;
            case ManagerModelType:

                for (int i = 0; i < managerRoomDatas.Count; i++) {
                    if (managerRoomDatas[i].roomID == room.roomID && managerRoomDatas[i].GroupID == room.GroupID) {
                        if (isOverideData) managerRoomDatas[i] = room as BaseRoom<ManagerModelType>;
                        IsHasData = true;
                    }
                }
                if (!IsHasData) managerRoomDatas.Add(room as BaseRoom<ManagerModelType>);
                break;
            case CleanModelType:
                for (int i = 0; i < cleanRoomDatas.Count; i++) {
                    if (cleanRoomDatas[i].roomID == room.roomID && cleanRoomDatas[i].GroupID == room.GroupID) {
                        if (isOverideData) cleanRoomDatas[i] = room as BaseRoom<CleanModelType>;
                        IsHasData = true;
                    }
                }
                if (!IsHasData) cleanRoomDatas.Add(room as BaseRoom<CleanModelType>);
                break;
            case PowerModelType:
                for (int i = 0; i < powerRoomDatas.Count; i++) {
                    if (powerRoomDatas[i].roomID == room.roomID && powerRoomDatas[i].GroupID == room.GroupID) {
                        if (isOverideData) powerRoomDatas[i] = room as BaseRoom<PowerModelType>;
                        IsHasData = true;
                    }
                }
                if (!IsHasData) powerRoomDatas.Add(room as BaseRoom<PowerModelType>);
                break;
            case RestroomModelType:
                for (int i = 0; i < restroomDatas.Count; i++) {
                    if (restroomDatas[i].roomID == room.roomID && restroomDatas[i].GroupID == room.GroupID) {
                        if (isOverideData) restroomDatas[i] = room as BaseRoom<RestroomModelType>;
                        IsHasData = true;
                    }
                }
                if (!IsHasData) restroomDatas.Add(room as BaseRoom<RestroomModelType>);
                break;
            
            case DeliverModelType:
                for (int i = 0; i < deliverRoomDatas.Count; i++) {
                    if (deliverRoomDatas[i].roomID == room.roomID && deliverRoomDatas[i].GroupID == room.GroupID) {
                        if (isOverideData) deliverRoomDatas[i] = room as BaseRoom<DeliverModelType>;
                        IsHasData = true;
                    }
                }
                if (!IsHasData) deliverRoomDatas.Add(room as BaseRoom<DeliverModelType>);
                break;
           
        }
    }
    public StaffConfig GetStaffConfigData(StaffConfig staffConfig) {
        for (int i = 0; i < staffConfigData.Count; i++) {
            if (staffConfigData[i].staffID == staffConfig.staffID && staffConfigData[i].GroupID == staffConfig.GroupID) {
                return staffConfigData[i];
            }
        }
        return null;
    }
    public void SaveStaffData(StaffConfig staffConfig) {
        bool IsHasData = false;
        for (int i = 0; i < staffConfigData.Count; i++) {
            if (staffConfigData[i].staffID == staffConfig.staffID && staffConfigData[i].GroupID == staffConfig.GroupID) {
                IsHasData = true;
                staffConfigData[i] = staffConfig;
            }
        }
        if (!IsHasData) staffConfigData.Add(staffConfig);
    }

}
