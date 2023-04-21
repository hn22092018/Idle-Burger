using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    public Image UIIcon;
    public Text UIAmount;

    RectTransform rt;
    Vector3 velocityPos = Vector3.zero;
    Vector2 posToMove = new Vector2(0f, 0f);

    public RectTransform backGround;
    public float bgRotateSpeed = 3.5f;
    Vector3 velocityBG = Vector3.zero;

    Vector3 velocitySize = Vector3.zero;
    
    public void Start() {
        rt = GetComponent<RectTransform>();
    }
    public void SetUp(int amount, Sprite icon, Vector2 moveTo) 
    {
        UIIcon.sprite = icon;
        UIAmount.text = amount.ToString();
        posToMove = moveTo;
    }
    void Update() 
    {
        backGround.eulerAngles = Vector3.SmoothDamp(backGround.eulerAngles, new Vector3(0f, 0f, backGround.eulerAngles.z + bgRotateSpeed), ref velocityBG, 0.05f); 
        rt.anchoredPosition = Vector3.SmoothDamp(rt.anchoredPosition, posToMove, ref velocityPos, 0.35f); 
        rt.sizeDelta = Vector3.SmoothDamp(rt.sizeDelta, new Vector2(500, 500), ref velocitySize, 0.35f); 
    }
}
