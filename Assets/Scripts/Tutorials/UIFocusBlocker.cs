using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIFocusBlocker : MonoBehaviour {
    public RectTransform T, B, L, R;
    public RectTransform CMask;
    public RectTransform PivotCenter;
    public int sizeCMask = 200;
    public float screenSize_X, screenSize_Y;
    public RectTransform hand;
    public RectTransform lightMask;
    RectTransform rectTransform;
    public void Off() {
        hand.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
    public void OnChangePos(Vector2 pos, int size = 300) {
        CMask.anchoredPosition = pos;
        sizeCMask = size;
        hand.anchoredPosition = CMask.anchoredPosition;
    }
  
    public void OnShow() {
        hand.gameObject.SetActive(true);
        this.gameObject.SetActive(true);

    }
    // Update is called once per frame
    void Update() {
        CMask.sizeDelta = new Vector2(sizeCMask, sizeCMask);
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        screenSize_X = rectTransform.rect.width;
        screenSize_Y = rectTransform.rect.height;
        float pos_X_CMask = CMask.anchoredPosition.x;
        float pos_Y_CMask = CMask.anchoredPosition.y;
        float pos_X_L = screenSize_X / 2 - sizeCMask / 2 + pos_X_CMask;
        L.sizeDelta = new Vector2(pos_X_L, L.sizeDelta.y);

        float pos_X_R = screenSize_X / 2 - sizeCMask / 2 - pos_X_CMask;
        R.sizeDelta = new Vector2(pos_X_R, R.sizeDelta.y);

        float pos_Y_T = screenSize_Y / 2 - sizeCMask / 2 - pos_Y_CMask;
        T.sizeDelta = new Vector2(sizeCMask, pos_Y_T);
        T.anchoredPosition = new Vector2(screenSize_X / 2 + pos_X_CMask - sizeCMask / 2, T.anchoredPosition.y);

        float pos_Y_B = screenSize_Y / 2 - sizeCMask / 2 + pos_Y_CMask;
        B.sizeDelta = new Vector2(sizeCMask, pos_Y_B);
        B.anchoredPosition = new Vector2(screenSize_X / 2 + pos_X_CMask - sizeCMask / 2, B.anchoredPosition.y);

        hand.anchoredPosition = CMask.anchoredPosition;
        lightMask.sizeDelta = new Vector2(sizeCMask * 1.5f, sizeCMask * 1.5f);
    }
    public void SetPivot(Transform transformTarget, int size = 300) {
        sizeCMask = size;
        PivotCenter.SetParent(transformTarget);
        PivotCenter.anchoredPosition = Vector2.zero;
        PivotCenter.SetParent(transform);
        CMask.anchoredPosition = PivotCenter.anchoredPosition;
    }
}
