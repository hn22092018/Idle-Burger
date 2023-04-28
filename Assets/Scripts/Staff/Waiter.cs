using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiter : BaseStaff {
    public GameObject foodOnHand;
    public TablePosition servingTable;
    public ICustomer servingCustomer;
    float deltaTime;
    WaiterManager WaiterManager;
    public Dictionary<int, GameObject> foodDic = new Dictionary<int, GameObject>();
    public override void OnCreated(int index) {
        base.OnCreated(index);
        foodOnHand.SetActive(false);
        m_Animator.SetBool("IsSleep", false);
        m_Animator.SetBool("IsBringFood", false);
        _SleepObject.SetActive(false);
        dtSleep = 60;
    }
    public override void Start() {
        base.Start();
        WaiterManager = AllRoomManager.instance.GetWaiterManager(_StaffGroupID);
    }
    float dtSleep;
    public override void Update() {
        base.Update();
        dtSleep += Time.deltaTime;
    }
    bool IsEnableSleep() {
        if (Vector3.Distance(transform.position, defaultPos) > 0.2f) return false;
        if (dtSleep >= 120) {
            dtSleep = 0;
            return true;
        }
        return false;
    }
    void UpdateSpeed(bool IsMultiplySkin = false) {
        if (_BaseStaffMoveSystem) {
            _BaseStaffMoveSystem.MultiplySpeedAI(GameManager.instance.waiterSpeedRate + WaiterManager.GetManagerRateProcessing() * 0.7f);
            if (IsMultiplySkin) _BaseStaffMoveSystem.MultiplySpeedAI(GetBuffSpeedEfficiencybySkin());
        }

    }

    public void OnReceiveTable(TablePosition tablePosition) {
        isFree = false;
        servingTable = tablePosition;
        servingCustomer = servingTable.customer;
    }

    public void OnReceiveFoodFromChef() {
        LoadFood();
        StateMachine.ChangeState(StaffDoWork.Instance);
    }
    GameObject currentFood;
    void LoadFood() {
        currentFood = null;
        if (!foodDic.ContainsKey(servingTable.CurrentFoodID)) {
            currentFood = CreatFood(servingTable.CurrentFoodID, foodOnHand.transform);
            foodDic.Add(servingTable.CurrentFoodID, currentFood);
            currentFood.gameObject.SetActive(true);
            currentFood.transform.localPosition = Vector3.zero;
        } else {
            currentFood = foodDic[servingTable.CurrentFoodID];
            currentFood.SetActive(true);
        }
    }
    public GameObject CreatFood(int id, Transform parent) {
        Research research = ProfileManager.Instance.dataConfig.researchDataConfig.GetResearchByID(id);
        if (research != null) return Instantiate(research.objFood, parent).gameObject;
        return null;
    }
    #region IDLE STATE
    float deltaTimeIdle;
    public override void OnIdleStart() {
        base.OnIdleStart();
        WaiterManager.OnCheckWork();
    }
    public override void OnIdleExecute() {
        base.OnIdleExecute();
        if (!isFree) return;
        deltaTimeIdle += Time.deltaTime;
        if (deltaTimeIdle >= 3) {
            deltaTimeIdle = 0;
            int rd = Random.Range(0, 1000);
            if (rd <= GetChanceSleep() && IsEnableSleepState() && !GameManager.instance.IsSkipSleep && IsEnableSleep()) {
                StateMachine.ChangeState(BaseStaffSleep.Instance);
                return;
            }
        }
    }

    #endregion IDLE STATE

    #region ServingFood State

    public override void OnDoWorkStart() {
        base.OnDoWorkStart();
        UpdateSpeed(true);
        isFree = false;
        if (_BaseStaffMoveSystem) _BaseStaffMoveSystem.SetDestination(servingTable.serverTransform.position);
        OnEnableMove();
        deltaTime = 0;
        m_Animator.SetBool("IsBringFood", true);
        foodOnHand.SetActive(true);
    }
    public override void OnDoWorkExecute() {
        base.OnDoWorkExecute();
        if (IsFinishMoveOnNavemesh()) {
            OnDisableMove();
            if (deltaTime < 0.5f) {
                if (servingCustomer != null) AIRotateToTarget(servingCustomer.GetTransform().position);
                deltaTime += Time.deltaTime;
            } else {
                DropReputation();
                WaiterManager.PaymentFoodOrder(this);
                m_Animator.SetBool("IsBringFood", false);
                foodOnHand.SetActive(false);
                servingTable.OnUseFood();
                StateMachine.ChangeState(StaffMoveState.Instance);
            }
        }
    }
    #endregion ServingFood State

    #region SLEEP STATE
    public override void OnSleepStart() {
        base.OnSleepStart();
        foodOnHand.SetActive(false);
        m_Animator.SetBool("IsBringFood", false);
    }

    public override void OnSleepExecute() {
        base.OnSleepExecute();
        _SleepTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && _Collider != null) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (hit.collider != null) {
                if (hit.collider == _Collider) {
                    StateMachine.ChangeState(StaffIdleState.Instance);
                }
            }
        }
        if (_SleepTime >= 120 || GameManager.instance.IsSkipSleep) {
            StateMachine.ChangeState(StaffIdleState.Instance);
        }
    }
    public override void OnSleepExit() {
        base.OnSleepExit();
    }
    #endregion SLEEP STATE

}

