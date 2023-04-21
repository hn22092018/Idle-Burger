using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[System.Serializable]
public class TablePosition {
    public Transform chairTransform;
    public ICustomer customer;
    public GameObject foodOnTable;
    public Transform outTransform;
    public Transform serverTransform;
    [HideInInspector]
    public RoomID OwnerRoomID;
    [HideInInspector]
    public int OwnerGroupID;
    public Dictionary<int, GameObject> foodDic = new Dictionary<int, GameObject>();
    GameObject currentFood = null;
    int currentFoodID;

    public int CurrentFoodID { get => currentFoodID; set => currentFoodID = value; }

    public void SetFoodID(int id) {
         currentFoodID= id;
    }
    public void OnUseFood() {
        if (customer != null) customer.OnStartUseFood();
        currentFood = null;
        if (!foodDic.ContainsKey(currentFoodID)) {
            currentFood = TableManager.instance.CreatFood(currentFoodID, foodOnTable.transform);
            foodDic.Add(currentFoodID, currentFood);
            currentFood.gameObject.SetActive(true);
            currentFood.transform.localPosition = Vector3.zero;
        } else {
            currentFood = foodDic[currentFoodID];
            currentFood.SetActive(true);
        }
    }
    public void OnStopUseFood() {
        customer = null;
        if (currentFood) currentFood.SetActive(false);
    }

}
public class Table : MonoBehaviour, IOnLoadRoomCallback {
    public List<TablePosition> tablePositions;
    public IRoomController roomController;
    public async void OLoadRoomCallback() {
        roomController = GetComponent<IRoomController>();
        foreach (var tablePosition in tablePositions) {
            tablePosition.OnStopUseFood();
            tablePosition.OwnerRoomID = roomController.GetRoomID();
            tablePosition.OwnerGroupID = roomController.GetGroupID();
        }
        while (!TableManager.instance) {
            await Task.Run(() => Task.Delay(1));
        }
        TableManager.instance.AddTable(this);
    }

    public void CloseRestaurant() {
        for (int i = 0; i < tablePositions.Count; i++) {
            if (tablePositions[i].customer != null) {
                TableManager.instance.PaymentTable(tablePositions[i]);
                tablePositions[i].OnStopUseFood();
            }
        }
    }

    public bool IsContainerTablePosition(TablePosition tablePosition) {
        for (int i = 0; i < tablePositions.Count; i++) {
            if (tablePositions[i] == tablePosition) {
                return true;
            }
        }
        return false;
    }

    public BigNumber GetMoneyEarnInTable() {
        return roomController.GetTotalMoneyEarn();
    }

    public bool HasCustomer() {
        foreach (TablePosition tb in tablePositions) {
            if (tb.customer != null) {
                if (tb.customer.GetIsOnTable()) {
                    return true;
                }
            }
        }
        return false;
    }
}
