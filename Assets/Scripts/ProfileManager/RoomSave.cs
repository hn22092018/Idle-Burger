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
}
[System.Serializable]
public class RoomSaveData {
    public List<BaseRoom<SmallTableModelType>> smallTableRoomDatas = new List<BaseRoom<SmallTableModelType>>();
    public List<BaseRoom<BigTableModelType>> bigTableRoomDatas = new List<BaseRoom<BigTableModelType>>();
    public List<BaseRoom<KitchenModelType>> kitchenRoomDatas = new List<BaseRoom<KitchenModelType>>();
    public List<BaseRoom<LobbyModelType>> lobbyRoomDatas = new List<BaseRoom<LobbyModelType>>();
    public List<BaseRoom<ManagerModelType>> managerRoomDatas = new List<BaseRoom<ManagerModelType>>();
    public List<BaseRoom<PowerModelType>> powerRoomDatas = new List<BaseRoom<PowerModelType>>();
    public List<BaseRoom<RestroomModelType>> restroomDatas = new List<BaseRoom<RestroomModelType>>();
    public List<BaseRoom<DeliverModelType>> deliverRoomDatas = new List<BaseRoom<DeliverModelType>>();
    public List<StaffConfig> staffConfigData = new List<StaffConfig>();

    const string SmallTableModelType = "SmallTableModelType";
    const string BigTableModelType = "BigTableModelType";
    const string LobbyModelType = "LobbyModelType";
    const string ManagerModelType = "ManagerModelType";
    const string KitchenModelType = "KitchenModelType";
    const string PowerModelType = "PowerModelType";
    const string RestroomModelType = "RestroomModelType";
    const string DeliverModelType = "DeliverModelType";

    public BaseRoom<T> GetRoomData<T>(RoomID roomID, int GroupID) {
        switch (roomID) {
            case RoomID.Lobby:
                return GetRoomData(lobbyRoomDatas, roomID, GroupID) as BaseRoom<T>;
            case RoomID.Table1:
            case RoomID.Table2:
            case RoomID.Table3:
            case RoomID.Table4:
            case RoomID.Table5:
            case RoomID.Table6:
                return GetRoomData(smallTableRoomDatas, roomID, GroupID) as BaseRoom<T>;
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
                return GetRoomData(bigTableRoomDatas, roomID, GroupID) as BaseRoom<T>;
            case RoomID.Kitchen:
                return GetRoomData(kitchenRoomDatas, roomID, GroupID) as BaseRoom<T>;
            case RoomID.Manager:
                return GetRoomData(managerRoomDatas, roomID, GroupID) as BaseRoom<T>;
            case RoomID.Power:
                return GetRoomData(powerRoomDatas, roomID, GroupID) as BaseRoom<T>;
            case RoomID.Restroom:
            case RoomID.Restroom2:
                return GetRoomData(restroomDatas, roomID, GroupID) as BaseRoom<T>;
            case RoomID.DeliverRoom:
                return GetRoomData(deliverRoomDatas, roomID, GroupID) as BaseRoom<T>;

        }
        return null;
    }
    public BaseRoom<T> GetRoomData<T>(List<BaseRoom<T>> list, RoomID roomID, int GroupID) {
        for (int i = 0; i < list.Count; i++) {
            if (list[i].roomID == roomID && list[i].GroupID == GroupID) {
                return list[i];
            }
        }
        return null;
    }
    public void SaveRoomData<T>(BaseRoom<T> room, bool isOverideData = true) {
        switch (typeof(T).ToString()) {
            case SmallTableModelType:
                SaveRoomData(smallTableRoomDatas, room as BaseRoom<SmallTableModelType>, isOverideData);
                break;
            case BigTableModelType:
                SaveRoomData(bigTableRoomDatas, room as BaseRoom<BigTableModelType>, isOverideData);
                break;
            case LobbyModelType:
                SaveRoomData(lobbyRoomDatas, room as BaseRoom<LobbyModelType>, isOverideData);
                break;
            case KitchenModelType:
                SaveRoomData(kitchenRoomDatas, room as BaseRoom<KitchenModelType>, isOverideData);
                break;
            case ManagerModelType:
                SaveRoomData(managerRoomDatas, room as BaseRoom<ManagerModelType>, isOverideData);
                break;
            case PowerModelType:
                SaveRoomData(powerRoomDatas, room as BaseRoom<PowerModelType>, isOverideData);
                break;
            case RestroomModelType:
                SaveRoomData(restroomDatas, room as BaseRoom<RestroomModelType>, isOverideData);
                break;
            case DeliverModelType:
                SaveRoomData(deliverRoomDatas, room as BaseRoom<DeliverModelType>, isOverideData);
                break;

        }
    }
    public void SaveRoomData<T>(List<BaseRoom<T>> list, BaseRoom<T> room, bool isOverideData = true) {
        bool isHasData = false;
        for (int i = 0; i < list.Count; i++) {
            if (list[i].roomID == room.roomID && list[i].GroupID == room.GroupID) {
                if (isOverideData) list[i] = room;
                isHasData = true;
            }
        }
        if (!isHasData) list.Add(room);
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
