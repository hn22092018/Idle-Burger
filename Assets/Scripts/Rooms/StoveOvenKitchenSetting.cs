using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StoveOvenKitchenConfig {
    public Transform transform;
    public GameObject meat;
    public Chef chef;
    public bool IsReadyToWork() {
        if (chef.IsEnableStartWork()) {
            return true;
        }
        return false;
    }
    public void OnStartWork(Waiter waiterOrder) {
        meat.SetActive(true);
        chef.MakeFood(waiterOrder);
        chef.transform.eulerAngles = new Vector3(0, -90f, 0);
    }
    public void OnFinishWork() {
        meat.SetActive(false);
        if (chef != null) chef.transform.eulerAngles = new Vector3(0, 180f, 0);
    }
}
public class StoveOvenKitchenSetting : MonoBehaviour {
    IDefineRoomGroupID IDefineRoomGroupID;
    public StoveOvenKitchenConfig stoveOvenKitchenConfig;
    // Start is called before the first frame update
    void Start() {
        IDefineRoomGroupID = GetComponent<IDefineRoomGroupID>();
        if (IDefineRoomGroupID == null) return;
        if (AllRoomManager.instance) AllRoomManager.instance._ChefManagers.AddStoveOven(stoveOvenKitchenConfig);
    }
    public void OnDisable() {
        IDefineRoomGroupID = GetComponent<IDefineRoomGroupID>();
        if (IDefineRoomGroupID == null) return;
        if (AllRoomManager.instance) AllRoomManager.instance._ChefManagers.RemoveStoveOven(stoveOvenKitchenConfig);
    }
}
