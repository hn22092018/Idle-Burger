using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class ChefSinkNode {
    public Chef chef;
    public Transform transform;
}

public class ChefManager : MonoBehaviour {
    [HideInInspector] public int GroupID;
    public RoomController<KitchenModelType> roomController;
    public List<Chef> chefList = new List<Chef>();
    public List<Waiter> waiterQueue = new List<Waiter>();
    public List<ChefSinkNode> sinkNodes = new List<ChefSinkNode>();
    public List<StoveOvenKitchenConfig> stoveOvenConfigs = new List<StoveOvenKitchenConfig>();
    private void Awake() {
        GroupID = roomController.GetGroupID();
        AllRoomManager.instance._ChefManagers = this;
    }

    private void Update() {
        OrderChef();
    }

    int countChef;
    List<Transform> listStaffTrans = new List<Transform>();
    public void UpdateStaffList() {
        countChef = chefList.Count;
        listStaffTrans = roomController.staffSetting.listStaffTrans;
        while (countChef < listStaffTrans.Count) {
            chefList.Add(listStaffTrans[countChef].GetComponent<Chef>());
            countChef++;
        }
    }
    public void AddStoveOven(StoveOvenKitchenConfig stoveOven) {
        stoveOven.OnFinishWork();
        roomController.OnHireStaff();
        // find chef nearest to set chef in stove oven
        var chefNearest = chefList.OrderBy(x => Vector3.Distance(x.transform.position, stoveOven.transform.position)).FirstOrDefault();
        stoveOven.chef = chefNearest;
        stoveOvenConfigs.Add(stoveOven);
    }
    public void RemoveStoveOven(StoveOvenKitchenConfig stoveOven) {
        stoveOvenConfigs.Remove(stoveOven);
    }
    public void ChefFinishWork(Chef chef) {
        for (int i = 0; i < stoveOvenConfigs.Count; i++) {
            if (stoveOvenConfigs[i].chef == chef) {
                stoveOvenConfigs[i].OnFinishWork();
                break;
            }
        }
    }

    public void RegistryWaiter(Waiter waiter) {
        waiterQueue.Add(waiter);
    }
    /// <summary>
    /// Order Chef When has waiter call food .
    /// </summary>
    int totalBurgetNeedInOrder;
    public void OrderChef() {
        for (int i = 0; i < stoveOvenConfigs.Count; i++) {
            if (stoveOvenConfigs[i].IsReadyToWork()) {
                if (waiterQueue.Count == 0) break;
                Waiter waiter = waiterQueue[0];
                stoveOvenConfigs[i].OnStartWork(waiter);
                waiterQueue.Remove(waiter);
            }
        }
        totalBurgetNeedInOrder = ProfileManager.PlayerData.GetOrderBookManager().GetTotalBurgetNeedInOrder();
        if (totalBurgetNeedInOrder == 0) return;
        for (int i = 0; i < stoveOvenConfigs.Count; i++) {
            if (stoveOvenConfigs[i].IsReadyToWork() && totalBurgetNeedInOrder > 0) {
                stoveOvenConfigs[i].OnStartWork(null);
                totalBurgetNeedInOrder--;

            }
        }

    }
    BigNumber kitchenRoomValue;
    BigNumber tip;
    public void Payment(Chef chef) {
        kitchenRoomValue = roomController.GetTotalMoneyEarn() * GameManager.instance.GetTotalIncomeRate();
        chef.CalculateSkinBuffIncome(ref kitchenRoomValue);
        tip = GameManager.instance.tipChefRateCard * kitchenRoomValue * GameManager.instance.tipBaseRate;
        ProfileManager.PlayerData.AddTipChef(tip);
        GameManager.instance.SpawnCashFx(chef.transform);
        GameManager.instance.AddCash(kitchenRoomValue);
        //UIManager.instance.CreatUIMoneyEff(kitchenRoomValue, chef.transform);
    }

    public float GetTimeMakeFood() {
        return roomController.GetTimeServiceCurrent() * GameManager.instance.chefCookTimeRate;
    }
    public void AddSinkNode(ChefSinkNode node) {
        sinkNodes.Add(node);
    }
    public void RemoveSinkNode(ChefSinkNode node) {
        if (sinkNodes.Contains(node)) sinkNodes.Remove(node);
    }
    List<ChefSinkNode> listSinkEmpty = new List<ChefSinkNode>();
    int countSinkEmpty;
    public ChefSinkNode GetSinkNodeEmpty(Chef chef) {
        listSinkEmpty.Clear();
        for (int i = 0; i < sinkNodes.Count; i++) {
            if (sinkNodes[i].chef == null) {
                listSinkEmpty.Add(sinkNodes[i]);
            }
        }
        countSinkEmpty = listSinkEmpty.Count;
        if (countSinkEmpty == 0) return null;
        ChefSinkNode chefSinkNode = listSinkEmpty[Random.Range(0, countSinkEmpty)];
        chefSinkNode.chef = chef;
        return chefSinkNode;
    }

}
