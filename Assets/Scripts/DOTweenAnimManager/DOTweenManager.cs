using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
[System.Serializable]
public class DOTweenManager 
{
    [Range(0, 1)]
    [SerializeField] float shakeDuration;
    [Range(0,1)]
    [SerializeField] float shakeStrength;
    public void ShakeObj(Transform objTransform) {
        //objTransform.DOShakePosition(shakeDuration, shakeStrength);
        //objTransform.DOShakeRotation(shakeDuration, shakeStrength);
        objTransform.DOShakeScale(shakeDuration, shakeStrength);
    }
    UnityAction onComplete;
    public void MoveObj(Transform objTransform,Vector3 target, float duration = .2f, UnityAction onCompleteFunction = null) {
        Tween myTween = objTransform.DOMove(target, duration);
        if (onCompleteFunction != null)
        {
            onComplete = onCompleteFunction;
            myTween.OnComplete(OnComplete);
        }
    }
    public void ScaleObj(Transform objTransform, Vector3 scaleTo, float duration = .2f, UnityAction onCompleteFunction = null) {
        Tween myTween = objTransform.DOScale(scaleTo, duration);
        if (onCompleteFunction != null)
        {
            onComplete = onCompleteFunction;
            myTween.OnComplete(OnComplete);
        }
    }
    public void ShakePosition(Transform objTransform, UnityAction onCompleteFunction = null, float strength = .5f, float duration = .5f) {
        Tween myTween = objTransform.DOShakePosition(duration, strength, 10000);
       
        if (onCompleteFunction != null)
        {
            onComplete = onCompleteFunction;
            myTween.OnComplete(OnComplete);
        }
    }
    public void DoFade(CanvasGroup canvasGroup, float fadeTo, UnityAction onComplete = null) {
        Tween myTween = canvasGroup.DOFade(fadeTo, .25f);
        if (onComplete != null)
        {
            this.onComplete = onComplete;
            myTween.OnComplete(OnComplete);
        }
    }
    public void PunchScale(Transform objTransform,Vector3 vectorPuch, UnityAction onComplete = null) {
        Tween myTween = objTransform.DOPunchScale(vectorPuch, .25f);

        if (onComplete != null)
        {
            this.onComplete = onComplete;
            myTween.OnComplete(OnComplete);
        }
    }
    void OnComplete() {
        if (onComplete == null)
            return;
        onComplete();
        onComplete = null;
    }
}
