using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UIDailyRewardTab : MonoBehaviour {
    [SerializeField] Text txtDay;
    [SerializeField] GameObject checker;
    [SerializeField] Image imgIcon;
    [SerializeField] Text txtAmount;
    public void Setup(DailyRewards dailyReward)
    {
        txtDay.text = "DAY " + string.Format("{0:D2}", dailyReward.day);
        imgIcon.sprite = dailyReward.reward.spr;
        switch (dailyReward.reward.type)
        {
            case ItemType.Gem:
                txtAmount.text = "+" + dailyReward.reward.amount.ToString() + " Gems";
                break;
            case ItemType.AdvancedChest:
                txtAmount.text = "Advanced Chest!";
                break;
            case ItemType.TimeSkip_1H:
                txtAmount.text = "1H Profit";
                break;
            case ItemType.NormalSkinBox:
                txtAmount.text = "+1 Normal Chest";
                break;
            case ItemType.NormalChest:
                txtAmount.text = "+1 Rare Skin";
                break;
            default:
                txtAmount.text = "";
                break;
        }
        checker.SetActive(dailyReward.collected);
        UIManager.instance.dotweenManager.ShakeObj(transform);
    }
}
