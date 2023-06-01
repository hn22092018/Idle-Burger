using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyReception : BaseStaff {

    public void StartOrder(float time) {
        m_Animator.SetTrigger("IsTalk");
        isFree = false;
        CalculateWorkEfficiencyBySkin(ref time);
        timeIndicator.InitTime(time);
    }
    public bool IsCompletedOrder() {
        bool isFinish = timeIndicator.IsFinish();
        if (isFinish) {
            timeIndicator.Hide();
            isFree = true;
        }
        return isFinish;
    }
    public void FinishOrder() {
        //DropReputation();
    }
}
