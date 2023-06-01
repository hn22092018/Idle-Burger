using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StaffID {
    Chef = 0,
    Waiter = 1,
    Receptionist = 2,
    Deliver = 3,
    Manager = 4,
}
[Serializable]
public class StaffConfig {
    /// <summary>
    /// Room ID
    /// </summary>
    public StaffID staffID;
    /// <summary>
    /// group ID
    /// </summary>
    public int GroupID;
    /// <summary>
    /// staff position in room
    /// </summary>
    public List<Transform> staffPositions;
    /// <summary>
    ///  Total Staff Hired
    /// </summary>
    public int totalStaffCurrent;
}

[Serializable]
public class BaseStaffSetting : MonoBehaviour {
    public StaffConfig config;
    public List<Transform> listStaffTrans = new List<Transform>();
    public BaseStaffData staffData;

    public void LoadStaffFromSave() {
        StaffConfig saveConfig = ProfileManager.Instance.playerData.GetStaffConfigData(config);
        if (saveConfig != null) {
            config.totalStaffCurrent = saveConfig.totalStaffCurrent;
        } else {
            ProfileManager.Instance.playerData.SaveStaffData(config);
        }
        LoadStaff();
    }
    public virtual void LoadStaff() {
        listStaffTrans = new List<Transform>();
        for (int i = 0; i < config.staffPositions.Count; i++) {
            /// clear model current
            if (config.staffPositions[i].childCount > 0) {
                Destroy(config.staffPositions[i].GetChild(0).gameObject);
            }
            if (i < config.totalStaffCurrent) {
                Transform t = Instantiate(staffData.modelStaff.transform, config.staffPositions[i].transform);
                t.position = config.staffPositions[i].position;
                t.eulerAngles = config.staffPositions[i].eulerAngles;
                //t.localScale = new Vector3(1, 1, 1);
                listStaffTrans.Add(t);
                t.GetComponent<BaseStaff>()._StaffGroupID = config.GroupID;
                t.GetComponent<BaseStaff>().OnCreated(i);
            }
        }
    }

    public virtual void OnHireStaff() {
        config.totalStaffCurrent++;
        config.totalStaffCurrent = Mathf.Clamp(config.totalStaffCurrent, 0, GetTotalStaff());
        int totalStaffCurrent = config.totalStaffCurrent;
        if (listStaffTrans.Count < totalStaffCurrent && config.staffPositions[totalStaffCurrent - 1].transform.childCount <= 0) {
            Vector3 pos = config.staffPositions[totalStaffCurrent - 1].position;
            Transform t = Instantiate(staffData.modelStaff.transform, pos, Quaternion.identity);
            t.eulerAngles = config.staffPositions[totalStaffCurrent - 1].eulerAngles;
            t.localScale = new Vector3(1, 1, 1);
            t.parent = config.staffPositions[totalStaffCurrent - 1].transform;
            listStaffTrans.Add(t);
            t.GetComponent<BaseStaff>()._StaffGroupID = config.GroupID;
            t.GetComponent<BaseStaff>().OnCreated(totalStaffCurrent-1);
        }
        TriggerUpdateStaff();

    }
    public virtual int GetTotalStaff() {
        return staffData.TotalStaff;
    }
    public virtual int GetTotalStaffAvaiableHire() {
        return staffData.TotalStaff;
    }
    public virtual int GetTotalStaffCurrent() {
        config.totalStaffCurrent = Mathf.Clamp(config.totalStaffCurrent, 0, GetTotalStaff());
        return config.totalStaffCurrent;
    }

    public void TriggerUpdateStaff() {
        EventManager.TriggerEvent(EventName.UpdateStaff.ToString(), (int)config.staffID);
    }
    [Button]
    public void LoadStaffEditor() {
        foreach (var obj in config.staffPositions) {
            if (obj.childCount > 0) {
                Destroy(obj.GetChild(0).gameObject);
            }
            Transform t = Instantiate(staffData.modelStaff.transform, obj.transform);
            t.position = obj.transform.position;
            t.eulerAngles = obj.transform.eulerAngles;
        }
    }
    [Button]
    public void ClearStaffEditor() {
        foreach (var obj in config.staffPositions) {
            if (obj.childCount > 0) {
                DestroyImmediate(obj.GetChild(0).gameObject);
            }
        }
    }
}
