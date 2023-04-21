using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderTableState : State<Customer> {

    /// <summary>
    ///  Order Table.
    ///  if has table empty, move to table
    ///  else out restaurant
    /// </summary>
    Customer customer;
    public override void Enter(Customer go) {
        customer = go;
        customer.OnDisableMove();
        customer.StartCoroutine(IWaitProcessOrder());
    }
    IEnumerator IWaitProcessOrder() {
        customer.m_Animator.SetTrigger("IsTalk");
        if (Tutorials.instance.IsRunStory) {
            while (!GameManager.instance.IsUnlockSmallTable(0)) {
                Tutorials.instance.IsReadyShowTutBuildTable = true;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(1f);
            CameraMove.instance.SetTargetFollow(customer.transform);
            customer.m_Animator.SetTrigger("IsTalk");

        }
        LobbyRoomManager.instance.ProcessOrder(customer.LobbyTarget);
        while (!LobbyRoomManager.instance.IsOrderFinish(customer.LobbyTarget)) {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.5f);

        if (IsHasEmptyTable()) {
            GameManager.instance.SpawnCashFx(customer.transform);
            LobbyRoomManager.instance.PaymentOrder(customer.LobbyTarget);
            LobbyRoomManager.instance.OutLobby(customer);

            customer.StateMachine.ChangeState(customer.customerUseFood);
            ProfileManager.PlayerData.researchManager.AddResearchValue();
        } else {
            LobbyRoomManager.instance.OutLobby(customer);
            if (Random.Range(0, 100) >= 50) customer.ShowAngryEmoji();
            customer.StateMachine.ChangeState(customer.customerOutRestaurant);
        }
    }

    bool IsHasEmptyTable() {
        if (TableManager.instance.IsHasTableEmpty()) return true;
        return false;
    }
    public override void Execute(Customer go) {
        if (GameManager.instance.IsPauseGame) return;
        if (go.LobbyTarget != null)
            go.AIRotateToTarget2(go.LobbyTarget.transform);

    }
    public override void Exit(Customer go) {
        customer.LobbyTarget = null;
    }

}
