using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShortLinkToEmployeesSleep : MonoBehaviour {
    int count;
    public void OnGoToEmployee() {
        Transform t = GameManager.instance._EmployeeSleeping[0];
        CameraMove.instance.ChangePosition(t.transform.position, null, 20);
        count++;
        if (count == 2 || count % 10 == 0) {
            UIManager.instance.ShowPanelSkipSleepAds();
        }
    }
}
