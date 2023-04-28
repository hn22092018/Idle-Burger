using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class TableManager : MonoBehaviour {
    public static TableManager instance;
    public List<Table> tables = new List<Table>();
    GameManager GameManager;
    float managerRateIcome, managerRateProcessing;
    // Start is called before the first frame update
    void Awake() {
        instance = this;
        AllRoomManager.instance._TableManager = this;
    }

    void LoadManagerRate() {
        CardManagerSave saver = ProfileManager.PlayerData.GetCardManager().GetCardManager(GameManager.instance.SmallTablesRoom[0].GetManagerStaffID());
        managerRateIcome = ProfileManager.Instance.dataConfig.cardData.GetIncomeRateByLevel(saver.level, saver.rarity);
        managerRateProcessing = 1f - ProfileManager.Instance.dataConfig.cardData.GetIncomeRateByLevel(saver.level, saver.rarity) / 100f;
    }
    private void Start() {
        GameManager = GameManager.instance;
        LoadManagerRate();
        EventManager.AddListener(EventName.UpdateCardManager.ToString(), (x) => {
            if ((int)x == (int)ManagerStaffID.MainRoom_1) {
                LoadManagerRate();
            }
        });
    }

    public void CloseRestaurant() {
        for (int i = 0; i < tables.Count; i++) {
            tables[i].CloseRestaurant();
        }
    }
    public void AddTable(Table table) {
        tables.Add(table);
    }
    List<TablePosition> TableEmptys = new List<TablePosition>();
    public bool IsHasTableEmpty() {
        TableEmptys = GetListTablePositionEmpty();
        if (TableEmptys.Count == 0) return false;
        return true;
    }
    public TablePosition GetTablePosition(Customer customer) {
        TableEmptys = GetListTablePositionEmpty();
        if (TableEmptys.Count == 0) return null;
        TablePosition tablePosition = TableEmptys[Random.Range(0, TableEmptys.Count)];
        tablePosition.customer = customer;
        return tablePosition;
    }

    public List<TablePosition> GetListTablePositionEmpty() {
        TableEmptys.Clear();
        for (int i = 0; i < tables.Count; i++) {
            //var list = tables[i].tablePositions.Where(x => x.customer == null).ToList();
            //TableEmptys.AddRange(list);
            for (int j = 0; j < tables[i].tablePositions.Count; j++) {
                if (tables[i].tablePositions[j].customer == null) {
                    TableEmptys.Add(tables[i].tablePositions[j]);
                }
            }
        }
        return TableEmptys;
    }

    /// <summary>
    ///  Customer Eating Time
    /// </summary>
    /// <param name="tablePosition"></param>
    /// <returns></returns>
    public float GetTimeServiceOnTable(TablePosition tablePosition) {
        for (int i = 0; i < tables.Count; i++) {
            if (tables[i].IsContainerTablePosition(tablePosition)) {
                float time = (float)tables[i].roomController.GetTimeServiceCurrent() * GameManager.instance.customerEatingTimeRate * managerRateProcessing;
                if (time <= 3) time = 3;
                return time;
            }
        }
        return 0;
    }
    /// <summary>
    ///  Tip Money For Waiter
    /// </summary>
    /// <param name="tablePosition"></param>
    public void PaymentTable(TablePosition tablePosition) {
        for (int i = 0; i < tables.Count; i++) {
            if (tables[i].IsContainerTablePosition(tablePosition)) {
                BigNumber money = tables[i].GetMoneyEarnInTable();
                money *= GameManager.GetTotalIncomeRate() * tablePosition.customer.GetVipRate() * managerRateIcome;
                UIManager.instance.CreatUIMoneyEff(money, tablePosition.customer.GetTransform());
                GameManager.SpawnCashFx(tablePosition.customer.GetTransform());
                ProfileManager.PlayerData.AddCash(money);
                break;
            }
        }
    }

    public GameObject CreatFood(int id, Transform parent) {
        Research research = ProfileManager.Instance.dataConfig.researchDataConfig.GetResearchByID(id);
        if (research != null) return Instantiate(research.objFood, parent).gameObject;
        return null;
    }
}

