using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIDOTween : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool b_Button;
    [ConditionalHide("b_Button", true)] public float f_DurationButton;
    [ConditionalHide("b_Button", true)] public Vector3 v_ScaleUp;
    [ConditionalHide("b_Button", true)] public Vector3 v_ScaleDown;
    public bool b_Move;
    [ConditionalHide("b_Move", true)] public Transform trs_Target;
    [ConditionalHide("b_Move", true)] public bool b_FirstMove;
    [ConditionalHide("b_FirstMove", true)] public Vector3 trs_FirstTarget;
    [ConditionalHide("b_Move", true)] public float f_DurationMove;
    [ConditionalHide("b_Move", true)] public float f_DurationFirstMove;
    [ConditionalHide("b_FirstMove", true)] public float f_DurationRotage;
    [ConditionalHide("b_FirstMove", true)] public Vector3 f_RotageTo;
    UnityAction action;
    void Start() {
        if (b_FirstMove)
        {
            f_RotageTo.z = Random.Range(0, 360);
            transform.DOMove(trs_FirstTarget, f_DurationFirstMove).OnComplete(StartWaitMove);
            transform.DORotate(f_RotageTo, f_DurationRotage, RotateMode.FastBeyond360);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (b_Button)
            transform.DOScale(v_ScaleDown, f_DurationButton);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (b_Button)
            transform.DOScale(v_ScaleUp, f_DurationButton);
    }
    void StartWaitMove() {
        StartCoroutine(IE_MoveTarget());
    }
    IEnumerator IE_MoveTarget() {
        yield return new WaitForSeconds(.25f);
        OnMoveTarget();
    }
    public void OnMoveTarget() {
        if (b_Move)
            transform.DOMove(trs_Target.position, f_DurationMove).OnComplete(CallAction);
    }
    void CallAction() { 
        action(); 
        Destroy(gameObject);
    }
    public void SetTargetMove(Transform target) { trs_Target = target; }
    public void SetFirstTargetMove(Vector3 target, UnityAction action) {
        trs_FirstTarget = target;
        this.action = action;
    }
}
