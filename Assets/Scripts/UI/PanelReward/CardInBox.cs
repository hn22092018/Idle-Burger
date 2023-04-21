using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInBox : MonoBehaviour
{
    [SerializeField] Image frameImg;
    [SerializeField] Image cardImg;
    [SerializeField] Text amountTxt;
    [SerializeField] Text nameTxt;

    public void SetUp(CardAmount card)
    {
        frameImg.sprite = UIManager.instance.GetPanel(UIPanelType.PanelReward).GetComponent<PanelReward>().GetFrameByRarity(card.card.cardRarity, card.card.cardType);
        cardImg.sprite = card.card.sprOn;
        amountTxt.text = card.amount.ToString();
        nameTxt.text = card.card.GetName();
    }
}
