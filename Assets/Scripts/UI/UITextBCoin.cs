using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextBCoin : MonoBehaviour {
    [SerializeField] Text txtValue;
    // Update is called once per frame

    void Start() {
        UpdateValue();
        EventManager.AddListener(EventName.UpdateBCoin.ToString(), UpdateValue);
    }
    void UpdateValue() {
        txtValue.text = ProfileManager.PlayerData.GetBurgerCoin().ToString();
    }

}
