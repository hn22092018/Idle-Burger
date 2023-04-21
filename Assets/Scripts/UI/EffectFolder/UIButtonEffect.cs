using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIButtonEffect : MonoBehaviour, IPointerDownHandler
{
    public bool onAnim;
    public Vector3 vectorScale;
    public Vector3 vectorDefault = new Vector3(1, 1, 1);
    public Vector3 vectorRotage = new Vector3(0, 0, 5f);

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnActive();
    }

    public virtual void OnActive()
    {
        if (onAnim)
            return;
        onAnim = true;
        transform.localScale = vectorDefault;
        transform.DORotate(vectorRotage, .1f).OnComplete(() => {
            transform.DORotate(Vector3.zero, .1f);
        });
        transform.DOScale(vectorScale, .1f).OnComplete(() => {
            transform.DOScale(new Vector3(1.1f, 0.9f, 1f), .1f).OnComplete(() => {
                transform.DOScale(vectorDefault, .1f).OnComplete(() =>
                {
                    onAnim = false;
                });
            });
        });
    }
}
