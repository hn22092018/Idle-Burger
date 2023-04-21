using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VerticalEffect : MonoBehaviour
{
    public bool onAnim;
    [SerializeField] Vector2 pointStart;
    [SerializeField] Vector2 pointTarget;
    [SerializeField] Vector2 vectorOffset;
    RectTransform myRect;
    float timeEffect;
    float timeEffectSetting;
    private void Awake()
    {
        myRect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        pointTarget = myRect.anchoredPosition;
        pointTarget += vectorOffset;
        pointStart = myRect.anchoredPosition;
        timeEffectSetting = Random.Range(1f, 2f);
        timeEffect = timeEffectSetting;
        onAnim = false;
    }
    private void Update()
    {
        if (timeEffect >= timeEffectSetting && !onAnim)
        {
            OnActive();
            timeEffect = 0;
        }
        else if (timeEffect < timeEffectSetting)
            timeEffect += Time.deltaTime;
    }
    public void OnActive()
    {
        if (onAnim)
            return;
        onAnim = true;
        myRect.anchoredPosition = pointStart;
        myRect.DOAnchorPos(pointTarget, 1f).OnComplete(() =>
        {
            myRect.DOAnchorPos(pointStart, 1f).OnComplete(() => {
                onAnim = false;
            });
        });
    }
}
