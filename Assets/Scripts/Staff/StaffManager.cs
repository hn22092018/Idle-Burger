using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : BaseStaff {
    public Transform[] targetMoves;
    Vector3 currentTarget = Vector3.zero;
    float idleTime=2;
    float dtTimeIdle;
    public void InitStaff(Transform[] targets) {
        targetMoves = targets;
        if (_BaseStaffMoveSystem) _BaseStaffMoveSystem.OnDisableAI();
        transform.position= targetMoves[Random.Range(0, targetMoves.Length)].position;
    }
    public override void OnIdleStart() {
        if (_BaseStaffMoveSystem) _BaseStaffMoveSystem.OnDisableAI();
        idleTime = Random.Range(2, 4);
        dtTimeIdle = 0;
        OnDisableMove();
    }
    public override void OnIdleExecute() {
        base.OnIdleExecute();
        dtTimeIdle += Time.deltaTime;
        if (dtTimeIdle >= idleTime) {
            dtTimeIdle = 0;
            StateMachine.ChangeState(StaffMoveState.Instance);
        }
    }

    public override void OnMoveStart() {
        Vector3 newTarget = targetMoves[Random.Range(0, targetMoves.Length)].position;
        if(Vector3.Distance(currentTarget, newTarget)<=0.1f) {
            StateMachine.ChangeState(StaffIdleState.Instance);
            OnDisableMove();
            return;
        } else {
            if (_BaseStaffMoveSystem) _BaseStaffMoveSystem.OnEnableAI();
            OnEnableMove();
            currentTarget = newTarget;
            if (_BaseStaffMoveSystem) _BaseStaffMoveSystem.SetDestination(currentTarget);
        }
    }
    public override void OnMoveExecute() {
        if (IsFinishMoveOnNavemesh()) {
            StateMachine.ChangeState(StaffIdleState.Instance);
            return;
        }
    }
}
