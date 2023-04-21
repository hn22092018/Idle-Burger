using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerDefaultMoveState : State<Customer> {
    bool IsCheck;
    public override void Enter(Customer go) {
        IsCheck = false;
        IsDespawn = false;
        go.OnEnableMove();
        go.navMeshAgent.enabled = true;
        Vector3 target = GameManager.instance.GetDespawnPos();
        if (go.navMeshAgent.isOnNavMesh == false) {
        } else if (go.navMeshAgent.enabled) go.navMeshAgent.SetDestination(target);
    }
    bool IsDespawn;
    public override void Execute(Customer go) {
        if (GameManager.instance.IsPauseGame) return;
        if (LobbyRoomManager.instance.IsHasLobbyQueueEmpty()) {
            if (!IsCheck) {
                IsCheck = true;
                if (Random.Range(0, 10) >= 5) go.StateMachine.ChangeState(go.customerToLobby);
            }
            return;
        }
        if (go.IsFinishMoveOnNavemesh() && !IsDespawn) {
            IsDespawn = true;
            GameManager.instance.UnRegisterCustomerInRes(go);
            PoolManager.Pools["GameEntity"].Despawn(go.transform);
            return;
        }
    }
    public override void Exit(Customer go) {
    }

}
