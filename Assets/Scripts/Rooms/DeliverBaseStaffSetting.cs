using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverBaseStaffSetting : BaseStaffSetting {
    public DeliverRoomController roomController;
   
    public override void LoadStaff() {
        listStaffTrans = new List<Transform>();
        for (int i = 0; i < config.staffPositions.Count; i++) {
            /// clear model current
            if (config.staffPositions[i].childCount > 0) {
                Destroy(config.staffPositions[i].GetChild(0).gameObject);
            }
        }
        int totalActive = config.totalStaffCurrent;
        for (int i = 0; i < config.staffPositions.Count; i++) {
            if (roomController.GetLevelItem(i) > 0 && totalActive > 0) {
                totalActive--;
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
    public override void OnHireStaff() {
        config.totalStaffCurrent++;
        config.totalStaffCurrent = Mathf.Clamp(config.totalStaffCurrent, 0, GetTotalStaff());
        for (int i = 0; i < config.staffPositions.Count; i++) {
            if (roomController.GetLevelItem(i) > 0 && config.staffPositions[i].transform.childCount <= 0) {
                Vector3 pos = config.staffPositions[i].position;
                Transform t = Instantiate(staffData.modelStaff.transform, pos, Quaternion.identity);
                t.eulerAngles = config.staffPositions[i].eulerAngles;
                t.localScale = new Vector3(1, 1, 1);
                t.parent = config.staffPositions[i].transform;
                listStaffTrans.Add(t);
                t.GetComponent<BaseStaff>()._StaffGroupID = config.GroupID;
                t.GetComponent<BaseStaff>().OnCreated(i);
                break;
            }
        }
        TriggerUpdateStaff();
    }
    public override int GetTotalStaffAvaiableHire() {
        int TotalStaff = 0;
        for (int i = 0; i < config.staffPositions.Count; i++) {
            if (roomController.GetLevelItem(i) > 0) {
                TotalStaff++;
            }
        }
        return TotalStaff;
    }
}
