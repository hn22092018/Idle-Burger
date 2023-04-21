using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WareHouseMaterialCompletePanel : MonoBehaviour
{
    [SerializeField] AnimationCurve movePositionCurve;
    [SerializeField] AnimationCurve moveNoiseCurve;
    [SerializeField] Transform trsMove;
    [SerializeField] Transform trsIcon;
    [SerializeField] float magnitude;
    Vector3 startPoint, targetPoint, horizontalVector;
    float timeAnim;
    float noisePosition;
    bool animMove;
    public void PrepareAnim(Vector3 startPosition, Vector3 targetPosition)
    {
        timeAnim = 0;
        trsMove.position = startPosition;
        trsIcon.DOPunchScale(new Vector3(.5f, .5f, 0), .5f).OnComplete(()=> {
            StartCoroutine(WaitAnim());
        });
        startPoint = startPosition;
        targetPoint = targetPosition;
        Vector2 direction = targetPoint - startPoint;
        horizontalVector = Vector2.Perpendicular(direction);
    }
    IEnumerator WaitAnim() {
        yield return new WaitForSeconds(1f);
        animMove = true;
    }
    private void FixedUpdate()
    {
        if (animMove)
        {
            if (timeAnim < movePositionCurve.keys[movePositionCurve.length - 1].time)
            {
                noisePosition = moveNoiseCurve.Evaluate(timeAnim);
                trsMove.position = Vector2.Lerp(startPoint, targetPoint, movePositionCurve.Evaluate(timeAnim)) + new Vector2(noisePosition * horizontalVector.x * magnitude, noisePosition * horizontalVector.y * magnitude);
                timeAnim += Time.deltaTime;
            }
            else
            {
                trsMove.position = targetPoint;
                animMove = false;
                trsIcon.DOPunchScale(new Vector3(.5f, .5f, 0), .2f).OnComplete(()=> {
                    StartCoroutine(IE_Close());
                });
            }
        }
    }
    IEnumerator IE_Close() {
        yield return new WaitForSeconds(.1f);
        gameObject.SetActive(false);
    }
}
