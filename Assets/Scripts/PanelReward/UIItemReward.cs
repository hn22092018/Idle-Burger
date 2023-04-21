using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemReward : MonoBehaviour {
    [SerializeField] Image imgIcon;
    [SerializeField] Text txtTitle, txtDes;
    public void Setup(ItemReward item) {
        imgIcon.sprite = item.spr;
        txtTitle.text = item.TypeToString();
        txtDes.text = item.AmountToString();
    }
}
