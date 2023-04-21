using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using PathologicalGames;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SDK;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    [Header("LobbyRoomController ********************************")]
    public LobbyRoomController LobbyRoom;
    [Header("ManagerRoomController ********************************")]
    public ManagerRoomController ManagerRoom;
    [Header("KitchenRoomController ********************************")]
    public KitchenRoomController KitchenRoom;
    [Header("PowerRoomController ********************************")]
    public PowerRoomController PowerRoom;
    [Header("RestroomController ********************************")]
    public List<RestroomController> WCRooms = new List<RestroomController>();
    //[Header("CleanRoomController ********************************")]
    //public CleanRoomController CleanRoom;
    [Header("DeliverRoomController ********************************")]
    public DeliverRoomController DeliverRoom;
    [Header("SmallTableRoomController ********************************")]
    public SmallTableRoomController[] SmallTablesRoom;
    [Tooltip("BigTableRoomController ********************************")]
    public BigTableRoomController[] BigTablesRoom;
    public IRoomController selectedRoom;
    public BuildData buildData;
    public Transform[] resOutPositions;
    public Transform[] resDespawnPositions;
    public List<Transform> CarMoveTrans1, CarMoveTrans2, CarMoveTrans3;
    public Transform famousCustomerInPos;
    float timeDefaultCustomer = 5;
    float deltaTimeCustomer, deltaTimeCar, deltaTimeCustomerDefault, deltaTimeFamousCustomer;
    float timeFamousCustomer = 180f;
    [HideInInspector] public bool isNegotitationRunning;
    float vipRate = 5;
    List<Customer> customersInRes = new List<Customer>();
    [HideInInspector] public CustomTimeManager timeManager;
    AdBoostManager boostManager;
    MarketingManager marketingManager;
    CardManager cardManager;
    OrderBookManager negotiationManager;
    public bool IsPauseGame;
    [HideInInspector] public float customerSpeedRate = 1;
    [HideInInspector] public float popularityRate = 1;
    [HideInInspector] public float financeRate = 1;
    [HideInInspector] public float customerEatingTimeRate = 1;
    [HideInInspector] public float waiterSpeedRate = 1;
    [HideInInspector] public float customerWCTimeRate = 1;
    [HideInInspector] public float tipBaseRate = 0.15f;
    [HideInInspector] public float tipWaiterRateCard = 1;
    [HideInInspector] public float tipChefRateCard = 1;
    [HideInInspector] public float tipCleanerRateCard;
    [HideInInspector] public float tipReceptionRate = 1;
    [HideInInspector] public float chefCookTimeRate = 1;
    [HideInInspector] public float deliciousRate = 1;
    [HideInInspector] public float receptionOrderTimeRate = 1;
    [HideInInspector] public float restaurantPopularityRate = 1;
    [HideInInspector] public float customerWaitTimeRate = 1;
    [HideInInspector] public float cleanerSpeedRate = 1;
    [HideInInspector] public float cleanerWorkTimeRate = 1;
    public bool IsSkipSleep;
    public List<int> _RoomCostServer;
    public List<int> _RoomVIPCostServer;
    [HideInInspector] public List<Transform> _EmployeeSleeping = new List<Transform>();
    public Sprite[] emoji_sad;
    public Sprite[] emoji_funny;
    public Sprite[] emoji_angry;
    private void Awake() {
        instance = this;
        timeManager = ProfileManager.PlayerData.GetCustomTimeManager();
        boostManager = ProfileManager.PlayerData.GetAdBoostManager();
        marketingManager = ProfileManager.PlayerData.GetMarketingManager();
        cardManager = ProfileManager.PlayerData.GetCardManager();
        negotiationManager = ProfileManager.PlayerData.GetOrderBookManager();
        LoadRoom();
        EventManager.AddListener(EventName.UpdateStaff.ToString(), (x) => {
            UpdateStaffByID((int)x);
        });
        EventManager.AddListener(EventName.UpgradeCard.ToString(), LoadCardRate);
        //LoadRoomCostServer();
    }
    void LoadCardRate() {
        UpdateFinanceRate();
        customerSpeedRate = cardManager.GetCustomerSpeedRate();
        popularityRate = cardManager.GetRestaurantPopularityRate();
        tipBaseRate = 0.1f * cardManager.GetCustomerSatisfactionRate();
        customerEatingTimeRate = cardManager.GetCustomerEatingTimeRate();
        waiterSpeedRate = cardManager.GetWaiterSpeedRate();
        customerWCTimeRate = cardManager.GetCustomerWCTimeRate();
        tipChefRateCard = cardManager.GetTipChefRate();
        tipReceptionRate = cardManager.GetTipReceptionRate();
        chefCookTimeRate = cardManager.GetChefCookingTimeRate();
        deliciousRate = cardManager.GetDeliciousFoodRate();
        receptionOrderTimeRate = cardManager.GetReceptionistOrderTimeRate();
        restaurantPopularityRate = cardManager.GetRestaurantPopularityRate();
        customerWaitTimeRate = cardManager.GetCustomerWaitingTimeRate();
        tipWaiterRateCard = cardManager.GetTipWaiterRate();
        cleanerSpeedRate = cardManager.GetCleanerSpeedRate();
        cleanerWorkTimeRate = cardManager.GetCleanerWorkingTimeRate();
        tipCleanerRateCard = cardManager.GetTipCleanerRate();
    }
    public void UpdateFinanceRate() {
        financeRate = cardManager.GetFinanceRate();
    }

    void UpdateStaffByID(int staffID) {
        AllRoomManager.instance.UpdateStaffList(staffID);
    }

    private void Start() {
        AllRoomManager.instance.UpdateStaffListAll();
        deltaTimeFamousCustomer = timeFamousCustomer;
        LoadCardRate();
        CalculateProfitOffline();
        UpdateStarProcess();
    }
    void LoadRoom() {
        ManagerRoom.OnLoadRoom();
        ManagerRoom.TriggerQuestWhenUnlock();
        LobbyRoom.OnLoadRoom();
        LobbyRoom.TriggerQuestWhenUnlock();
        KitchenRoom.OnLoadRoom();
        KitchenRoom.TriggerQuestWhenUnlock();
        // Load PowerRoom
        if (IsUnlockPowerRoom()) PowerRoom.OnLoadRoom();
        else PowerRoom.OnLockRoom();

        for (int i = 0; i < WCRooms.Count; i++) {
            // Load WCRoom
            if (IsUnlockRestroom(WCRooms[i])) WCRooms[i].OnLoadRoom();
            else WCRooms[i].OnLockRoom();
        }
        //// Load CleanRoom
        //if (IsUnlockCleanRoom()) {
        //    CleanRoom.OnLoadRoom();
        //    //objCashTipClean.SetActive(true);
        //} else {
        //    CleanRoom.OnLockRoom();
        //    //objCashTipClean.SetActive(false);
        //}
        // Load DeliverRoom
        if (IsUnlockDeliverRoom()) DeliverRoom.OnLoadRoom();
        else DeliverRoom.OnLockRoom();
        // Load Tables
        OnLoadTablesStage();
        if (!IsUnlockSmallTable(0) && GetCash() < 1000) AddCash(1000);
    }
    void OnLoadTablesStage() {

        int IndexUnlock = 0;
        for (int i = 0; i < SmallTablesRoom.Length; i++) {
            if (ProfileManager.Instance.playerData.GetRoomData(SmallTablesRoom[i].roomSetting) != null) {
                SmallTablesRoom[i].OnLoadRoom();
                IndexUnlock = i + 1;
            } else {
                if (i == IndexUnlock) SmallTablesRoom[i].OnLockRoom();
                else SmallTablesRoom[i].OnHideRoom();
            }
        }
        if (IndexUnlock == SmallTablesRoom.Length) IndexUnlock = 0;
        else IndexUnlock = -1;
        for (int i = 0; i < BigTablesRoom.Length; i++) {
            if (ProfileManager.Instance.playerData.GetRoomData(BigTablesRoom[i].roomSetting) != null) {
                BigTablesRoom[i].OnLoadRoom();
                IndexUnlock = i + 1;
            } else {
                if (i == IndexUnlock) BigTablesRoom[i].OnLockRoom();
                else BigTablesRoom[i].OnHideRoom();
            }
        }
       
    }

    Vector3 touchUp, touchDown;
    bool isTouching;
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Delete)) {
            ProfileManager.Instance.ClearData();
            ReloadGame();
        }
        if (!Tutorials.instance.IsRunStory) {
            cashRateTime += Time.deltaTime;
            timeManager.Update();
            FillCustomerOnStart();
            UpdateCustomerAndCar();
            UpdateTimeSleep();
        };
        if (Input.touchCount > 0) {
            // Check if finger is over a UI element
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
                return;
            }
        }
        if (Tutorials.instance.IsBlockInput) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonDown(0)) {
            touchDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isTouching = true;
        }
        if (isTouching) {
            if (Input.GetMouseButtonUp(0)) {
                isTouching = false;
                touchUp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Vector3.Distance(touchDown, touchUp) >= 0.1f) return;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                if (hit.collider != null) {
                    bool IsAcceptTouch;
                    UIBuildTarget buildTarget = hit.collider.GetComponent<UIBuildTarget>();
                    if (buildTarget != null) {
                        IsAcceptTouch = IsCorrectRoomTutorial(buildTarget);
                        if (IsAcceptTouch) UIManager.instance.ShowPanelManagerBuild(buildTarget.target);
                    } else {
                        selectedRoom = hit.collider.GetComponent<IRoomController>();
                        if (selectedRoom != null) {
                            IsAcceptTouch = IsCorrectRoomTutorial(selectedRoom);
                            if (IsAcceptTouch) {

                                CameraMove.instance.ZoomOutCamera();
                                UIManager.instance.ShowPanelRoomInfo(selectedRoom);
                            }
                        }
                    }
                    if (hit.collider.tag == "TipProfit" && !Tutorials.instance.IsShow) {
                        UIManager.instance.ShowPanelTipProfit();
                    }
                }
            }
        }
    }

    bool IsCorrectRoomTutorial<T>(T selected) {
        if (!Tutorials.instance.IsShow) return true;
        if (selected is UIBuildTarget) {
            if (Tutorials.instance.GetTutorialStep() == TutorialStepID.BuildTable && (selected as UIBuildTarget).target == RoomID.Table1) return true;
            if (Tutorials.instance.GetTutorialStep() == TutorialStepID.BuildPower && (selected as UIBuildTarget).target == RoomID.Power) return true;
            if (Tutorials.instance.GetTutorialStep() == TutorialStepID.BuildClean && (selected as UIBuildTarget).target == RoomID.Clean) return true;
            if (Tutorials.instance.GetTutorialStep() == TutorialStepID.BuildRestroom && (selected as UIBuildTarget).target == RoomID.Restroom) return true;

        } else if (selected is IRoomController) {
            if (Tutorials.instance.GetTutorialStep() == TutorialStepID.UpgradeTable && selectedRoom == SmallTablesRoom[0] as IRoomController) return true;
            if (Tutorials.instance.GetTutorialStep() == TutorialStepID.HireStaff && selectedRoom == SmallTablesRoom[0] as IRoomController) return true;
        }
        return false;
    }
    bool IsFirstFillCustomer;
    void FillCustomerOnStart() {
        if (!IsFirstFillCustomer) {
            IsFirstFillCustomer = true;
            int max = TableManager.instance.GetListTablePositionEmpty().Count;
            int count = 0;
            while (count < max) {
                count++;
                bool isFill = Random.Range(0, 100) >= 80;
                if (!isFill) continue;
                Transform t = PoolManager.Pools["GameEntity"].Spawn("Customer", GetDespawnPos(), Quaternion.identity);
                t.gameObject.SetActive(true);
                Customer cus = t.GetComponent<Customer>();
                cus.InitCustomer();
                cus.UpdateMoveSpeed(customerSpeedRate);
                cus.OnCustomerUseFoodStateOnInit();
            }
        }
    }

    void UpdateCustomerAndCar() {
        deltaTimeCustomer += Time.deltaTime;
        if (deltaTimeCustomer >= marketingManager.GetCustormerSpawnTime()) {
            deltaTimeCustomer = 0;
            if (LobbyRoomManager.instance.IsHasLobbyQueueEmpty()) SpawnCustomer();
        }
        deltaTimeCustomerDefault += Time.deltaTime;
        if (deltaTimeCustomerDefault >= timeDefaultCustomer) {
            deltaTimeCustomerDefault = 0;
            timeDefaultCustomer = Random.Range(3f, 5f);
            SpawnDefaultCustomer();
        }
        deltaTimeCar += Time.deltaTime;
        if (deltaTimeCar >= 5f) {
            deltaTimeCar = 0;
            SpawnCar();
        }
    }
    void SpawnCar() {
        int rd = Random.Range(0, 2);
        if (rd == 0) {
            Transform t = PoolManager.Pools["GameEntity"].Spawn("Car", CarMoveTrans1[0].transform.position, Quaternion.identity);
            t.gameObject.SetActive(true);
            t.GetComponent<Car>().InitCar(CarMoveTrans1);

        } else {
            Transform t = PoolManager.Pools["GameEntity"].Spawn("Car", CarMoveTrans2[0].transform.position, Quaternion.identity);
            t.gameObject.SetActive(true);
            t.GetComponent<Car>().InitCar(CarMoveTrans2);
        }
    }


    public void SpawnCustomer() {
        Transform t = PoolManager.Pools["GameEntity"].Spawn("Customer", GetDespawnPos(), Quaternion.identity);
        t.gameObject.SetActive(true);
        Customer cus = t.GetComponent<Customer>();
        vipRate = marketingManager.GetSpawnVIPRate();
        bool isVip = Random.Range(0, 100) <= (vipRate + popularityRate);
        //isVip = false;
        if (!LobbyRoomManager.instance.IsHasLobbyQueueEmpty()) {
            cus.InitCustomer(isVip);
            cus.OnNotComeToRestaurant();
        } else {
            cus.InitCustomer(isVip);
            cus.OnComeToRestaurant();
            cus.UpdateMoveSpeed(customerSpeedRate);
        }
    }
    public void SpawnCustomer_Story() {
        Transform t = PoolManager.Pools["GameEntity"].Spawn("Customer", resDespawnPositions[0].transform.position, Quaternion.identity);
        t.gameObject.SetActive(true);
        Customer cus = t.GetComponent<Customer>();
        cus.InitCustomer(false);
        cus.OnComeToRestaurant();
        cus.UpdateMoveSpeed(1.2f);
        CameraMove.instance.SetTargetFollow(t);
    }

    void SpawnDefaultCustomer() {
        Transform t = PoolManager.Pools["GameEntity"].Spawn("Customer", GetDespawnPos(), Quaternion.identity);
        t.gameObject.SetActive(true);
        Customer cus = t.GetComponent<Customer>();
        bool isVip = Random.Range(0, 100) <= (vipRate + popularityRate);
        cus.InitCustomer(isVip);
        cus.OnNotComeToRestaurant();
    }

    public Transform[] GetOutDoorPos() {
        return resOutPositions;
    }
    public Vector3 GetDespawnPos() {
        return resDespawnPositions[Random.Range(0, resDespawnPositions.Length)].position;
    }
    public bool IsUnlockPowerRoom() {
        return ProfileManager.PlayerData.GetRoomData(PowerRoom.roomSetting) != null ? true : false;
    }
    public void UnlockPowerRoom() {
        ProfileManager.PlayerData.SaveRoomData(PowerRoom.roomSetting);
        PowerRoom.gameObject.SetActive(true);
        PowerRoom.OnLoadRoom();
        if (PowerRoom.GetComponent<DropRoomEffect>() != null) PowerRoom.GetComponent<DropRoomEffect>().OnDrop();
    }
    public bool IsUnlockRestroom(RestroomController room) {
        return ProfileManager.PlayerData.GetRoomData(room.roomSetting) != null ? true : false;
    }
    public bool IsUnlockRestroom(int GroupID) {
        foreach (var room in WCRooms) {
            if (room.roomSetting.GroupID == GroupID) {
                return ProfileManager.PlayerData.GetRoomData(room.roomSetting) != null ? true : false;
            }
        }
        return false;
    }
    public void UnlockWCRoom(RestroomController room) {
        ProfileManager.PlayerData.SaveRoomData(room.roomSetting);
        room.gameObject.SetActive(true);
        room.OnLoadRoom();
        if (room.GetComponent<DropRoomEffect>() != null) room.GetComponent<DropRoomEffect>().OnDrop();
    }


    public bool IsUnlockDeliverRoom() {
        return ProfileManager.PlayerData.GetRoomData(DeliverRoom.roomSetting) != null ? true : false;
    }
    public void UnlockCoffeeRoom() {
        ProfileManager.PlayerData.SaveRoomData(DeliverRoom.roomSetting);
        DeliverRoom.gameObject.SetActive(true);
        DeliverRoom.OnLoadRoom();
        if (DeliverRoom.GetComponent<DropRoomEffect>() != null) DeliverRoom.GetComponent<DropRoomEffect>().OnDrop();
    }

    public bool IsUnlockSmallTable(int id) {
        return ProfileManager.PlayerData.GetRoomData(SmallTablesRoom[id].roomSetting) != null ? true : false;
    }
    void UnlockSmallTable(int id) {
        ProfileManager.PlayerData.SaveRoomData(SmallTablesRoom[id].roomSetting);
        SmallTablesRoom[id].gameObject.SetActive(true);
        SmallTablesRoom[id].OnLoadRoom();
        if (SmallTablesRoom[id].GetComponent<DropRoomEffect>() != null) SmallTablesRoom[id].GetComponent<DropRoomEffect>().OnDrop();
        for(int i=0;i < SmallTablesRoom[id]._NextRoomsWhenUnlock.Count; i++) {
            SmallTablesRoom[id]._NextRoomsWhenUnlock[i].GetComponent<IRoomController>().OnLockRoom();
        }
    }

    public bool IsUnlockBigTable(int id) {
        return ProfileManager.PlayerData.GetRoomData(BigTablesRoom[id].roomSetting) != null ? true : false;
    }
    void UnlockBigTable(int id) {
        ProfileManager.PlayerData.SaveRoomData(BigTablesRoom[id].roomSetting);
        BigTablesRoom[id].gameObject.SetActive(true);
        BigTablesRoom[id].OnLoadRoom();
        if (BigTablesRoom[id].GetComponent<DropRoomEffect>() != null) BigTablesRoom[id].GetComponent<DropRoomEffect>().OnDrop();
        for (int i = 0; i < BigTablesRoom[id]._NextRoomsWhenUnlock.Count; i++) {
            BigTablesRoom[id]._NextRoomsWhenUnlock[i].GetComponent<IRoomController>().OnLockRoom();
        }
    }

    public void BuildRoom(RoomID buildTarget, int GroupID = 0) {

        switch (buildTarget) {
            case RoomID.Power:
                UnlockPowerRoom();
                PowerRoom.TriggerQuestWhenUnlock();
                break;
            case RoomID.Restroom:
                UnlockWCRoom(WCRooms[0]);
                WCRooms[0].TriggerQuestWhenUnlock();
                break;
            case RoomID.Restroom2:
                UnlockWCRoom(WCRooms[1]);
                WCRooms[1].TriggerQuestWhenUnlock();
                break;

            case RoomID.DeliverRoom:
                UnlockCoffeeRoom();
                DeliverRoom.TriggerQuestWhenUnlock();
                EventManager.TriggerEvent(EventName.UpdateStaff.ToString(), (int)StaffID.Deliver);
                CheckShowRatePopup();
                break;
            case RoomID.Table1:
                UnlockSmallTable(0);
                SmallTablesRoom[0].TriggerQuestWhenUnlock();
                EventManager.TriggerEvent(EventName.UpdateStaff.ToString(), (int)StaffID.Waiter);
                break;
            case RoomID.Table2:
                UnlockSmallTable(1);
                SmallTablesRoom[1].TriggerQuestWhenUnlock();
                SmallTablesRoom[1].OnHireStaff();
                break;
            case RoomID.Table3:
                UnlockSmallTable(2);
                SmallTablesRoom[2].TriggerQuestWhenUnlock();
                SmallTablesRoom[2].OnHireStaff();
                break;
            case RoomID.Table4:
                UnlockSmallTable(3);
                SmallTablesRoom[3].TriggerQuestWhenUnlock();
                SmallTablesRoom[3].OnHireStaff();
                break;
            case RoomID.Table5:
                UnlockSmallTable(4);
                SmallTablesRoom[4].TriggerQuestWhenUnlock();
                SmallTablesRoom[4].OnHireStaff();
                break;
            case RoomID.Table6:
                UnlockSmallTable(5);
                SmallTablesRoom[5].TriggerQuestWhenUnlock();
                SmallTablesRoom[5].OnHireStaff();
                break;
            case RoomID.BigTable1:
                UnlockBigTable(0);
                BigTablesRoom[0].TriggerQuestWhenUnlock();
                BigTablesRoom[0].OnHireStaff();
                CheckShowRatePopup();
                break;
            case RoomID.BigTable2:
                UnlockBigTable(1);
                BigTablesRoom[1].TriggerQuestWhenUnlock();
                BigTablesRoom[1].OnHireStaff();
                CheckShowRatePopup();
                break;
            case RoomID.BigTable3:
                UnlockBigTable(2);
                BigTablesRoom[2].TriggerQuestWhenUnlock();
                BigTablesRoom[2].OnHireStaff();
                CheckShowRatePopup();
                break;
            case RoomID.BigTable4:
                UnlockBigTable(3);
                BigTablesRoom[3].TriggerQuestWhenUnlock();
                BigTablesRoom[3].OnHireStaff();
                CheckShowRatePopup();
                break;
            case RoomID.BigTable5:
                UnlockBigTable(4);
                BigTablesRoom[4].TriggerQuestWhenUnlock();
                BigTablesRoom[4].OnHireStaff();
                CheckShowRatePopup();
                break;
            case RoomID.BigTable6:
                UnlockBigTable(5);
                BigTablesRoom[5].TriggerQuestWhenUnlock();
                BigTablesRoom[5].OnHireStaff();
                CheckShowRatePopup();
                break;
            case RoomID.BigTable7:
                UnlockBigTable(6);
                BigTablesRoom[6].TriggerQuestWhenUnlock();
                BigTablesRoom[6].OnHireStaff();
                CheckShowRatePopup();
                break;
            case RoomID.BigTable8:
                UnlockBigTable(7);
                BigTablesRoom[7].TriggerQuestWhenUnlock();
                BigTablesRoom[7].OnHireStaff();
                CheckShowRatePopup();
                break;
            case RoomID.BigTable9:
                UnlockBigTable(8);
                BigTablesRoom[8].TriggerQuestWhenUnlock();
                BigTablesRoom[8].OnHireStaff();
                CheckShowRatePopup();
                break;
            case RoomID.BigTable10:
                UnlockBigTable(9);
                BigTablesRoom[9].TriggerQuestWhenUnlock();
                BigTablesRoom[9].OnHireStaff();
                CheckShowRatePopup();
                break;
            case RoomID.BigTable11:
                UnlockBigTable(10);
                BigTablesRoom[10].TriggerQuestWhenUnlock();
                BigTablesRoom[10].OnHireStaff();
                CheckShowRatePopup();
                break;
            case RoomID.BigTable12:
                UnlockBigTable(11);
                BigTablesRoom[11].TriggerQuestWhenUnlock();
                BigTablesRoom[11].OnHireStaff();
                CheckShowRatePopup();
                break;
            case RoomID.BigTable13:
                UnlockBigTable(12);
                BigTablesRoom[12].TriggerQuestWhenUnlock();
                BigTablesRoom[12].OnHireStaff();
                CheckShowRatePopup();
                break;
            case RoomID.BigTable14:
                UnlockBigTable(13);
                BigTablesRoom[13].TriggerQuestWhenUnlock();
                BigTablesRoom[13].OnHireStaff();
                CheckShowRatePopup();
                break;
        }
        UpdateStarProcess();
        QuestHelperUI.instance.TriggerQuestBuildRoom(buildTarget);
        ABIAnalyticsManager.Instance.TrackUnlockRoom(buildTarget, ProfileManager.PlayerData.GetSelectedWorld());

    }
    public int GetTotalEnergyUsed() {
        int total = 0;
        foreach (var room in WCRooms) {
            if (IsUnlockRestroom(room)) {
                total += buildData.GetBuildEnergy(RoomID.Restroom);
            }
        }

        if (IsUnlockDeliverRoom()) {
            total += buildData.GetBuildEnergy(RoomID.DeliverRoom);
        }

        for (int i = 0; i < SmallTablesRoom.Length; i++) {
            if (IsUnlockSmallTable(i)) {
                total += buildData.GetBuildEnergy(SmallTablesRoom[i].GetRoomID());
            }
        }
        for (int i = 0; i < BigTablesRoom.Length; i++) {
            if (IsUnlockBigTable(i)) {
                total += buildData.GetBuildEnergy(BigTablesRoom[i].GetRoomID());
            }
        }
        return total;
    }
    public int GetTablesUnlock() {
        int total = 0;
        for (int i = 0; i < SmallTablesRoom.Length; i++) {
            if (IsUnlockSmallTable(i)) total++;
        }
        for (int i = 0; i < BigTablesRoom.Length; i++) {
            if (IsUnlockBigTable(i)) total++;
        }
        return total;
    }
    public int GetTotalChef() {
        return KitchenRoom.staffSetting.GetTotalStaffCurrent();
    }
    public bool IsMaxChef() {
        return KitchenRoom.staffSetting.GetTotalStaffCurrent() >= KitchenRoom.staffSetting.GetTotalStaff();
    }
 
    public int GetTotalWaiter() {
        return SmallTablesRoom[0].staffSetting.GetTotalStaffCurrent();
    }
    public bool IsMaxWaiter() {
        return SmallTablesRoom[0].staffSetting.GetTotalStaffCurrent() >= SmallTablesRoom[0].staffSetting.GetTotalStaff();
    }
    public bool IsEnoughCash(BigNumber amount) {
        bool b = GetCash() >= amount;
        return b;
    }
    public bool IsEnoughResearchValue(int value) {
        bool check = GetResearchValue() >= value;
        return check;
    }
    public bool IsEnoughEnergy(int amount) {
        if (IsUnlockPowerRoom()) {
            int value = PowerRoom.GetTotalEnergyEarn() - GetTotalEnergyUsed();
            if (value < 0) value = 0;
            return value >= amount;
        } else if (amount > 0) return false;
        return true;
    }
    public BigNumber GetCash() {
        return ProfileManager.PlayerData.GetCash();
    }
    public int GetResearchValue() {
        return ProfileManager.PlayerData.GetResearchValue();
    }
    public void ConsumeCash(float amount) {
        ProfileManager.PlayerData.ConsumeCash(amount);
    }
    public void AddCash(float amount) {
        ProfileManager.PlayerData.AddCash(amount);
    }
    public void AddCash(BigNumber amount) {
        ProfileManager.PlayerData.AddCash(new BigNumber(amount));
    }

    public int GetGem() {
        return ProfileManager.PlayerData.GetGem();
    }
    public bool IsEnoughGem(int amount) {
        return GetGem() >= amount;
    }
    public int GetPowerRoomEnergy() {
        if (IsUnlockPowerRoom()) return PowerRoom.totalEnergyEarn;
        else return 0;
    }

    void CalculateProfitOffline() {
        int totalMinutesOffline = timeManager.GetOfflineTime() / 60;
        if (totalMinutesOffline < 2) return;
        offlineProfit += CalculateProfitFood(timeManager.GetOfflineTime());
        offlineProfit += CalculateProfitRestroom(timeManager.GetOfflineTime());
        offlineProfit += CalculateProfitDeliver(timeManager.GetOfflineTime());
        offlineProfit *= financeRate;
        if (offlineProfit > 0) {
            if (!Tutorials.instance.IsShow)
                UIManager.instance.ShowPanelBalanceOffline();
            else {
                AddCash(offlineProfit);
                UIManager.instance.ShowUIMoneyProfit(offlineProfit);
            }
        }
    }
    BigNumber offlineProfit= new BigNumber(0);
    public BigNumber GetOfflineProfit() {
        return offlineProfit;
    }
    int customerPerTurn = 0;
    BigNumber CalculateProfitFood(int time) {
        BigNumber profit = 0;
        customerPerTurn = 0;
        int timePerTurn = 3;
        int totalMinutes = time / 60;
        int turn = totalMinutes / timePerTurn;
        for (int i = 0; i < SmallTablesRoom.Length; i++) {
            if (IsUnlockSmallTable(i)) {
                // small table has 2 customer
                customerPerTurn += 2;
                profit += turn * (SmallTablesRoom[i].GetTotalMoneyEarn() + KitchenRoom.GetTotalMoneyEarn() + LobbyRoom.GetTotalMoneyEarn()) * 2;

            }
        }
        for (int i = 0; i < BigTablesRoom.Length; i++) {
            if (IsUnlockBigTable(i)) {
                customerPerTurn += 2;
                profit += turn * (BigTablesRoom[i].GetTotalMoneyEarn() + KitchenRoom.GetTotalMoneyEarn() + LobbyRoom.GetTotalMoneyEarn()) * 4;
            }
        }
        //ProfileManager.PlayerData.researchManager.AddResearchValue(customerPerTurn);
        return profit;
    }
    BigNumber CalculateProfitRestroom(int time) {
        BigNumber profit = 0;
        foreach (var room in WCRooms) {
            if (IsUnlockRestroom(room)) {
                int timePerTurn = 4;
                int totalMinutes = time / 60;
                int turn = totalMinutes / timePerTurn;
                profit += room.GetTotalMoneyEarn() * customerPerTurn * turn;
            }
        }
        return profit;
    }

    BigNumber CalculateProfitDeliver(int time) {
        BigNumber profit = 0;
        if (!IsUnlockDeliverRoom()) return 0;
        int timePerTurn = 8;
        int totalMinutes = time / 60;
        int turn = totalMinutes / timePerTurn;
        profit += DeliverRoom.GetTotalMoneyEarn() * customerPerTurn * turn;
        return profit;
    }

    public float GetTotalIncomeRate() {
        if (boostManager.IsBoostFinanceActive()) return 2f * financeRate;
        return financeRate;
    }
    public void GoToPowerRoom() {
        UIManager.instance.CloseAllPopup();
        if (IsUnlockPowerRoom()) {
            selectedRoom = PowerRoom;
            CameraMove.instance.ChangePosition(PowerRoom.GetTransform().position - new Vector3(0, 6.5f, 0f), null);
            UIManager.instance.ShowPanelRoomInfo(PowerRoom);
        } else {
            UIManager.instance.ShowPanelManagerBuild(RoomID.Power);
            selectedRoom = PowerRoom;
            CameraMove.instance.ChangePosition(PowerRoom.GetTransform().position - new Vector3(0, 6.5f, 0f), null);
        }
    }


    BigNumber baseProfit;
    int timeLoan;
    int freeCashRate = 15;
    float cashRateTime = 60;
    public BigNumber GetFreeCashAdsProfit() {
        baseProfit = 0;
        int num1 = freeCashRate;
        if (cashRateTime < 25f) {
            num1 = 8;
        } else if (cashRateTime < 40f) {
            num1 = 12;
        } else if (cashRateTime < 60f) {
            num1 = 15;
        }
        timeLoan = Random.Range(num1, (num1 + 5)) * 60;
        baseProfit += CalculateProfitFood(timeLoan);
        baseProfit += CalculateProfitRestroom(timeLoan);
        baseProfit += CalculateProfitDeliver(timeLoan);
        baseProfit *= (financeRate * Random.Range(1f, 1.1f));
        if (baseProfit <= 2000) {
            float num = Random.Range(10, 50) * 10;
            baseProfit = 2000 + num;
        }
        return baseProfit;
    }

    public void IncreaseFreeCashRate() {
        if (freeCashRate < 18) {
            freeCashRate++;
        }
    }
    public void ResetFreeCashRate() {
        freeCashRate = 15;
        cashRateTime = 0;
    }
    public int GetFreeGemAdsProfit() {
        return Random.Range(2, 5) * 5;
    }
    /// <summary>
    /// Calculate Cash Earn In Time (Minute)
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public BigNumber GetCashProfit(int time) {
        baseProfit = 0;
        timeLoan = time * 60;
        baseProfit += CalculateProfitFood(timeLoan);
        baseProfit += CalculateProfitRestroom(timeLoan);
        baseProfit += CalculateProfitDeliver(timeLoan);
        baseProfit *= financeRate;
        return baseProfit;
    }
    public void UpdateStarProcess() {
        int current = 0;
        current += LobbyRoom.GetTotalUpgradePoint();
        current += ManagerRoom.GetTotalUpgradePoint();
        current += KitchenRoom.GetTotalUpgradePoint();
        if (IsUnlockPowerRoom()) current += PowerRoom.GetTotalUpgradePoint();
        foreach (var room in WCRooms) {
            if (IsUnlockRestroom(room)) current += room.GetTotalUpgradePoint();
        }
        if (IsUnlockDeliverRoom()) current += DeliverRoom.GetTotalUpgradePoint();
        for (int i = 0; i < SmallTablesRoom.Length; i++) {
            if (IsUnlockSmallTable(i)) current += SmallTablesRoom[i].GetTotalUpgradePoint();
        }
        for (int i = 0; i < BigTablesRoom.Length; i++) {
            if (IsUnlockBigTable(i)) current += BigTablesRoom[i].GetTotalUpgradePoint();
        }
        ProfileManager.PlayerData.SaveUpgradeProcess(current);
        UpdateStarUpgrade();
    }
    public void UpdateStarUpgrade() {
        UIStarUpgradeProcess.instance.UpdateStarUpgrade();
    }
    public void StopFocusRoom() {
        if (selectedRoom != null) selectedRoom.TurnOffSelectedEffectItem();
    }
    public void RegisterCustomerInRes(Customer cus) {
        customersInRes.Add(cus);
    }
    public void UnRegisterCustomerInRes(Customer cus) {
        if (customersInRes.Contains(cus)) customersInRes.Remove(cus);
    }
    public void RegisterEmployeeSleep(Transform t) {
        _EmployeeSleeping.Add(t);
    }
    public void UnRegisterEmployeeSleep(Transform t) {
        if (_EmployeeSleeping.Contains(t)) _EmployeeSleeping.Remove(t);
    }

    public void SpawnCashFx(Transform transform) {
        PoolManager.Pools["GameEntity"].Spawn("CashFX", new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
    }

    public List<RoomID> GetAllRoomID() {
        List<RoomID> list = new List<RoomID>();
        foreach (var room in SmallTablesRoom) {
            list.Add(room.GetRoomID());
        }
        foreach (var room in BigTablesRoom) {
            list.Add(room.GetRoomID());
        }
        list.Add(LobbyRoom.GetRoomID());
        list.Add(KitchenRoom.GetRoomID());
        foreach (var room in WCRooms) {
            list.Add(room.GetRoomID());
        }
        list.Add(DeliverRoom.GetRoomID());
        list.Add(PowerRoom.GetRoomID());
        return list;
    }
    void ReloadGame() {
        Debug.Log("Reload Game");
        PoolManager.Pools["GameEntity"].DespawnAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadScene(int level) {
        ProfileManager.PlayerData.ChangeSelectedWorld(level);
        SoundManager.instance.PlayMusic();
        timeManager.LoadData();
        PoolManager.Pools["GameEntity"].DespawnAll();
        SceneManager.LoadScene("LoadScene");
    }
    public void OnCollectRewardIAPPackage(OfferData currentIAPPackage) {
        if (currentIAPPackage.offerID == OfferID.NoAds || currentIAPPackage.offerID == OfferID.Vip1Pack || currentIAPPackage.offerID == OfferID.Vip2Pack || currentIAPPackage.offerID == OfferID.Vip3Pack || currentIAPPackage.offerID == OfferID.ComboPack_Ads_Researcher_Order) ProfileManager.PlayerData.OnSaveBoughtIAPPackage(currentIAPPackage.offerID);
        List<ItemReward> rewards = currentIAPPackage.itemRewards;
        foreach (var reward in rewards) {
            OnCollectReward(reward, true);

        }
        ProfileManager.PlayerData.ResourceSave.SaveData();
    }
    public void OnCollectReward(ItemReward reward, bool isFromIAP = false) {
        switch (reward.type) {
            case ItemType.Gem:
                ProfileManager.PlayerData.AddGem(reward.amount);
                if (isFromIAP) {
                    ABIAnalyticsManager.Instance.TrackEventGem(GemAction.Earn_IAP, reward.amount);
                }
                break;
            case ItemType.PremiumSuit:
                ProfileManager.PlayerData.skinManager.OnPremiumSuitPurchased();
                ProfileManager.PlayerData.ResourceSave.activePremiumSuit = true;
                break;
            case ItemType.GodenSuit:
                ProfileManager.PlayerData.skinManager.OnGoldenSuitPurchased();
                ProfileManager.PlayerData.ResourceSave.activeGoldenSuit = true;
                break;
            case ItemType.RemoveAds:
                ProfileManager.PlayerData.ResourceSave.SetRemoveAds(true);
                break;
            case ItemType.OfflineTime:
                if (reward.amount == 2) {
                    int id = ProfileManager.Instance.dataConfig.shopConfig.GetCardByOfferID(CardIapProductType.OFFLINE_TIME_2).id;
                    ProfileManager.PlayerData.GetCardManager().AddCardIAPOneTime(id);
                
                } else if (reward.amount == 10) {
                    int id = ProfileManager.Instance.dataConfig.shopConfig.GetCardByOfferID(CardIapProductType.OFFLINE_TIME_10).id;
                    ProfileManager.PlayerData.GetCardManager().AddCardIAPOneTime(id);
                }
                break;
            case ItemType.NormalChest:
                ProfileManager.PlayerData.ResourceSave.AddNormalChest(reward.amount);
                break;
            case ItemType.AdvancedChest:
                ProfileManager.PlayerData.ResourceSave.AddAdvancedChest(reward.amount);
                break;
            case ItemType.ADTicket:
                ProfileManager.PlayerData.ResourceSave.AddADTicket(reward.amount);
                break;
            case ItemType.IncreaseProfit:
                if (reward.amount == 50) {
                    int id = ProfileManager.Instance.dataConfig.shopConfig.GetCardByOfferID(CardIapProductType.FINANCIAL_MANAGER_50).id;
                    ProfileManager.PlayerData.GetCardManager().AddCardIAPOneTime(id);
                } else if (reward.amount == 100) {
                    int id = ProfileManager.Instance.dataConfig.shopConfig.GetCardByOfferID(CardIapProductType.FINANCIAL_MANAGER_100).id;
                    ProfileManager.PlayerData.GetCardManager().AddCardIAPOneTime(id);
                }
                UpdateFinanceRate();
                break;
            case ItemType.TimeSkip_1H:
                ProfileManager.PlayerData.ResourceSave.AddTimeSkipTicket_1H(reward.amount);
                break;
            case ItemType.TimeSkip_4H:
                ProfileManager.PlayerData.ResourceSave.AddTimeSkipTicket_4H(reward.amount);
                break;
            case ItemType.TimeSkip_24H:
                ProfileManager.PlayerData.ResourceSave.AddTimeSkipTicket_24H(reward.amount);
                break;
            case ItemType.VIPMarketing:
                ProfileManager.PlayerData.GetMarketingManager().IsVIPActive = true;
                break;
            case ItemType.NormalSkinBox:
                ProfileManager.PlayerData.ResourceSave.AddNormalSkinBox(reward.amount);
                break;
            case ItemType.AdvancedSkinBox:
                ProfileManager.PlayerData.ResourceSave.AddAdvanceSkinBox(reward.amount);
                break;
            case ItemType.ExpertSkinBox:
                ProfileManager.PlayerData.ResourceSave.AddExpertSkinBox(reward.amount);
                break;
            case ItemType.Uniform:
                ProfileManager.PlayerData.skinManager.AddSkin(reward.skinID);
                break;
            case ItemType.Researcher:
                break;
            case ItemType.Reputation:
                ProfileManager.PlayerData.researchManager.AddResearchValue(reward.amount);
                break;
            case ItemType.Biscuit5:
                for (int i = 0; i < reward.amount; i++) {
                    ProfileManager.PlayerData.wareHouseManager.AddWareHouseMaterialSaves(WareHouseMaterialType.Biscuit, 5);
                }
                break;
            case ItemType.Candy5:
                for (int i = 0; i < reward.amount; i++) {
                    ProfileManager.PlayerData.wareHouseManager.AddWareHouseMaterialSaves(WareHouseMaterialType.Candy, 5);
                }
                break;
            case ItemType.Melon5:
                for (int i = 0; i < reward.amount; i++) {
                    ProfileManager.PlayerData.wareHouseManager.AddWareHouseMaterialSaves(WareHouseMaterialType.Melon, 5);
                }
                break;
            case ItemType.Potato5:
                for (int i = 0; i < reward.amount; i++) {
                    ProfileManager.PlayerData.wareHouseManager.AddWareHouseMaterialSaves(WareHouseMaterialType.Potato, 5);
                }
                break;
            case ItemType.Sushi5:
                for (int i = 0; i < reward.amount; i++) {
                    ProfileManager.PlayerData.wareHouseManager.AddWareHouseMaterialSaves(WareHouseMaterialType.Sushi, 5);
                }
                break;
        }
    }
    public bool IsRateGame() {
        return PlayerPrefs.GetInt("RateGame") == 1;
    }
    public void SaveRateGame() {
        PlayerPrefs.SetInt("RateGame", 1);
    }
    public void CheckShowRatePopup() {
        if (IsRateGame()) return;
        bool isEnableShow = false;
        if (!PlayerPrefs.HasKey("LastTimeShowRate")) {
            isEnableShow = true;
        } else {
            string last = PlayerPrefs.GetString("LastTimeShowRate");
            System.TimeSpan span = System.DateTime.Now.Subtract(System.DateTime.Parse(last));
            if (span.TotalHours >= 4) isEnableShow = true;
        }
        if (isEnableShow && !Tutorials.instance.IsShow) {
            PlayerPrefs.SetString("LastTimeShowRate", System.DateTime.Now.ToString());
            UIManager.instance.ShowPanelRate();
        }
    }
    void LoadRoomCostServer() {
        _RoomCostServer.Clear();
        string data = "";
        string dataVip = "";
        if (ProfileManager.PlayerData.selectedWorld == 1) {
            data = ABIFirebaseManager.Instance.m_FirebaseRemoteConfigManager.GetValues(ABI.Keys.key_remote_room_pirce_w1).StringValue;
        } else if (ProfileManager.PlayerData.selectedWorld == 2) {
            data = ABIFirebaseManager.Instance.m_FirebaseRemoteConfigManager.GetValues(ABI.Keys.key_remote_room_pirce_w2).StringValue;
            dataVip = ABIFirebaseManager.Instance.m_FirebaseRemoteConfigManager.GetValues(ABI.Keys.key_remote_room_vip_pirce_w2).StringValue;
        }
        if (!string.IsNullOrEmpty(data)) {
            string[] strCosts = data.Split(",");
            foreach (string str in strCosts) {
                _RoomCostServer.Add(int.Parse(str));
            }
        }
        if (!string.IsNullOrEmpty(dataVip)) {
            string[] strCosts = dataVip.Split(",");
            foreach (string str in strCosts) {
                _RoomVIPCostServer.Add(int.Parse(str));
            }
        }
    }
    [Button]
    void ApplyTargetBuildRooms() {
        ManagerRoom.OnApplyTargetBuildRoom();
        LobbyRoom.OnApplyTargetBuildRoom();
        KitchenRoom.OnApplyTargetBuildRoom();
        PowerRoom.OnApplyTargetBuildRoom();
        DeliverRoom.OnApplyTargetBuildRoom();
        foreach (var room in SmallTablesRoom) {
            room.OnApplyTargetBuildRoom();
        }
        foreach (var room in BigTablesRoom) {
            room.OnApplyTargetBuildRoom();
        }
    }
    float deltaTimeSleep;
    void UpdateTimeSleep() {
        if (IsSkipSleep) {
            deltaTimeSleep += Time.deltaTime;
            if (deltaTimeSleep >= 30 * 60) {
                deltaTimeSleep = 0;
                IsSkipSleep = false;
            }
        }
    }
    public int GetRewardValueForWareHouse(ItemType itemType) {

        switch (itemType) {
            case ItemType.Gem: {
                    int num = Random.Range(0, 4);
                    int num1 = 10 + num * 5;
                    return num1;
                }
            case ItemType.Cash: {
                    int num = GetCashProfit(15).ToIntValue();
                    num = (num / 500) * 500;
                    if (num < 5000) num = 5000;
                    return num;
                }
            case ItemType.Reputation: {
                    float num = Random.Range(1f, 2f);
                    return (int)(num * 70);
                }
        }
        return 0;
    }
    public int GetSkinDefaultPrice(int index) {
        int price = 200 * (index + (int)(System.MathF.Pow(2, index)));
        return price;
    }
    public int GetTotalReputationOffline() {
        // calculate research value offline
        int minutes = timeManager.GetOfflineTime() / 60;
        int turn = minutes / 3;
        int value = 0;
        int slot = 0;
        for (int i = 0; i < SmallTablesRoom.Length; i++) {
            if (IsUnlockSmallTable(i)) {
                slot += 2;
            }
        }
        for (int i = 0; i < BigTablesRoom.Length; i++) {
            if (IsUnlockBigTable(i)) {
                slot += 4;
            }
        }
        value = (int)(slot * turn * 0.6f);
        return value;
    }
    System.DateTime lastPauseTime = System.DateTime.Now;
    private void OnApplicationPause(bool pause) {
        if (pause) {
            timeManager.SaveData();
            lastPauseTime = System.DateTime.Now;
        } else {
            if (System.DateTime.Now.Subtract(lastPauseTime).TotalMinutes < 2) return;
            if (Tutorials.instance.IsActiveOnScene()) return;
            IsPauseGame = true;
            timeManager.LoadData();
            CalculateProfitOffline();
            IsPauseGame = false;
        }
    }
}
