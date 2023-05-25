using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TipType {
    Receptionist, Waiter, Chef
}
public class Tip : MonoBehaviour {
    [SerializeField] TipType type;
    [SerializeField] GameObject canvasMoney, obj3DMoney;
    // Update is called once per frame
    BigNumber value = 0;
    void Update() {
        if (type == TipType.Receptionist) {
            value = ProfileManager.PlayerData.GetTipReception();
        } else if (type == TipType.Waiter) {
            value = ProfileManager.PlayerData.GetTipWaiter();
        } else if (type == TipType.Chef) {
            value = ProfileManager.PlayerData.GetTipChef();
        } 
        canvasMoney.SetActive(value > 0);
        obj3DMoney.SetActive(value > 0);
        obj3DMoney.transform.localScale = Vector3.one;
    }
}
