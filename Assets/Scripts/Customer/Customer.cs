using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public interface ICustomer {
    public void OnStartUseFood();
    public Transform GetTransform();
    public float GetVipRate() {
        return 1f;
    }
    public float GetOrderFoodValue() {
        return 0f;
    }
    public bool GetIsOnTable();
}
public class Customer : MonoBehaviour, ICustomer {
    public StateMachine<Customer> StateMachine { get { return m_StateMachine; } }

    public WCPosition WcPositionTarget { get => wcPositionTarget; set => wcPositionTarget = value; }
    public LobbyPosition LobbyTarget { get => lobbyTarget; set => lobbyTarget = value; }
    public TablePosition TablePositionTarget { get => tablePositionTarget; set => tablePositionTarget = value; }

    protected StateMachine<Customer> m_StateMachine;
    public NavMeshAgent navMeshAgent;
    [Header("Movement")]
    public float m_MoveSpeed = 4;

    public TimeFill timeIndicator;
    [SerializeField] UIEmojiIcon emoji_Icon;
    public UIFoodOrderIcon ui_foodOrder;
    public Animator m_Animator;
    [HideInInspector] public bool IsMale;

    bool IsMoving;
    [HideInInspector] public Vector3 lastPosInNavmesh;
    [HideInInspector] public float timeUseFood = 10;
    [HideInInspector] public float timeWC = 5;
    [HideInInspector] public float timeBar = 30;
    [HideInInspector] public float deltaTime;
    bool IsSit;
    private LobbyPosition lobbyTarget = null;

    private TablePosition tablePositionTarget;
    [HideInInspector] public bool IsOnTable;
    [HideInInspector] public bool IsStartUseFood;
    private WCPosition wcPositionTarget = null;
    [HideInInspector] public bool IsWCRunning;

    public CustomerToLobbyState customerToLobby = new CustomerToLobbyState();
    public CustomerOrderTableState customerOrderTable = new CustomerOrderTableState();
    public CustomerUseFoodState customerUseFood = new CustomerUseFoodState();
    public CustomerToWCState customerToWC = new CustomerToWCState();
    public CustomerOutRestaurantState customerOutRestaurant = new CustomerOutRestaurantState();
    public CustomerDefaultMoveState customerDefault = new CustomerDefaultMoveState();
    [HideInInspector] public RoomID OwnerRoomID;
    [HideInInspector] public int OwnerGroupID;
    [HideInInspector] public Research foodOrder;
    private void Awake() {
        m_StateMachine = new StateMachine<Customer>(this);
        m_StateMachine.SetCurrentState(customerToLobby);
    }
    public virtual void Update() {
        if (GameManager.instance.IsPauseGame) return;
        m_StateMachine.Update();
    }
    public void UpdateAnimator() {
        m_Animator.SetBool("IsMove", IsMoving);
        m_Animator.SetBool("IsSit", IsSit);
    }
    public void OnEnableSit() {
        IsSit = true;
        m_Animator.SetBool("IsSit", IsSit);
    }
    public void OnDisableSit() {
        IsSit = false;
        m_Animator.SetBool("IsSit", IsSit);
    }
    public void OnEnableMove() {
        IsMoving = true;
        m_Animator.SetBool("IsMove", IsMoving);
    }
    public void OnDisableMove() {
        IsMoving = false;
        m_Animator.SetBool("IsMove", IsMoving);
    }
    public virtual void AIRotateToTarget2(Transform target) {
        transform.rotation = target.transform.rotation;
    }
    public void InitCustomer(bool isVip = false) {
        GameManager.instance.RegisterCustomerInRes(this);
        IsMale = Random.Range(0, 2) == 0;
        m_Animator.GetComponent<RandomCustomerMesh>().RandomMesh();
        IsMoving = false;
        IsSit = false;
        navMeshAgent.speed = m_MoveSpeed;
        if (m_Animator) m_Animator.SetBool("IsMale", IsMale);
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        navMeshAgent.enabled = true;
        emoji_Icon.gameObject.SetActive(false);
        ui_foodOrder.gameObject.SetActive(false);
        timeIndicator.Init();
        UpdateAnimator();
    }
    public void UpdateMoveSpeed(float speedRate) {
        navMeshAgent.speed = m_MoveSpeed * speedRate;
    }
    public void OnNotComeToRestaurant() {
        m_StateMachine.ChangeState(customerDefault);
    }
    public void OnComeToRestaurant() {
        StateMachine.ChangeState(customerToLobby);
    }
    [HideInInspector] public bool IsUseFoodStateOnInit;
    public void OnCustomerUseFoodStateOnInit() {
        IsUseFoodStateOnInit = true;
        m_StateMachine.ChangeState(customerUseFood);
    }

    public void OnStartUseFood() {
        IsStartUseFood = true;
        deltaTime = 0;
        timeIndicator.InitTime(timeUseFood);
        m_Animator.SetTrigger("IsEat");
    }

    public void ShowFunnyEmoji() {
        emoji_Icon.ShowFunnyEmoji();
        emoji_Icon.gameObject.SetActive(true);
    }
    public void ShowSadEmoji() {
        emoji_Icon.ShowSadEmoji();
        emoji_Icon.gameObject.SetActive(true);
    }
    public void ShowAngryEmoji() {
        emoji_Icon.ShowAngryEmoji();
        emoji_Icon.gameObject.SetActive(true);
    }
    public void OffEmojiIcon() {
        emoji_Icon.gameObject.SetActive(false);
    }
    public void ForceOutRestaurant() {
        if (StateMachine.CurrentState == customerDefault) return;
        StopAllCoroutines();
        m_Animator.SetBool("IsDance", false);
        OnDisableSit();
        if (!navMeshAgent.enabled) transform.position = lastPosInNavmesh;
        emoji_Icon.gameObject.SetActive(false);
        ui_foodOrder.gameObject.SetActive(false);
        timeIndicator.Hide();
        StateMachine.ChangeState(customerDefault);

    }
    public bool IsFinishMoveOnNavemesh() {
        if (!navMeshAgent.pathPending) {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) {
                    return true;
                }
            }
        }
        return false;
    }
    public Transform GetTransform() {
        return transform;
    }
    public bool GetIsOnTable() {
        return IsOnTable;
    }
    public float GetOrderFoodValue() {
        int level = ProfileManager.PlayerData.researchManager.GetLevelByName(foodOrder.researchType);
        if (foodOrder != null && level > 0)
            return (float)foodOrder.CalculateProfit(level);
        return 0;
    }

}



