using DG.Tweening;
using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliverRoomManager : MonoBehaviour {

    [HideInInspector] public int GroupID;
    public List<DeliverPosition> deliverPositionsSetting;
    public List<DeliverCakePosition> cakePositionsSetting;
    public static DeliverRoomManager instance;
    public RoomController<DeliverModelType> roomController;
    public List<DeliverStaff> StaffList = new List<DeliverStaff>();
    /// <summary>
    /// Default Path To Move In, Move Out
    /// </summary>
    [Header("**Default Path To Move In**")]
    public DOTweenPath BeginPath;
    [Header("**Default Path To  Move Out**")]
    public DOTweenPath OutPath;
    /// <summary>
    /// Path Move In & Out Queue
    /// </summary>
    [Header("**Path Move In Queue**")]
    public DOTweenPath[] QueueInPaths;
    [Header("**Path Move Out Queue**")]
    public DOTweenPath[] QueueOutPaths;
    [SerializeField] Transform DeliverCarCustomerPrefab;
    int DeliverCarCustomerSpawnTime = 2;
    float dtTimeSpawn;
    float managerRateIcome, managerRateProcessing;
    private void Awake() {
        instance = this;
        GroupID = roomController.GetGroupID();
        AllRoomManager.instance._DeliverManagers = this;

    }
    private void Start() {
        LoadManagerRate();
        if (GroupID == 0) {
            EventManager.AddListener(EventName.UpdateCardManager.ToString(), (x) => {
                if ((int)x == (int)roomController.GetManagerStaffID()) {
                    LoadManagerRate();
                }
            });
        }
    }
    void LoadManagerRate() {

        CardManagerSave saver = ProfileManager.PlayerData.GetCardManager().GetCardManager(roomController.GetManagerStaffID());
        managerRateIcome = ProfileManager.Instance.dataConfig.cardData.GetIncomeRateByLevel(saver.level, saver.rarity);
        managerRateProcessing = 1f - ProfileManager.Instance.dataConfig.cardData.GetIncomeRateByLevel(saver.level, saver.rarity) / 100f;
    }
    private void Update() {
        dtTimeSpawn -= Time.deltaTime;
        if (dtTimeSpawn <= 0) {
            dtTimeSpawn = DeliverCarCustomerSpawnTime;
            SpawnDeliverCarCustomer();
        }

    }
    void SpawnDeliverCarCustomer() {
        Transform car = PoolManager.Pools["GameEntity"].Spawn(DeliverCarCustomerPrefab, BeginPath.transform.position, Quaternion.identity);
        car.GetComponent<DeliverCarCustomer>().OnInit();
    }
    public void UpdateStaffList() {
        int count = StaffList.Count;
        while (count < roomController.staffSetting.listStaffTrans.Count) {
            StaffList.Add(roomController.staffSetting.listStaffTrans[count].GetComponent<DeliverStaff>());

            count++;
        }
    }
    public void OrderStaff(DeliverCarCustomer customer) {
        for (int i = 0; i < StaffList.Count; i++) {
            if (StaffList[i]._StaffIndex == customer.deliverPosTarget.IndexQueue) {
                StaffList[i].MakeFood(customer);
                return;
            }
        }
    }

    #region Delivers
    public void AddPosition(DeliverPosition pos) {
        if (!deliverPositionsSetting.Contains(pos)) {

            var queue = QueueOutPaths.OrderBy(x => Vector3.Distance(x.transform.position, pos.transform.position)).FirstOrDefault();
            int index = QueueOutPaths.ToList().IndexOf(queue);
            pos.IndexQueue = index;
            bool IsAddNewQueue = true;
            // Find In List DeliverPos, If Has Pos with index == pos.index, replace transform, 
            // else add pos to list DeliverPos
            foreach (var q in deliverPositionsSetting) {
                if (q.IndexQueue == pos.IndexQueue) {
                    q.transform = pos.transform;
                    IsAddNewQueue = false;
                    break;
                }
            }
            if (IsAddNewQueue) {
                deliverPositionsSetting.Add(pos);
                roomController.staffSetting.OnHireStaff();
            }
        }
    }

    public void OutPosition(DeliverCarCustomer customer) {
        for (int i = 0; i < deliverPositionsSetting.Count; i++) {
            if (deliverPositionsSetting[i].customer == customer) {
                deliverPositionsSetting[i].customer = null;
                break;
            }
        }
    }
    List<DeliverPosition> listPostionEmpty = new List<DeliverPosition>();
    int countPositionEmpty;
    public DeliverPosition GetDeliverPostionEmpty(DeliverCarCustomer customer) {
        listPostionEmpty = GetListPositionEmpty();
        countPositionEmpty = listPostionEmpty.Count;
        if (countPositionEmpty == 0) return null;
        DeliverPosition pos = listPostionEmpty[Random.Range(0, countPositionEmpty)];
        pos.customer = customer;
        return pos;
    }

    List<DeliverPosition> GetListPositionEmpty() {
        listPostionEmpty.Clear();
        for (int i = 0; i < deliverPositionsSetting.Count; i++) {
            if (deliverPositionsSetting[i].customer == null)
                listPostionEmpty.Add(deliverPositionsSetting[i]);
        }
        return listPostionEmpty;
    }
    #endregion

    #region Cake
    public void AddCakePosition(DeliverCakePosition pos) {
        if (!cakePositionsSetting.Contains(pos)) {
            if (cakePositionsSetting.Count == 0) {
                // Add new cake position
                cakePositionsSetting.Add(pos);
                return;
            }
            var nearestCake = cakePositionsSetting.OrderBy(x => Vector3.Distance(x.transform.position, pos.transform.position)).FirstOrDefault();
            // Is Replace Cake When Upgrade
            if (Vector3.Distance(nearestCake.transform.position, pos.transform.position) <= 0.1f) {
                nearestCake.transform = pos.transform;
            } else {
                // Add new cake position
                cakePositionsSetting.Add(pos);
            }

        }
    }

    public void OutCakePosition(DeliverStaff staff) {
        for (int i = 0; i < cakePositionsSetting.Count; i++) {
            if (cakePositionsSetting[i].staff == staff) {
                cakePositionsSetting[i].staff = null;
                break;
            }
        }
    }
    List<DeliverCakePosition> listCakePostionEmpty = new List<DeliverCakePosition>();
    int countCakePositionEmpty;
    public DeliverCakePosition GetCakePostionIndexEmpty(DeliverStaff staff) {
        listCakePostionEmpty = GetListCakePositionEmpty();
        countCakePositionEmpty = listCakePostionEmpty.Count;
        if (countCakePositionEmpty == 0) return null;
        DeliverCakePosition pos = listCakePostionEmpty[Random.Range(0, countCakePositionEmpty)];
        pos.staff = staff;
        return pos;
    }

    List<DeliverCakePosition> GetListCakePositionEmpty() {
        listCakePostionEmpty.Clear();
        for (int i = 0; i < cakePositionsSetting.Count; i++) {
            if (cakePositionsSetting[i].staff == null)
                listCakePostionEmpty.Add(cakePositionsSetting[i]);
        }
        return listCakePostionEmpty;
    }
    #endregion
    public float GetTimeService() {
        return roomController.GetTimeService() * managerRateProcessing;
    }

    BigNumber roomValue;
    public void Payment(DeliverStaff staff) {
        roomValue = (roomController.GetTotalMoneyEarn() + staff.customerOrder.GetOrderFoodValue()) * GameManager.instance.GetTotalIncomeRate() * managerRateIcome;
        staff.CalculateSkinBuffIncome(ref roomValue);
        UIManager.instance.CreatUIMoneyEff(roomValue, staff.transform);
        ProfileManager.PlayerData.AddCash(roomValue);
    }

}
