using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUseFoodState : State<Customer> {
    Customer customer;
    bool isStartUseFood;
    bool IsPaymentTable;
    WaiterManager WaiterManager;

    public override void Enter(Customer go) {
        IsPaymentTable = false;
        customer = go;
        go.TablePositionTarget = TableManager.instance.GetTablePosition(go);
        go.OwnerRoomID = go.TablePositionTarget.OwnerRoomID;
        go.OwnerGroupID = go.TablePositionTarget.OwnerGroupID;
        WaiterManager = AllRoomManager.instance.GetWaiterManager(go.OwnerGroupID);
        go.OnEnableMove();
        go.IsOnTable = false;
        go.deltaTime = 0;
        go.OnDisableSit();
        go.IsStartUseFood = false;
        if (go.IsUseFoodStateOnInit) {
            go.IsUseFoodStateOnInit = false;
            go.transform.position = go.TablePositionTarget.chairTransform.position;
            SitInTable();
        } else go.navMeshAgent.SetDestination(go.TablePositionTarget.chairTransform.position);
        isStartUseFood = false;
    }

    public override void Execute(Customer go) {
        if (GameManager.instance.IsPauseGame) return;
        customer = go;
        if (customer.ui_foodOrder) customer.ui_foodOrder.UpdateAngle();
        if (!go.IsOnTable) {
            if (go.IsFinishMoveOnNavemesh()) {
                go.IsOnTable = true;
                SitInTable();
            }
        } else if (go.IsStartUseFood) {
            // using food
            OnUseFoodUpdate();
        }
    }
    void OnUseFoodUpdate() {
        if (!isStartUseFood) {
            isStartUseFood = true;
            customer.StartCoroutine(IShowEmojiFood());
            if (customer.ui_foodOrder) {
                customer.ui_foodOrder.gameObject.SetActive(false);
            }
        }
        customer.deltaTime += Time.deltaTime;
        if (customer.deltaTime >= customer.timeUseFood) {
            if (Tutorials.instance.IsRunStory) {
                Tutorials.instance.IsRunStory = false;
            }
            if (!IsPaymentTable) {
                IsPaymentTable = true;
                TableManager.instance.PaymentTable(customer.TablePositionTarget);
                if (Random.Range(0, 100) >= 50) customer.ShowFunnyEmoji();
            }
            customer.timeIndicator.Hide();
            customer.transform.position = customer.TablePositionTarget.outTransform.position;
            customer.TablePositionTarget.OnStopUseFood();
            customer.OnDisableSit();
            if (GameManager.instance.IsUnlockRestroom(customer.OwnerGroupID)) {
                customer.StateMachine.ChangeState(customer.customerToWC);
            } else {
                customer.StateMachine.ChangeState(customer.customerOutRestaurant);
            }
            return;
        }

    }
    /// <summary>
    ///  Set Customer Position In Table Setting. Cache last position in navmesh
    /// </summary>
    void SitInTable() {
        // cache last navmesh position
        customer.lastPosInNavmesh = customer.transform.position;
        customer.navMeshAgent.enabled = false;
        customer.IsOnTable = true;
        customer.transform.position = customer.TablePositionTarget.chairTransform.position;
        customer.transform.eulerAngles = customer.TablePositionTarget.chairTransform.eulerAngles;
        customer.OnDisableMove();
        customer.OnEnableSit();
        customer.StartCoroutine(CallWaiter());
        customer.StartCoroutine(IShowEmojiFood());
    }
    IEnumerator IShowEmojiFood() {

        yield return new WaitForSeconds(Random.Range(0.5f, 2.5f));
        if (Random.Range(0, 100) >= 70) {
            if (Random.Range(0, 100) >= 70) customer.ShowFunnyEmoji();
            else customer.ShowSadEmoji();
        }
        yield return new WaitForSeconds(Random.Range(2f, 3f));
        if (Random.Range(0, 100) >= 80) {
            if (Random.Range(0, 100) >= 70) customer.ShowFunnyEmoji();
            else customer.ShowSadEmoji();
        }
    }

    public override void Exit(Customer go) {
        //go.OnCustomerUseFoodExit();
        go.TablePositionTarget = null;
    }
    IEnumerator CallWaiter() {
        if (Tutorials.instance.IsRunStory) {
            Tutorials.instance.IsReadyShowTutHireStaff = true;
            while (!Tutorials.instance.IsFinishIntroKitchen) {
                yield return new WaitForEndOfFrame();
            }
            customer.foodOrder = ProfileManager.PlayerData.researchManager.GetTutorialFood();
            customer.TablePositionTarget.SetFoodID((int)customer.foodOrder.researchName);
            WaiterManager.AddTablePositionCallWaiter(customer.TablePositionTarget);
            if (customer.ui_foodOrder) {
                customer.ui_foodOrder.gameObject.SetActive(true);
                customer.ui_foodOrder.ShowIcon(customer.foodOrder.icon);
            }
            Transform t_Chef = GameManager.instance.KitchenRoom.staffSetting.listStaffTrans[0];
            while (!t_Chef.GetComponent<Chef>().isFree) {
                yield return new WaitForEndOfFrame();
            }
            Transform t_Waiter = GameManager.instance.SmallTablesRoom[0].staffSetting.listStaffTrans[0];
            CameraMove.instance.SetTargetFollow(t_Waiter);
            while (!customer.IsStartUseFood) {
                yield return new WaitForEndOfFrame();
            }
            customer.timeUseFood = 5;
            customer.timeIndicator.InitTime(5);
            CameraMove.instance.SetTargetFollow(customer.transform);

        } else {
            customer.StartCoroutine(OrderFood());
        }
    }
    /// <summary>
    ///  Order Food, If Food Order Is Not Reseach, Wait in 6s and check again
    ///  Out Restaurant if Food Order Is Not Reseach
    /// </summary>
    /// <returns></returns>
    IEnumerator OrderFood() {
        // call food
        customer.foodOrder = ProfileManager.PlayerData.researchManager.GetRandomFood();
        if (customer.ui_foodOrder) {
            customer.ui_foodOrder.gameObject.SetActive(true);
            customer.ui_foodOrder.ShowIcon(customer.foodOrder.icon);
        }
        customer.TablePositionTarget.SetFoodID((int)customer.foodOrder.researchName);
        // check food order is research or not
        bool IsUnlockFood = ProfileManager.PlayerData.researchManager.GetLevelByName(customer.foodOrder.researchName) > 0;
        customer.deltaTime = 0;
        while (customer.deltaTime <= 6f && !IsUnlockFood) {
            yield return new WaitForSeconds(2);
            customer.deltaTime += 2;
            IsUnlockFood = ProfileManager.PlayerData.researchManager.GetLevelByName(customer.foodOrder.researchName) > 0;
        }
        customer.deltaTime = 0;
        if (IsUnlockFood) {
            WaiterManager.AddTablePositionCallWaiter(customer.TablePositionTarget);
            customer.timeUseFood = TableManager.instance.GetTimeServiceOnTable(customer.TablePositionTarget);
            customer.timeUseFood += customer.foodOrder.time;

        } else {
            // Out Restaurant
            customer.TablePositionTarget.OnStopUseFood();
            customer.OnDisableSit();
            if (customer.ui_foodOrder) {
                customer.ui_foodOrder.gameObject.SetActive(false);
            }
            customer.ShowAngryEmoji();
            customer.StateMachine.ChangeState(customer.customerOutRestaurant);
        }


    }
}
