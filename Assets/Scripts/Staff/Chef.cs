using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chef : BaseStaff {
    Waiter waiterOrdered;
    float timeMakingFood;
    ChefManager ChefManager;
    public GetStaffTools staffTools;
    public override void OnCreated(int index) {
        base.OnCreated(index);
        staffTools.HideTool();
        m_Animator.SetBool("IsSleep", false);
        m_Animator.SetBool("IsCook", false);
        m_Animator.SetBool("IsWash", false);
        _SleepObject.SetActive(false);
        dtSleep = 60;

    }
    public override void Start() {
        base.Start();
        ChefManager = AllRoomManager.instance._ChefManagers;

    }
    float dtSleep;
    public override void Update() {
        base.Update();
        dtSleep += Time.deltaTime;
    }
    bool IsEnableSleep() {
        if (dtSleep >= 120) {
            dtSleep = 0;
            return true;
        }
        return false;
    }
    public void MakeFood(Waiter waiter) {
        transform.localEulerAngles = Vector3.zero;
        isFree = false;
        waiterOrdered = waiter;
        if (ChefManager == null) ChefManager = AllRoomManager.instance._ChefManagers;
        timeMakingFood = ChefManager.GetTimeMakeFood();
        if (Tutorials.instance.IsRunStory) timeMakingFood = 2;
        CalculateWorkEfficiencyBySkin(ref timeMakingFood);
        timeIndicator.InitTime(timeMakingFood);
        StartCoroutine(IMakingFood());
    }
    /// <summary>
    /// If Has Waiter Order, Food Is Serve for customer
    /// else food is server for delivery order task in order book
    /// </summary>
    /// <returns></returns>
    IEnumerator IMakingFood() {
        staffTools.ShowTool();
        m_Animator.SetBool("IsCook", true);
        yield return new WaitForSeconds(timeMakingFood);
        timeIndicator.Hide();
        if (waiterOrdered) {
            AllRoomManager.instance._ChefManagers.Payment(this);
            waiterOrdered.OnReceiveFoodFromChef();
        } else {
            ProfileManager.PlayerData.GetOrderBookManager().OnCollect();
        }
        m_Animator.SetBool("IsCook", false);
        staffTools.HideTool();
        yield return new WaitForSeconds(1);
        isFree = true;
        ChefManager.ChefFinishWork(this);
    }

    //======= OnIdle=================
    bool IsWashing;
    float deltaTimeWash;
    float deltaTimeIdle;
    public override void OnIdleStart() {
        base.OnIdleStart();
        deltaTimeIdle = 0;
    }
    public override void OnIdleExecute() {
        base.OnIdleExecute();
        if (!isFree || Tutorials.instance.IsRunStory) return;
        deltaTimeIdle += Time.deltaTime;
        if (deltaTimeIdle >= 2) {
            deltaTimeIdle = 0;
            int rd = Random.Range(0, 100);
            if (rd >= 90) {
                chefSinkNode = ChefManager.GetSinkNodeEmpty(this);
                if (chefSinkNode != null) {
                    SinkNodeTarget = chefSinkNode.transform.position;
                    SinkNodeAngle = chefSinkNode.transform.eulerAngles;
                    StateMachine.ChangeState(ChefFreeTime.Instance);
                }
                return;
            } else {
                if (Vector3.Distance(transform.position, defaultPos) > 0.2f) return;
                if (!IsPassTutorialSleep() && _StaffIndex == 0) {
                    StateMachine.ChangeState(BaseStaffSleep.Instance);
                    return;
                } else {
                    rd = Random.Range(1, 1000);
                    if (rd <= GetChanceSleep() && IsEnableSleepState() && !GameManager.instance.IsSkipSleep && IsEnableSleep()) {
                        StateMachine.ChangeState(BaseStaffSleep.Instance);
                        return;
                    }
                }
            }
        }
    }

    //======= OnFreeTime=================
    ChefSinkNode chefSinkNode;
    Vector3 SinkNodeTarget;
    Vector3 SinkNodeAngle;
    public override void OnFreeTimeStart() {
        base.OnFreeTimeStart();
        deltaTimeWash = 0;
        isFree = false;
        IsWashing = false;
        if (_BaseStaffMoveSystem) _BaseStaffMoveSystem.OnEnableAI();
        OnEnableMove();
        if (_BaseStaffMoveSystem) _BaseStaffMoveSystem.SetDestination(SinkNodeTarget);
    }

    public override void OnFreeTimeExecute() {
        base.OnFreeTimeExecute();
        if (IsWashing) {
            deltaTimeWash += Time.deltaTime;
            if (deltaTimeWash >= 5) {
                deltaTimeWash = 0;
                m_Animator.SetBool("IsWash", false);
                if (chefSinkNode != null) chefSinkNode.chef = null;
                StateMachine.ChangeState(StaffMoveState.Instance);
                return;
            }
        }
        if (IsFinishMoveOnNavemesh() && !IsWashing) {
            transform.eulerAngles = SinkNodeAngle;
            OnDisableMove();
            IsWashing = true;
            m_Animator.SetBool("IsWash", true);
        }

    }
    #region SLEEP STATE
    bool IsShowTut;
    public override void OnSleepStart() {
        base.OnSleepStart();
        staffTools.HideTool();
        m_Animator.SetBool("IsWash", false);
        m_Animator.SetBool("IsCook", false);
        _SleepObject.SetActive(true);
        IsShowTut = false;
        if (ProfileManager.PlayerData.GetSelectedWorld() > 1) PassedTutorialSleep();

    }
    public override void OnSleepExecute() {
        base.OnSleepExecute();
        if (!IsPassTutorialSleep() && !IsShowTut) {
            if (!UIManager.instance.isHasPopupOnScene && !Tutorials.instance.IsShow && !Tutorials.instance.IsRunStory) {
                IsShowTut = true;
                Tutorials.instance.ShowTutWakeupStaff();
            }
        }
        _SleepTime += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && _Collider != null && !Tutorials.instance.IsBlockInput) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (hit.collider != null) {
                if (hit.collider == _Collider) {
                    if (IsShowTut) {
                        IsShowTut = false;
                        PassedTutorialSleep();
                        Tutorials.instance.FinishTutorial();
                        Tutorials.instance.OnCloseTutorial();
                    }
                    StateMachine.ChangeState(StaffIdleState.Instance);
                }
            }
        }
        if (_SleepTime >= 120 && !IsShowTut || GameManager.instance.IsSkipSleep) {
            StateMachine.ChangeState(StaffIdleState.Instance);
        }
    }
    public override void OnSleepExit() {
        base.OnSleepExit();
    }
    #endregion SLEEP STATE
}
public class ChefFreeTime : State<BaseStaff> {
    private static ChefFreeTime m_Instance;
    public static ChefFreeTime Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new ChefFreeTime();
            }
            return m_Instance;
        }
    }
    public override void Enter(BaseStaff go) {
        go.OnFreeTimeStart();
    }
    public override void Execute(BaseStaff go) {
        go.OnFreeTimeExecute();
    }
    public override void Exit(BaseStaff go) {
        go.OnFreeTimeEnd();
    }
}

