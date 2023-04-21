using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonAdBoost : MonoBehaviour
{
    [SerializeField] Button btnShow;
    [SerializeField] GameObject objBoostActive, objBoostDisactive;
    [SerializeField] Text txtBoostTime;
    AdBoostManager adBoostManager;
    [SerializeField] GameObject adsBoostX2Notify;

    // Start is called before the first frame update
    void Awake()
    {
        adBoostManager = ProfileManager.PlayerData.GetAdBoostManager();
        btnShow.onClick.AddListener(OnShowPanelAdBoost);
    }
    private void Update() {
        if (adBoostManager.IsBoostFinanceActive()) {
            adsBoostX2Notify.SetActive(false);
            objBoostActive.SetActive(true);
            objBoostDisactive.SetActive(false);
            txtBoostTime.text = adBoostManager.FinanceRemainTimeToString();
        }
        else {
            adsBoostX2Notify.SetActive(true);
            objBoostActive.SetActive(false);
            objBoostDisactive.SetActive(true);
        }
    }
    void OnShowPanelAdBoost()
    {
        UIManager.instance.ShowPanelAdBoost();
    }
}
