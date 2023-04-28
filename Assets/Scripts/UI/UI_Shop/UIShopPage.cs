using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopPage : MonoBehaviour {
    public TabName shopName;
    protected List<CardIAPConfig> listCardIAPs = new List<CardIAPConfig>();
    protected List<OfferData> listOfferData = new List<OfferData>();
    RectTransform rectTransform;
    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Start() {
        InitData();
    }

    public virtual void InitData() {

    }
    public virtual float GetPageHeight() {
        return rectTransform.rect.height;
    }
}
