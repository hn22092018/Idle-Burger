using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ImageEffect : UIButtonEffect
{
    public float timeAnim;
    public override void OnPointerDown(PointerEventData eventData)
    {
    }
    public void Active() {
        OnActive();
    }
    public override void OnActive() {
        if (onAnim)
            return;
        onAnim = true;
        transform.localScale = vectorDefault;
        transform.DORotate(vectorRotage, timeAnim).OnComplete(() => {
            vectorRotage *= -1;
            transform.DORotate(vectorRotage, timeAnim).OnComplete(() => {
                transform.DORotate(Vector3.zero, timeAnim);
            });
        });
        transform.DOScale(vectorScale, timeAnim).OnComplete(() => {
            transform.DOScale(new Vector3(1.1f, 0.9f, 1f), timeAnim).OnComplete(() => {
                transform.DOScale(vectorDefault, timeAnim).OnComplete(() =>
                {
                    onAnim = false;
                });
            });
        });
    }
}
