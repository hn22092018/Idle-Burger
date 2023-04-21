using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIObjectMove : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] AnimationCurve curvePosition, curveNoise;
    Transform targetPoint;
    Transform startPoint;
    Transform trsMove;
    [SerializeField] bool needMove;
    float timeMove;
    Vector3 horizontalVector;
    float positionNoise;
    float magnitude;
    UnityAction actionDone;

    private void Start()
    {
        trsMove = transform;
    }

    public void PrepareToMove(Transform startPoint, Transform targetPoint) {
        this.startPoint = startPoint;
        this.targetPoint = targetPoint;
        magnitude = Random.Range(-1, 2);
        Vector2 direction = (Vector2)(targetPoint.position - startPoint.position);
        horizontalVector = Vector2.Perpendicular(direction);
        timeMove = 0f;
    }

    public void StartMove(UnityAction actionDone = null) {
        if (actionDone != null)
            this.actionDone = actionDone;
        needMove = true;
    }

    public void Update()
    {
        if (needMove)
        {
            if (timeMove < curvePosition.keys[curvePosition.length - 1].time)
            {
                positionNoise = curveNoise.Evaluate(timeMove);
                trsMove.position = Vector2.Lerp(startPoint.position, targetPoint.position, curvePosition.Evaluate(timeMove))
                    + new Vector2(positionNoise * horizontalVector.x * magnitude, positionNoise * horizontalVector.y * magnitude);
                timeMove += Time.deltaTime * speed;
            }
            else
            {
                trsMove.position = targetPoint.position;
                if (actionDone != null)
                    actionDone();
                needMove = false;
            }
        }
    }
}
