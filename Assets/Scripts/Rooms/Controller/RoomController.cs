using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public interface IRoomController {
    public BaseStaffSetting GetStaffSetting();

    public int GetTotalModel();
    // item
    public Sprite GetSpriteItem(int itemIndex);
    public int GetLevelItem(int itemIndex);
    public int GetLevelMaxItem(int itemIndex);
    public string GetNameItem(int index);
    public string GetInfoItem(int index);
    // energy
    public int GetTotalEnergyEarn();
    public int GetEnergyEarnItem(int index);
    public int GetEnergyEarnIncreaseInNextLevel(int index);
    //public int GetTotalEnergyRequireCurrent();
    public int GetEnergyRequireItem(int index);
    public int GetEnergyRequirePreviorLevel(int index);
    public float GetTimeServiceCurrent();
    public float GetTimeReduce(int index);
    public float GetTimeReduceIncreaseInNextLevel(int index);
    // money
    public BigNumber GetTotalMoneyEarn();
    public BigNumber GetUpgradePriceItem(int itemIndex);
    public BigNumber GetUpgradePriceItemByLevel(int itemIndex, int level);
    public float GetMoneyEarnItem(int index);
    public float GetMoneyIncreaseInNextLevel(int index);
    public void OnHireStaff();
    public void OnUpgradeItem(int itemIndex);
    public RoomID GetRoomID();
    public int GetGroupID();
    public Transform GetTransform();
    public void TurnOnSelectedEffectItem(int id);
    public void TurnOffSelectedEffectItem();
    public int GetFirstItemIndexByQuest(Quest quest);
    public float GetProcessUpgrade();
    public Vector3 GetPositionItem(int index);
    public int GetTotalUpgradePoint();
    public void OnLockRoom();
    public ManagerStaffID GetManagerStaffID();
    public BigNumber GetMinUpgradeValue();
    public int GetIndexHasMinUpgradeValue();

}
public class RoomController<T> : MonoBehaviour, IRoomController {
    public ManagerStaffID managerStaffID;
    public BaseRoom<T> roomSetting;
    public BaseStaffSetting staffSetting;
    [InlineEditor]
    public RoomDataAsset<T> roomDataAsset;
    [BoxGroup("View Room")]
    public GameObject _ObjModelRoom;
    [BoxGroup("View Room")]
    public GameObject _ObjBuildRoom;
    public float RoomPowerRate = 1f;
    public BigNumber totalMoneyEarn;
    float timeService;
    //public int totalEnergyUsed;
    public int totalEnergyEarn;
    public bool IsLoadStaffFromSave = true;
    int selectedItemIndex;
    BigNumber minUpgradeValue = new BigNumber(0);
    int indexItemHasMinUpgrade;
    public List<GameObject> _NextRoomsWhenUnlock;
    public void OnApplyTargetBuildRoom() {
        if (_ObjBuildRoom) _ObjBuildRoom.GetComponent<UIBuildTarget>().target = roomSetting.roomID;
        if (_ObjBuildRoom) _ObjBuildRoom.GetComponent<UIBuildTarget>().GroupID = roomSetting.GroupID;
    }
    public void OnLockRoom() {
        if (GetComponent<Collider>()) GetComponent<Collider>().enabled = false;
        if (_ObjBuildRoom) _ObjBuildRoom.gameObject.SetActive(true);
        if (_ObjModelRoom) _ObjModelRoom.gameObject.SetActive(false);
    }
    public void OnHideRoom() {
        if (GetComponent<Collider>()) GetComponent<Collider>().enabled = false;
        if (_ObjBuildRoom) _ObjBuildRoom.gameObject.SetActive(false);
        if (_ObjModelRoom) _ObjModelRoom.gameObject.SetActive(false);
    }
    public void OnLoadRoom() {
        if (GetComponent<Collider>()) GetComponent<Collider>().enabled = true;
        if (_ObjBuildRoom) _ObjBuildRoom.gameObject.SetActive(false);
        if (_ObjModelRoom) _ObjModelRoom.gameObject.SetActive(true);
        LoadFromSaveData(ProfileManager.Instance.playerData.GetRoomData(roomSetting));
        totalMoneyEarn = GetTotalMoney();
        timeService = GetTimeService();
        //totalEnergyUsed = GetTotalEnergyUsed();
        totalEnergyEarn = GetTotalEnergyEarn();
        if (staffSetting && IsLoadStaffFromSave) staffSetting.LoadStaffFromSave();
        if (GetComponent<IOnLoadRoomCallback>() != null) GetComponent<IOnLoadRoomCallback>().OLoadRoomCallback();
    }

    void LoadFromSaveData(BaseRoom<T> saveRoom) {
        try {
            for (int i = 0; i < roomSetting.modelPositions.Count; i++) {
                ModelPosition<T> model = roomSetting.modelPositions[i];
                /// clear model current
                if (model.RootObject.childCount > 0) {
                    Destroy(model.RootObject.GetChild(0).gameObject);
                }

                if (saveRoom != null && saveRoom.modelPositions.Count > i) {
                    model.level = saveRoom.modelPositions[i].level;
                }
                if (model.IsInitItem && model.level == 0) model.level = 1;
                if (model.level > 0) {
                    Transform t = Instantiate(roomDataAsset.GetModelByType(model.type.ToString(), model.level));
                    t.SetParent(model.RootObject);
                    IDefineRoomGroupID defineRoomGroupID = t.gameObject.AddComponent<IDefineRoomGroupID>();
                    defineRoomGroupID.OwnerGroupID = roomSetting.GroupID;
                    defineRoomGroupID.OwnerRoomID = roomSetting.roomID;
                    t.localPosition = Vector3.zero;
                    t.localEulerAngles = Vector3.zero;
                    t.localScale = new Vector3(1, 1, 1);
                    model.currentModel = t;

                }
            }
        } catch (Exception e) {
            Debug.Log(this.gameObject.name + "_Load Error_" + e.Message);
        }
    }

    /// <summary>
    /// upgrade model & save room data.
    /// </summary>
    /// <param name="index"></param>
    public virtual void OnUpgradeItem(int index) {
        ModelPosition<T> model = roomSetting.modelPositions[index];
        model.level++;
        model.level = Mathf.Clamp(model.level, 0, GetLevelMaxItem(index));
        /// clear model current
        if (IsReplaceModel(model.level)) {
            if (model.RootObject.childCount > 0) {
                foreach (Transform child in model.RootObject) {
                    Destroy(child.gameObject);
                }
            }
            if (model.level > 0) {
                Transform t = Instantiate(roomDataAsset.GetModelByType(model.type.ToString(), model.level));
                t.SetParent(model.RootObject);
                IDefineRoomGroupID defineRoomGroupID = t.gameObject.AddComponent<IDefineRoomGroupID>();
                defineRoomGroupID.OwnerGroupID = roomSetting.GroupID;
                defineRoomGroupID.OwnerRoomID = roomSetting.roomID;
                t.localPosition = Vector3.zero;
                t.localEulerAngles = Vector3.zero;
                t.localScale = new Vector3(1, 1, 1);
                model.currentModel = t;
            }
        }

        totalMoneyEarn = GetTotalMoneyAfterUpgradeItem(index);
        timeService = GetTimeService();
        //totalEnergyUsed = GetTotalEnergyUsed();
        totalEnergyEarn = GetTotalEnergyEarn();
        TriggerQuestUpgrade(index);
        GameManager.instance.LoadMapUpgradeProcess();
        ProfileManager.Instance.playerData.SaveRoomData(roomSetting);

    }
    bool IsReplaceModel(int level) {
        if (level == 1 || level == 26 || level == 51) return true;
        return false;
    }
    public void OnHireStaff() {
        if (staffSetting == null) return;
        staffSetting.OnHireStaff();
        ProfileManager.Instance.playerData.SaveStaffData(staffSetting.config);
    }
    /// <summary>
    ///  Get Sprite Show In UI of model
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public Sprite GetSpriteItem(int itemIndex) {
        T type = roomSetting.modelPositions[itemIndex].type;
        return roomDataAsset.GetpriteModelByType(type.ToString());
    }
    /// <summary>
    /// Get Current Level
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public int GetLevelItem(int itemIndex) {
        return roomSetting.modelPositions[itemIndex].level;
    }
    /// <summary>
    /// Get Level Highest Enable Upgrade
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public int GetLevelMaxItem(int itemIndex) {
        T type = roomSetting.modelPositions[itemIndex].type;
        return roomDataAsset.GetLevelMaxByType(type.ToString());
    }
    /// <summary>
    /// Get Model Name by type
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public string GetNameItem(int itemIndex) {
        T type = roomSetting.modelPositions[itemIndex].type;
        return roomDataAsset.GetNameByType(type.ToString());
    }
    /// Get Info Model by model type
    public string GetInfoItem(int itemIndex) {
        T type = roomSetting.modelPositions[itemIndex].type;
        return roomDataAsset.GetInfoByType(type.ToString());
    }
    /// Get Money Need To Upgrade by model type & model level
    public float GetMoneyEarnItem(int itemIndex) {
        T type = roomSetting.modelPositions[itemIndex].type;
        int level = roomSetting.modelPositions[itemIndex].level;
        return roomDataAsset.GetMoneyEarn(type.ToString(), level) * RoomPowerRate;
    }
    public float GetMoneyIncreaseInNextLevel(int itemIndex) {
        T type = roomSetting.modelPositions[itemIndex].type;
        int level = roomSetting.modelPositions[itemIndex].level;
        return RoomPowerRate * (roomDataAsset.GetMoneyEarn(type.ToString(), level + 1) - (roomDataAsset.GetMoneyEarn(type.ToString(), level)));
    }
    BigNumber GetTotalMoney() {
        BigNumber total = roomDataAsset.profitBase;
        for (int i = 0; i < roomSetting.modelPositions.Count; i++) {
            T type = roomSetting.modelPositions[i].type;
            int level = roomSetting.modelPositions[i].level;
            for (int k = 1; k <= level; k++) {
                total += total * roomDataAsset.GetProfitIncreaseRateByType(type.ToString()) / 100;
            }
            //total += GetMoneyEarnItem(i);
        }
        return total;
    }
    BigNumber GetTotalMoneyAfterUpgradeItem(int index) {
        T type = roomSetting.modelPositions[index].type;
        totalMoneyEarn += totalMoneyEarn * (roomDataAsset.GetProfitIncreaseRateByType(type.ToString()) / 100f);
        return totalMoneyEarn;
    }
    public float GetTimeService() {
        float time = roomDataAsset.timeService;
        for (int i = 0; i < roomSetting.modelPositions.Count; i++) {
            time -= GetTimeReduce(i);
        }
        return (float)Math.Round(time, 2);
    }
    public float GetTimeReduceIncreaseInNextLevel(int itemIndex) {
        T type = roomSetting.modelPositions[itemIndex].type;
        int level = roomSetting.modelPositions[itemIndex].level;
        float time = roomDataAsset.GetTimeReduce(type.ToString(), level + 1) - roomDataAsset.GetTimeReduce(type.ToString(), level);
        return (float)Math.Round(time, 2);
    }
    public float GetTimeReduce(int itemIndex) {
        T type = roomSetting.modelPositions[itemIndex].type;
        int level = roomSetting.modelPositions[itemIndex].level;
        float time = roomDataAsset.GetTimeReduce(type.ToString(), level);
        return (float)Math.Round(time, 2);
    }
    public int GetTotalEnergyUsed() {
        int total = 0;
        //for (int i = 0; i < roomSetting.modelPositions.Count; i++) {
        //    T type = roomSetting.modelPositions[i].type;
        //    int level = roomSetting.modelPositions[i].level;
        //    if (!roomSetting.modelPositions[i].IsInitItem && level > 0)
        //        total += roomDataAsset.GetEnergyRequire(type.ToString(), level - 1);
        //    else if (level > 1) {
        //        total += roomDataAsset.GetEnergyRequire(type.ToString(), level - 1);
        //        total -= roomDataAsset.GetEnergyRequire(type.ToString(), 0);
        //    }
        //}
        return total;
    }

    public int GetTotalEnergyEarn() {
        int total = 0;
        for (int i = 0; i < roomSetting.modelPositions.Count; i++) {
            total += GetEnergyEarnItem(i);
        }
        return total;
    }
    /// <summary>
    /// Get Energy Need To Upgrade by model type & model level
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public int GetEnergyRequireItem(int itemIndex) {
        T type = roomSetting.modelPositions[itemIndex].type;
        int level = roomSetting.modelPositions[itemIndex].level;
        return roomDataAsset.GetEnergyRequire(type.ToString(), level);
    }
    public int GetEnergyRequirePreviorLevel(int itemIndex) {
        T type = roomSetting.modelPositions[itemIndex].type;
        int level = roomSetting.modelPositions[itemIndex].level;
        return roomDataAsset.GetEnergyRequire(type.ToString(), level - 1);
    }

    public int GetEnergyEarnItem(int itemIndex) {
        T type = roomSetting.modelPositions[itemIndex].type;
        int level = roomSetting.modelPositions[itemIndex].level;
        return roomDataAsset.GetEnergyEarn(type.ToString(), level);
    }
    public int GetEnergyEarnIncreaseInNextLevel(int itemIndex) {
        T type = roomSetting.modelPositions[itemIndex].type;
        int level = roomSetting.modelPositions[itemIndex].level;
        return roomDataAsset.GetEnergyEarn(type.ToString(), level + 1) - roomDataAsset.GetEnergyEarn(type.ToString(), level);
    }
    /// <summary>
    /// Get Upgrade Price by model type & model level
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public BigNumber GetUpgradePriceItem(int itemIndex) {
        T type = roomSetting.modelPositions[itemIndex].type;
        int level = roomSetting.modelPositions[itemIndex].level;
        BigNumber price = roomDataAsset.GetUpgradePrice(type.ToString(), level) * RoomPowerRate;
        return price;
    }
    public BigNumber GetUpgradePriceItemByLevel(int itemIndex, int level) {
        T type = roomSetting.modelPositions[itemIndex].type;
        BigNumber price = roomDataAsset.GetUpgradePrice(type.ToString(), level) * RoomPowerRate;
        return price;
    }
    /// <summary>
    /// Get Total Model In Room
    /// </summary>
    /// <returns></returns>
    public int GetTotalModel() {
        return roomSetting.modelPositions.Count;
    }
    public BaseStaffSetting GetStaffSetting() {
        return staffSetting;
    }

    //public int GetTotalEnergyRequireCurrent() {
    //    return totalEnergyUsed;
    //}

    public float GetTimeServiceCurrent() {
        return timeService;
    }

    public BigNumber GetTotalMoneyEarn() {
        return totalMoneyEarn;
    }

    public RoomID GetRoomID() {
        return roomSetting.roomID;
    }
    public int GetGroupID() {
        return roomSetting.GroupID;
    }
    public Transform GetTransform() {
        return transform;
    }

    public void TurnOnSelectedEffectItem(int id) {
        TurnOffSelectedEffectItem();
        selectedItemIndex = id;
        Transform selectedModel = roomSetting.modelPositions[selectedItemIndex].currentModel;
        if (selectedModel == null) {
            // if level=0 , cache spawn to view demo
            selectedModel = Instantiate(roomDataAsset.GetModelByType(roomSetting.modelPositions[selectedItemIndex].type.ToString(), 1));
            selectedModel.SetParent(roomSetting.modelPositions[selectedItemIndex].RootObject);
            selectedModel.localPosition = Vector3.zero;
            selectedModel.localEulerAngles = Vector3.zero;
            selectedModel.localScale = new Vector3(1, 1, 1);
            roomSetting.modelPositions[selectedItemIndex].currentModel = selectedModel;
        };
        FlashModel[] flashes = selectedModel.GetComponents<FlashModel>();
        for (int i = 0; i < flashes.Length; i++) {
            if (flashes[i].isActiveAndEnabled) flashes[i].TurnOnFlash();
        }
    }

    public void TurnOffSelectedEffectItem() {
        Transform selectedModel = roomSetting.modelPositions[selectedItemIndex].currentModel;
        if (selectedModel == null) return;
        int level = roomSetting.modelPositions[selectedItemIndex].level;
        if (level == 0) {
            // if has cache spawn, clear object
            Destroy(selectedModel.gameObject);
            return;
        }
        FlashModel[] flashes = selectedModel.GetComponents<FlashModel>();
        for (int i = 0; i < flashes.Length; i++) {
            flashes[i].TurnOffFlash();
        }
    }
    public virtual void TriggerQuestWhenUnlock() {
        for (int i = 0; i < roomSetting.modelPositions.Count; i++) {
            int level = roomSetting.modelPositions[i].level;
            if (level > 0) {
                TriggerQuestUpgrade(i);
            }
        }
    }
    public virtual void TriggerQuestUpgrade(int indexItem) {

    }
    public virtual int GetFirstItemIndexByQuest(Quest quest) {
        return 0;
    }
    public int GetFirstItemIndexByType(string type) {
        for (int i = 0; i < roomSetting.modelPositions.Count; i++) {
            if (roomSetting.modelPositions[i].type.ToString() == type) return i;
        }
        return 0;
    }
    public int GetTotalModelOwnerByType(string type) {
        int total = 0;
        for (int i = 0; i < roomSetting.modelPositions.Count; i++) {
            ModelPosition<T> modelPosition = roomSetting.modelPositions[i];
            if (modelPosition.type.ToString() == type && modelPosition.level > 0) total++;
        }
        return total;
    }

    public virtual Vector3 GetPositionItem(int index) {
        Vector3 pos = roomSetting.modelPositions[index].RootObject.transform.position;
        return new Vector3(pos.x, 0, pos.z);
    }
    public virtual float GetProcessUpgrade() {
        float max = 0;
        float current = 0;
        for (int i = 0; i < roomSetting.modelPositions.Count; i++) {
            max += GetLevelMaxItem(i);
            current += GetLevelItem(i);
        }
        return current / max;
    }
    public int GetTotalUpgradePoint() {
        int total = 0;
        for (int i = 0; i < roomSetting.modelPositions.Count; i++) {
            int level = roomSetting.modelPositions[i].level;
            for (int lv = 1; lv <= level; lv++) {
                total += 1;
            }
        }
        return total;
    }
    public ManagerStaffID GetManagerStaffID() {
        return managerStaffID;
    }
    public void CalculateMinUpgrade() {
        if (roomSetting.modelPositions.Count == 0) return;
        indexItemHasMinUpgrade = 0;
        minUpgradeValue = 0;
        for (int i = 0; i < roomSetting.modelPositions.Count; i++) {
            int level = roomSetting.modelPositions[i].level;
            if (level >= GetLevelMaxItem(i)) continue;
            indexItemHasMinUpgrade = i;
            minUpgradeValue = GetUpgradePriceItem(i);
            break;
        }
        for (int i = indexItemHasMinUpgrade; i < roomSetting.modelPositions.Count; i++) {
            int level = roomSetting.modelPositions[i].level;
            if (level >= GetLevelMaxItem(i)) continue;
            BigNumber price = GetUpgradePriceItem(i);
            if (price <= minUpgradeValue) {
                indexItemHasMinUpgrade = i;
                minUpgradeValue = price;
            }
        }
    }

    public BigNumber GetMinUpgradeValue() {
        return minUpgradeValue;
    }
    public int GetIndexHasMinUpgradeValue() {
        return indexItemHasMinUpgrade;
    }
    [Button]
    public void LoadStaffEditor() {
        foreach (var obj in staffSetting.config.staffPositions) {
            if (obj.childCount > 0) {
                Destroy(obj.GetChild(0).gameObject);
            }
            Transform t = Instantiate(staffSetting.staffData.modelStaff.transform, obj.transform);
            t.position = obj.transform.position;
            t.eulerAngles = obj.transform.eulerAngles;
        }
    }
    [Button]
    public void ClearStaffEditor() {
        foreach (var obj in staffSetting.config.staffPositions) {
            if (obj.childCount > 0) {
                DestroyImmediate(obj.GetChild(0).gameObject);
            }
        }
    }
}
