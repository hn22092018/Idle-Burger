using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoneyNotify : MonoBehaviour {
    [SerializeField] Image imgBG;
    [SerializeField] Color[] colorsBG;
    [SerializeField] Text txtMoney;
    
    public void ShowSalaryCost(float cost) {
        imgBG.color = colorsBG[1];
        txtMoney.text = "-" + cost ;
    }
    public void ShowProfitEarn(BigNumber profit) {
        imgBG.color = colorsBG[0];
        txtMoney.text = "+" + profit.ToString() ;
    }
    public void Hide() {
        PoolManager.Pools["GameEntity"].Despawn(transform,0, PoolManager.Pools["GameEntity"].group);
    }
}
