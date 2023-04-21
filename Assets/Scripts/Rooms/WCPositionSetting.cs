using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WCPositionSetting : MonoBehaviour {
    public List<WCPosition> positionsSetting;
    WCManager WCManager;
    IDefineRoomGroupID IDefineRoomGroupID;
    // Start is called before the first frame update
    void Start() {
        IDefineRoomGroupID = GetComponent<IDefineRoomGroupID>();
        if (IDefineRoomGroupID == null) return;
        WCManager = AllRoomManager.instance.GetRestroomManager(IDefineRoomGroupID.OwnerRoomID);
        for (int i = 0; i < positionsSetting.Count; i++) {
            WCManager.AddPosition(positionsSetting[i]);
        }
    }
    private void OnDisable() {
        if (IDefineRoomGroupID == null) return;
        for (int i = 0; i < positionsSetting.Count; i++) {
            if (WCManager) WCManager.RemovePosition(positionsSetting[i]);
        }
    }
}
