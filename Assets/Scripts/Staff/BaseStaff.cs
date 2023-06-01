using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BaseStaff : MonoBehaviour {
    protected Vector3 TargetingPos; // current tartgeted object 
    public StateMachine<BaseStaff> StateMachine { get { return m_StateMachine; } }
    protected StateMachine<BaseStaff> m_StateMachine;
    public Animator m_Animator;
    public bool isFree;
    protected Vector3 defaultPos { get { return transform.parent.position; } }
    public TimeFill timeIndicator;
    protected Collider _Collider;
    public int _StaffIndex;
    [HideInInspector] public int _StaffGroupID;
    [HideInInspector] public SkinItem _SkinUsing = null;
    [HideInInspector] protected float _BuffSkillTiming = 0f;
    protected BaseStaffSkinSystem _BaseStaffSkinSystem;
    protected BaseStaffMoveSystem _BaseStaffMoveSystem;
    private void Awake() {
        _Collider = GetComponent<Collider>();
        _BaseStaffSkinSystem = GetComponent<BaseStaffSkinSystem>();
        _BaseStaffMoveSystem = GetComponent<BaseStaffMoveSystem>();
        InitStateMachine();
    }
    public virtual void Start() {
        if (timeIndicator) timeIndicator.Hide();
    }
    public virtual void OnCreated(int index) {
        _StaffIndex = index;
        if (_BaseStaffSkinSystem) _BaseStaffSkinSystem._StaffIndex = index;
        if (_BaseStaffMoveSystem != null) _BaseStaffMoveSystem.OnEnableAI();
        isFree = true;
        UpdateStaffSkin();
    }
    public virtual void Update() {
        if (GameManager.instance.IsPauseGame) return;
        if (_BuffSkillTiming > 0) _BuffSkillTiming -= Time.unscaledDeltaTime;
        m_StateMachine.Update();
    }

    public virtual void OnIdleStart() {
        if (_BaseStaffMoveSystem != null) {
            _BaseStaffMoveSystem.OnDisableAI();
            _BaseStaffMoveSystem.OnDisableMove();
        }
        isFree = true;
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
    }
    public virtual void OnIdleExecute() {

    }
    public virtual void OnIdleExit() {

    }
    public virtual void OnMoveStart() {
        isFree = false;
        if (_BaseStaffMoveSystem != null) {
            _BaseStaffMoveSystem.OnEnableAI();
            _BaseStaffMoveSystem.OnEnableMove();
            _BaseStaffMoveSystem.SetDestination(defaultPos);
        }
    }
    public virtual void OnMoveExecute() {
        if (IsFinishMoveOnNavemesh()) {
            transform.localEulerAngles = Vector3.zero;
            if (_BaseStaffMoveSystem != null) _BaseStaffMoveSystem.OnDisableMove();
            StateMachine.ChangeState(StaffIdleState.Instance);
        }
    }

    public virtual void OnFreeTimeEnd() { }
    public virtual void OnFreeTimeExecute() { }
    public virtual void OnFreeTimeStart() { }
    public virtual void OnMoveExit() { }
    public virtual void OnDoWorkStart() {
        if (_BaseStaffMoveSystem != null) _BaseStaffMoveSystem.OnEnableAI();
    }
    public virtual void OnDoWorkExecute() { }
    public virtual void OnDoWorkEnd() { }
    public virtual void OnNegotiateStart() { }
    public virtual void OnNegotiateExecute() { }
    public virtual void OnNegotiateExit() { }

    [HideInInspector]
    public float _SleepTime;
    public GameObject _SleepObject;
    public virtual void OnSleepStart() {
        _SleepTime = 0;
        _SleepObject.SetActive(true);
        isFree = false;
        if (_BaseStaffMoveSystem != null) _BaseStaffMoveSystem.OnDisableAI();
       m_Animator.SetBool("IsSleep", true);
        GameManager.instance.RegisterEmployeeSleep(transform);
    }
    public virtual void OnSleepExecute() { }
    public virtual void OnSleepExit() {
        GameManager.instance.UnRegisterEmployeeSleep(transform);
        m_Animator.SetBool("IsSleep", false);
        _SleepObject.SetActive(false);
        _SleepTime = 0;
    }

    public void OnEnableMove() {
        if (_BaseStaffMoveSystem != null) _BaseStaffMoveSystem.OnEnableMove();
    }
    public void OnDisableMove() {
        if (_BaseStaffMoveSystem != null) _BaseStaffMoveSystem.OnDisableMove();
    }

    protected virtual void InitStateMachine() {
        m_StateMachine = new StateMachine<BaseStaff>(this);
        m_StateMachine.SetCurrentState(StaffIdleState.Instance);
        m_StateMachine.SetGlobalState(StaffGlobalState.Instance);
    }

    public virtual void AIRotateToTarget(Vector3 target, bool isImmediate = false, float rotateSpeed = 400) {
        Vector3 pos = new Vector3(target.x, transform.position.y, target.z) - transform.position;
        if (pos != Vector3.zero) {
            var q = Quaternion.LookRotation(new Vector3(target.x, transform.position.y, target.z) - transform.position);
            if (!isImmediate)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotateSpeed * Time.deltaTime);
            else transform.rotation = q;
        }
    }

    public bool IsFinishMoveOnNavemesh() {
        if (_BaseStaffMoveSystem != null) return _BaseStaffMoveSystem.IsFinishMoveOnNavemesh();
        return false;
    }

    public void UpdateStaffSkin() {
        if (_BaseStaffSkinSystem != null) _BaseStaffSkinSystem.UpdateStaffSkin();
    }

    public bool IsEnableSleepState() {
        if (_SkinUsing != null && _SkinUsing.diligenceWork == DiligenceWork.Crazy) return false;
        return IsPassTutorialSleep();
    }
    public bool IsPassTutorialSleep() {
        if (ProfileManager.PlayerData.selectedWorld != 1) return true;
        return PlayerPrefs.GetInt("PassedTutorialSleep") == 1;
    }
    public void PassedTutorialSleep() {
        PlayerPrefs.SetInt("PassedTutorialSleep", 1);
    }
    protected float GetBuffWorkEfficiency() {
        if (_SkinUsing == null) return 1;
        return (100f - _SkinUsing.workEfficiency) / 100f;
    }
    protected float GetBuffSpeedEfficiencybySkin() {
        if (_SkinUsing == null) return 1;
        return (100f + _SkinUsing.workEfficiency) / 100f;
    }
    protected void CalculateWorkEfficiencyBySkin(ref float value) {
        if (_SkinUsing != null) {
            value *= GetBuffWorkEfficiency();
            int rdSkillChance = Random.Range(1, 100);
            if (_BuffSkillTiming > 0) {
                value /= 3;
            } else if (rdSkillChance < _SkinUsing.specialSkillChance) {
                _BuffSkillTiming = 30;
                value /= 3;
            }
        }
    }
    public void CalculateSkinBuffIncome(ref BigNumber value) {
        if (_SkinUsing != null) {
            int rdSkillChance = Random.Range(1, 100);
            if (rdSkillChance < _SkinUsing.incomeX2Chance) {
                value *= 2;
            }
        }
    }
    public bool IsEnableStartWork() {
        return isFree && StateMachine.CurrentState == StaffIdleState.Instance;
    }
    public bool IsCleanerEnableStartWork() {
        return isFree;
    }
    protected int GetChanceSleep() {
        if (_SkinUsing != null) {
            if (_SkinUsing.skinType == SkinType.GoldenSuit || _SkinUsing.skinType == SkinType.PremiumSuit || _SkinUsing.skinType == SkinType.Gem) return 0;
            else if (_SkinUsing.skinType == SkinType.Video) return 3;
        }
        return 7;
    }
  
}
public class BaseStaffSleep : State<BaseStaff> {
    private static BaseStaffSleep m_Instance;
    public static BaseStaffSleep Instance {
        get {
            if (m_Instance == null) {
                m_Instance = new BaseStaffSleep();
            }
            return m_Instance;
        }
    }
    public override void Enter(BaseStaff go) {
        go.OnSleepStart();
    }
    public override void Execute(BaseStaff go) {
        go.OnSleepExecute();
    }
    public override void Exit(BaseStaff go) {
        go.OnSleepExit();
    }
}
