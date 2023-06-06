using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerToLobbyState : State<Customer> {
    float waitTime;
    float deltaTime;
    Transform queuePosition;
    Customer customer;
    bool isMoveToQueue;
    bool IsWating = false;
    bool  isShowTalkWaitOrder;
    public override void Enter(Customer go) {
        customer = go;
        go.navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        queuePosition = LobbyRoomManager.instance.GetLobbyQueuePos(go);
        if (queuePosition == null) {
            go.StateMachine.ChangeState(go.customerOutRestaurant);
            return;
        }
        go.navMeshAgent.SetDestination(queuePosition.transform.position);
        go.OnEnableMove();
        isMoveToQueue = true;
        waitTime = LobbyRoomManager.instance.GetWaitingTimeMax();
        deltaTime = 0f;
        IsWating = false;
        isShowTalkWaitOrder = Random.Range(0, 100) >= 70;
        go.LobbyTarget = null;
    }
    public override void Execute(Customer go) {
        if (GameManager.instance.IsPauseGame) return;
        if (!go.navMeshAgent.enabled) return;
        if (isMoveToQueue && Tutorials.instance.IsRunStory) Time.timeScale = 1.5f;
        if (go.IsFinishMoveOnNavemesh()) {
            if (isMoveToQueue) {
                Time.timeScale = 1f;
                IsWating = true;
                isMoveToQueue = false;
                go.OnDisableMove();
                go.transform.localEulerAngles = queuePosition.transform.localEulerAngles;
                queuePosition = null;
            } else {
                if (IsWating) {
                    if (isShowTalkWaitOrder && !Tutorials.instance.IsRunStory) {
                        isShowTalkWaitOrder = false;
                        customer.ShowFunnyEmoji();
                    }
                    deltaTime += Time.deltaTime;
                    if (deltaTime >= waitTime) {
                        if (Random.Range(0, 100) >= 50) customer.ShowAngryEmoji();
                        deltaTime = 0;
                        LobbyRoomManager.instance.OutLobbyQueue(go);
                        if (go.LobbyTarget != null) go.LobbyTarget.Out();
                        go.StateMachine.ChangeState(go.customerOutRestaurant);
                        return;
                    } else {
                        if (go.LobbyTarget == null) {
                            go.LobbyTarget = LobbyRoomManager.instance.GetLobbyReceptionFreeTime(go);
                            if (go.LobbyTarget != null) {
                                LobbyRoomManager.instance.OutLobbyQueue(go);
                                go.navMeshAgent.SetDestination(go.LobbyTarget.transform.position);
                                go.OnEnableMove();
                            }
                        } else {
                            go.navMeshAgent.SetDestination(go.LobbyTarget.transform.position);
                            go.OnEnableMove();
                            go.StateMachine.ChangeState(go.customerOrderTable);
                        }
                    }
                }
            }
        }
    }

    public override void Exit(Customer go) {

    }
    public void UpdateQueuePos(Transform newQueue) {
        if (customer.LobbyTarget != null) return;
        queuePosition = newQueue;
        customer.navMeshAgent.SetDestination(queuePosition.position);
        customer.OnEnableMove();
        isMoveToQueue = true;
    }

}
