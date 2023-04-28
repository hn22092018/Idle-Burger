using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardReward : MonoBehaviour
{
    public Image UIIcon;
    public Text UIName;
    public Image UIFrame;

    public RectTransform rt;
    Vector3 velocityPos = Vector3.zero;
    bool biggestScale = false;

    Vector3 velocitySize = Vector3.zero;

    public RectTransform nameRt;
    Vector3 velocityName = Vector3.zero;
    Vector3 velocityNameSize = Vector3.zero;
    bool showName = false;
    float showCardTimer = 0;

    public bool toFinalPos;
    public Transform finalPos;
    Vector3 velocityFinalPos = Vector3.zero;
    Vector3 velocityFinalSize = Vector3.zero;

    void Start() {
        rt = GetComponent<RectTransform>();
    }
    public void SetUp(CardNormalConfig card) 
    {   
        UIIcon.sprite = card.sprOn;
        UIName.text = card.GetName();
        //UIFrame.sprite = PanelReward.instance.cardFrames[(int)card.cardRarity];
        biggestScale = false;
        rt.anchoredPosition = new Vector2(1280f, 0f);
        nameRt.anchoredPosition = new Vector2(0f, 0f);
    }
    public void Onclick()
    {
        toFinalPos = true;

    }
    void Update() 
    {
        if(Vector2.Distance(rt.anchoredPosition,new Vector2(0f, 0f)) < 30f) {
            showName = true;
        }
        if (!toFinalPos)
        {
            rt.anchoredPosition = Vector3.SmoothDamp(rt.anchoredPosition, new Vector2(0f, 0f), ref velocityPos, 1f);
            rt.sizeDelta = Vector3.SmoothDamp(rt.sizeDelta, new Vector2(1290, 1935), ref velocitySize, 1.2f);
            if (rt.sizeDelta.x >= 1200f)
            {
                biggestScale = true;
            }
            if (biggestScale)
            {
                rt.sizeDelta = Vector3.SmoothDamp(rt.sizeDelta, new Vector2(900, 1350), ref velocitySize, 0.5f);
                if (rt.sizeDelta.x <= 930f)
                {
                    showName = true;
                }
            }
        }
        if(showName) {
            showCardTimer += Time.deltaTime;
            nameRt.anchoredPosition = Vector3.SmoothDamp(nameRt.anchoredPosition, new Vector2(0f, -915f), ref velocityName, 0.2f); 
            nameRt.sizeDelta = Vector3.SmoothDamp(nameRt.sizeDelta, new Vector2(1200, 400), ref velocityNameSize, 0.2f); 
            if(showCardTimer >= 3f)
            {
                toFinalPos = true;
                showName = false;
            }
        }
        if(toFinalPos)
        {
            transform.position = Vector3.SmoothDamp(transform.position, finalPos.position, ref velocityFinalPos, 0.5f);
            rt.sizeDelta = Vector3.SmoothDamp(rt.sizeDelta, finalPos.GetComponent<RectTransform>().sizeDelta, ref velocityFinalSize, 0.5f);
            if(Vector2.Distance(transform.position, finalPos.position) < 50f)
            {
                //PanelReward.instance.ContinueShowListEarnedCardToUI();
                Destroy(gameObject);
            }
        }
    }
}
