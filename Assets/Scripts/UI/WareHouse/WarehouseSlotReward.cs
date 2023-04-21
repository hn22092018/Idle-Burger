using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WarehouseSlotReward : MonoBehaviour
{
    [SerializeField] Text txtCount;
    [SerializeField] Image imgIcon;
    [SerializeField] Transform trsWrapItem;
    [SerializeField] float timePunch;
    [SerializeField] GameObject effectObj;

    public void InitData(ItemReward itemReward) {
        txtCount.text = itemReward.AmountToString();
        imgIcon.sprite = itemReward.spr;
        trsWrapItem.gameObject.SetActive(true);
        trsWrapItem.DOPunchScale(new Vector3(.2f, .2f, 0), timePunch, 1).OnComplete(()=> {
            effectObj.SetActive(true);
        });
    }

    public void OnTurnOff() {
        effectObj.SetActive(false);
        gameObject.SetActive(false);
        trsWrapItem.gameObject.SetActive(false);
    }
}
