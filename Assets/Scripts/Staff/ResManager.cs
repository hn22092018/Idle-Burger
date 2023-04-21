using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager : BaseStaff {
    Transform targetMove;
    float idleTime;
    float deltaTime;
    bool isSit;
    Vector3 lastNavmeshPos;


    public override void Start() {
        base.Start();
        lastNavmeshPos = defaultPos;
        ManagerRoom.instance.currentManagerStaff = this;
    }
    public override void Update() {
        base.Update();
    }
    void OnEnableSit() {
        isSit = true;
        m_Animator.SetBool("IsSit", isSit);
    }
    void OnDisableSit() {
        isSit = false;
        m_Animator.SetBool("IsSit", isSit);
    }
    public void OnEnableTalk() {
        m_Animator.SetTrigger("IsTalk");
    }
   
    int indexChair = -1;
    int indexMove = -1;
    #region OnMOve
    public override void OnMoveStart() {
        if (_BaseStaffMoveSystem) _BaseStaffMoveSystem.OnEnableAI();
        OnDisableSit();
        isSit = Random.Range(0, 100) > 60;
        if (!isSit) {
            int newMoveIndex = Random.Range(0, ManagerRoom.instance.movePositions.Length);
            if (indexMove == newMoveIndex) {
                isSit = true;
                int newChairIndex = Random.Range(0, ManagerRoom.instance.chairPosition.Length);
                targetMove = ManagerRoom.instance.chairPosition[newChairIndex];
            } else {
                indexMove = newMoveIndex;
                targetMove = ManagerRoom.instance.movePositions[indexMove];
            }

        } else {
            int newChairIndex = Random.Range(0, ManagerRoom.instance.chairPosition.Length);
            if (indexChair == newChairIndex) {
                isSit = false;
                int newMoveIndex = Random.Range(0, ManagerRoom.instance.movePositions.Length);
                targetMove = ManagerRoom.instance.movePositions[newMoveIndex];
            } else {
                indexChair = newChairIndex;
                targetMove = ManagerRoom.instance.chairPosition[indexChair];
            }
        }
        if (_BaseStaffMoveSystem)  _BaseStaffMoveSystem.SetDestination(targetMove.position);
        OnEnableMove();
    }
    public override void OnMoveExecute() {
        if (IsFinishMoveOnNavemesh()) {
            if (_BaseStaffMoveSystem) _BaseStaffMoveSystem.OnDisableAI();
           lastNavmeshPos = transform.position;
            OnDisableMove();
            StateMachine.ChangeState(StaffIdleState.Instance);
            return;
        }
    }

    #endregion OnMove

    #region OnIdle
    public override void OnIdleStart() {
        idleTime = Random.Range(2, 5);
        deltaTime = 0;
        OnDisableMove();
        if (isSit) {
            transform.position = targetMove.transform.position;
            transform.rotation = targetMove.rotation;
            m_Animator.SetBool("IsSit", true);
            if (indexChair == 0) {
                m_Animator.SetTrigger("IsWork");
                idleTime = 10f;
            } else {
                idleTime = 5f;
            }
        }
    }
    public override void OnIdleExecute() {
        deltaTime += Time.deltaTime;
        if (deltaTime >= idleTime) {
            transform.position = lastNavmeshPos;
            StateMachine.ChangeState(StaffMoveState.Instance);
            return;
        }
    }

    #endregion OnIdle

}

