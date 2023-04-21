using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WCPosition {
    public Transform transform;
    public Customer customer;
    public bool IsMale;
}
public class WCManager : MonoBehaviour {
    [HideInInspector] public int GroupID;
    public RoomController<RestroomModelType> currentRoom;
    public List<WCPosition> wcPositionsSetting;
    private void Awake() {
        GroupID = currentRoom.GetGroupID();
        AllRoomManager.instance.RegistryRestroomManager(this);
    }
    public void CloseRestaurant() {
        for (int i = 0; i < wcPositionsSetting.Count; i++) {
            if (wcPositionsSetting[i].customer) {
                Payment(wcPositionsSetting[i]);
                wcPositionsSetting[i].customer = null;
            }
        }
    }
    public void AddPosition(WCPosition pos) {
        if (!wcPositionsSetting.Contains(pos)) {
            wcPositionsSetting.Add(pos);
        }
    }
    public void RemovePosition(WCPosition pos) {
        if (wcPositionsSetting.Contains(pos)) {
            wcPositionsSetting.Remove(pos);
        }
    }
    public void OutPosition(Customer customer) {
        for (int i = 0; i < wcPositionsSetting.Count; i++) {
            if (wcPositionsSetting[i].customer == customer) {
                wcPositionsSetting[i].customer = null;
                break;
            }
        }
    }
    List<WCPosition> listEmpty = new List<WCPosition>();
    int countEmpty;
    public WCPosition GetPostionEmpty(Customer customer) {
        if (customer.OwnerGroupID != GroupID) return null;
        listEmpty = GetListPositionEmpty(customer);
        countEmpty = listEmpty.Count;
        if (countEmpty == 0) return null;
        float dis = Vector3.Distance(customer.transform.position, listEmpty[0].transform.position);
        WCPosition wcPosition = listEmpty[0];
        foreach (var empty in listEmpty) {
            if (Vector3.Distance(empty.transform.position, customer.transform.position) < dis) wcPosition = empty;
        }
        wcPosition.customer = customer;
        return wcPosition;
    }
    public WCPosition ReplaceByNearestPostionEmpty(Customer customer) {
        listEmpty = GetListPositionEmpty(customer);
        if (listEmpty.Count == 0) return null;
        int index = 0;
        float minDis = Vector3.Distance(customer.transform.position, listEmpty[0].transform.position);
        for (int i = 0; i < listEmpty.Count; i++) {
            float newDis = Vector3.Distance(customer.transform.position, listEmpty[i].transform.position);
            if (newDis < minDis) {
                minDis = newDis;
                index = i;
            }
        }
        listEmpty[index].customer = customer;
        return listEmpty[index];
    }
    List<WCPosition> GetListPositionEmpty(Customer customer) {
        listEmpty = new List<WCPosition>();
        for (int i = 0; i < wcPositionsSetting.Count; i++) {
            if (wcPositionsSetting[i].customer == null && wcPositionsSetting[i].IsMale == customer.IsMale )
                listEmpty.Add(wcPositionsSetting[i]);
        }
        return listEmpty;
    }
    BigNumber value;
    public void Payment(WCPosition pos) {
        value = currentRoom.GetTotalMoneyEarn() * GameManager.instance.GetTotalIncomeRate();
        GameManager.instance.AddCash(value);
        UIManager.instance.CreatUIMoneyEff(value, pos.transform);
    }
    public float GetTimeService() {
        return currentRoom.GetTimeServiceCurrent() * GameManager.instance.customerWCTimeRate;
    }
}
