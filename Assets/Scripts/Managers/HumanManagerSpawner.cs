using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManagerSpawner : MonoBehaviour {
    public ManagerStaffID staffID;
    public int GroupID;
    public Transform Prefab;
    public Transform[] Nodes;
    CardManagerSave cardSaveInfo;
    bool isSpawned;
    private void Awake() {
        SpawnManager();
        EventManager.AddListener(EventName.UpdateCardManager.ToString(), (x) => {
            if ((int)x == (int)(staffID) && !isSpawned) SpawnManager();
        });
    }
    void SpawnManager() {
        switch (staffID) {
            case ManagerStaffID.Deliver_1:
                if (!GameManager.instance.IsUnlockDeliverRoom()) return;
                break;
            case ManagerStaffID.Restroom_1:
                if (!GameManager.instance.IsUnlockRestroom(GameManager.instance.WCRooms[0])) return;
                break;
        }
        cardSaveInfo = ProfileManager.PlayerData.GetCardManager().GetCardManager(staffID);
        if (cardSaveInfo.level > 0) {
            isSpawned = true;
            Transform t = Instantiate(Prefab);
            t.GetComponent<StaffManager>().InitStaff(staffID, Nodes);
        }
    }

}
