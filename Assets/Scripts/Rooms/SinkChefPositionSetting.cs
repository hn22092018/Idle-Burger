using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkChefPositionSetting : MonoBehaviour {
    public ChefSinkNode node;
    //IDefineRoomGroupID IDefineRoomGroupID;
    // Start is called before the first frame update
    void Start() {
        //IDefineRoomGroupID = GetComponent<IDefineRoomGroupID>();
        //if (IDefineRoomGroupID == null) return;
        if (AllRoomManager.instance) AllRoomManager.instance._ChefManagers.AddSinkNode(node);
    }
    private void OnDisable() {
        //if (IDefineRoomGroupID == null) return;
        if (AllRoomManager.instance) AllRoomManager.instance._ChefManagers.RemoveSinkNode(node);

    }
}
