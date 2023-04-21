using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextMoney : MonoBehaviour {
    [SerializeField] Text txtMoney;
    // Update is called once per frame

    void Start() {
        UpdateMoney();
        EventManager.AddListener(EventName.UpdateMoney.ToString(), UpdateMoney);
    }
    void UpdateMoney() {
        txtMoney.text = new BigNumber(GameManager.instance.GetCash()).ToString();
    }
   
}
