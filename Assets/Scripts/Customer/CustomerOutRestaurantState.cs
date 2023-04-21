using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerOutRestaurantState : State<Customer> {
    int outIndex;
    List<Vector3> outPos = new List<Vector3>();
    public override void Enter(Customer go) {
        outIndex = 0;
        go.OnEnableMove();
        go.navMeshAgent.enabled = true;
        outPos.Clear();
        if (go.StateMachine.PreviousState == go.customerToLobby || go.StateMachine.PreviousState == go.customerOrderTable)
            outPos.Add(GameManager.instance.GetOutDoorPos()[0].position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)));
        else outPos.Add(GameManager.instance.GetOutDoorPos()[1].position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)));
        outPos.Add(GameManager.instance.GetDespawnPos());
        go.navMeshAgent.SetDestination(outPos[outIndex]);
    }
    public override void Execute(Customer go) {
        if (GameManager.instance.IsPauseGame) return;
        if (go.IsFinishMoveOnNavemesh()) {
            if (outIndex < outPos.Count - 1) {
                outIndex++;
                go.OffEmojiIcon();
                go.navMeshAgent.SetDestination(outPos[outIndex]);
            } else {
                PoolManager.Pools["GameEntity"].Despawn(go.transform);
            }
            return;
        }
    }
    public override void Exit(Customer go) {

    }
}
