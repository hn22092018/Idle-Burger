using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverStaff : BaseStaff {
    [HideInInspector]
    public DeliverCarCustomer customerOrder;
    float timeMakingFood = 3;
    public override void OnCreated(int index) {
        base.OnCreated(index);
        m_Animator.SetBool("IsWork", false);
    }

    public void MakeFood(DeliverCarCustomer customer) {
        isFree = false;
        customerOrder = customer;
        timeMakingFood = DeliverRoomManager.instance.GetTimeService();
        CalculateWorkEfficiencyBySkin(ref timeMakingFood);
        StartCoroutine(IMakeFood());
    }
    IEnumerator IMakeFood() {
        yield return new WaitForSeconds(1f);
        StateMachine.ChangeState(StaffDoWork.Instance);
    }
    DeliverCakePosition cakePosition;
    bool IsHasCake;
    float dtTimeTakeCake;
    float dtDeliverCake;
    public override void OnDoWorkStart() {
        base.OnDoWorkStart();
        cakePosition = null;
        IsHasCake = false;
        dtTimeTakeCake = 0;
        dtDeliverCake = 0;
    }
    public override void OnDoWorkExecute() {
        base.OnDoWorkExecute();

        if (!IsHasCake) {
            if (cakePosition == null) {
                cakePosition = DeliverRoomManager.instance.GetCakePostionIndexEmpty(this);
                if (cakePosition != null) {
                    OnEnableMove();
                    if (_BaseStaffMoveSystem) _BaseStaffMoveSystem.SetDestination(cakePosition.transform.position);
                }
            } else {
                if (IsFinishMoveOnNavemesh()) {
                    transform.eulerAngles = cakePosition.transform.eulerAngles;
                    OnDisableMove();
                    if (!timeIndicator.gameObject.activeInHierarchy) timeIndicator.InitTime(timeMakingFood / 2);
                    dtTimeTakeCake += Time.deltaTime;
                    if (dtTimeTakeCake >= timeMakingFood / 2) {
                        OnEnableMove();
                        IsHasCake = true;
                        if (_BaseStaffMoveSystem) _BaseStaffMoveSystem.SetDestination(defaultPos);
                        timeIndicator.Hide();
                        DeliverRoomManager.instance.OutCakePosition(this);
                    }
                }
            }
        } else {
            if (IsFinishMoveOnNavemesh()) {
                transform.localEulerAngles = Vector3.zero;
                OnDisableMove();
                if (!timeIndicator.gameObject.activeInHierarchy) timeIndicator.InitTime(timeMakingFood / 2);
                dtDeliverCake += Time.deltaTime;
                if (dtDeliverCake >= timeMakingFood / 2) {
                    timeIndicator.Hide();
                    customerOrder.OnReceiveFoodFromStaff();
                    StateMachine.ChangeState(StaffIdleState.Instance);
                }
            }
        }

    }
    public override void OnDoWorkEnd() {
        base.OnDoWorkEnd();
    }
    IEnumerator IMakingFood() {
        m_Animator.SetBool("IsWork", true);
        yield return new WaitForSeconds(timeMakingFood);
        PaymentOrder();
        m_Animator.SetBool("IsWork", false);
        timeIndicator.Hide();
        customerOrder.OnReceiveFoodFromStaff();
        yield return new WaitForSeconds(0.5f);
        isFree = true;
    }
    BigNumber orderValue;
    public void PaymentOrder() {
        DropReputation();
        orderValue = customerOrder.GetOrderFoodValue() * GameManager.instance.GetTotalIncomeRate();
        CalculateSkinBuffIncome(ref orderValue);
        ProfileManager.PlayerData.AddCash(orderValue);
        //UIManager.instance.CreatUIMoneyEff(drinkValue, transform);
    }
}
