using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DeliverPosition {
    public Transform transform;
    public DeliverCarCustomer customer;
    public DeliverStaff staff;
    public int IndexQueue;

}
public class DeliverPositionSetting : MonoBehaviour {
    IDefineRoomGroupID IDefineRoomGroupID;
    public List<DeliverPosition> positionsSetting;
    // Start is called before the first frame update
    void Start() {
        IDefineRoomGroupID = GetComponent<IDefineRoomGroupID>();
        if (IDefineRoomGroupID == null) return;
        for (int i = 0; i < positionsSetting.Count; i++) {
            AllRoomManager.instance._DeliverManagers.AddPosition(positionsSetting[i]);
        }

    }
    //private void OnDisable() {
    //    if (IDefineRoomGroupID == null) return;
    //    for (int i = 0; i < positionsSetting.Count; i++) {
    //        AllRoomManager.instance._DeliverManagers.RemovePosition(positionsSetting[i]);
    //    }
    //}
}
