using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UISaleOffSlot : UIShopSlot
{
    [Header("LimitTimeOffer")]
    public Text limitText;
    TimeSpan timeRemaining;
    DateTime timeEnd;
    public override void ChangeTimeTextOffer(string timeText) {
        limitText.text = timeText;
    }
}