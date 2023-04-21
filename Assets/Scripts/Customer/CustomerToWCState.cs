using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerToWCState : State<Customer> {
    Customer customer;
    bool IsPayment;
    WCManager WCManager;
    List<WCManager> listWCManagers = new List<WCManager>();
    public override void Enter(Customer go) {
        IsPayment = false;
        customer = go;
        customer.deltaTime = 0;
        customer.IsWCRunning = false;
        customer.navMeshAgent.enabled = true;
        listWCManagers = AllRoomManager.instance._RestroomManagers;
        listWCManagers.Sort((x, y) => Vector3.Distance(x.currentRoom.GetTransform().position, go.transform.position).CompareTo(Vector3.Distance(y.currentRoom.GetTransform().position, go.transform.position)));
        foreach (var manager in listWCManagers) {
            customer.WcPositionTarget = manager.GetPostionEmpty(customer);
            if (customer.WcPositionTarget != null) {
                WCManager = manager;
                customer.timeWC = WCManager.GetTimeService();
                customer.OnEnableMove();
                customer.navMeshAgent.SetDestination(customer.WcPositionTarget.transform.position);
                return;
            }
        }
        NextRoomWhenOutWC();
    }

    public override void Execute(Customer go) {
        if (GameManager.instance.IsPauseGame) return;
        customer = go;
        CheckReplaceWCTarget(go);
        if (customer.IsWCRunning) {
            UpdateUseWC();
            return;
        }
        if (customer.IsFinishMoveOnNavemesh()) {
            customer.OnDisableMove();
            if (!customer.IsWCRunning) {
                customer.lastPosInNavmesh = customer.transform.position;
                customer.navMeshAgent.enabled = false;
                customer.IsWCRunning = true;
                customer.OnDisableMove();
                customer.timeIndicator.InitTime(customer.timeWC);
                customer.deltaTime = 0;
                customer.transform.position = customer.WcPositionTarget.transform.position;
                customer.transform.eulerAngles = customer.WcPositionTarget.transform.eulerAngles;
                customer.m_Animator.SetBool("IsWC", true);
            }

        }
    }
    void CheckReplaceWCTarget(Customer go) {
        if (go.WcPositionTarget.transform == null) {
            Debug.Log("Replace WC Target if player upgrade model");
            go.WcPositionTarget = WCManager.ReplaceByNearestPostionEmpty(go);

        }
    }
    void UpdateUseWC() {
        customer.deltaTime += Time.deltaTime;
        if (customer.deltaTime >= customer.timeWC) {
            customer.m_Animator.SetBool("IsWC", false);
            customer.transform.position = customer.lastPosInNavmesh;
            customer.navMeshAgent.enabled = true;
            customer.timeIndicator.Hide();
            if (!IsPayment) {
                IsPayment = true;
                WCManager.Payment(customer.WcPositionTarget);
            }
            WCManager.OutPosition(customer);
            NextRoomWhenOutWC();
            return;
        }
    }

    void NextRoomWhenOutWC() {
        customer.StateMachine.ChangeState(customer.customerOutRestaurant);
    }
    public override void Exit(Customer go) {
        go.m_Animator.SetBool("IsWC", false);
        go.WcPositionTarget = null;
    }
}
