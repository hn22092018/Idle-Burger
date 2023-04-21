using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DeliverCakePosition {
    public Transform transform;
    public DeliverStaff staff;
}
public class DeliverCakePositionSetting : MonoBehaviour
{
    public RoomID roomID;
    IDefineRoomGroupID IDefineRoomGroupID;
    public List<DeliverCakePosition> positionsSetting;
    void Start() {
        IDefineRoomGroupID = GetComponent<IDefineRoomGroupID>();
        if (IDefineRoomGroupID == null) return;
        for (int i = 0; i < positionsSetting.Count; i++) {
            AllRoomManager.instance._DeliverManagers.AddCakePosition(positionsSetting[i]);
        }

    }
 
}
