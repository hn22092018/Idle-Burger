using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBuildTarget : MonoBehaviour {
    public RoomID target;
    public int GroupID;
    public TextMeshPro txtPrice;
    public bool IsGetPrice = false;
    private void Start() {
        if (IsGetPrice) {
            txtPrice.text =new BigNumber( GameManager.instance.buildData.GetBuildCashPrice(target)).IntToString();
        }
    }
}
