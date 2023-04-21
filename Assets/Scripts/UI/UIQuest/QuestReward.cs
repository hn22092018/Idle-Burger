using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReward : MonoBehaviour
{
    public Vector3 takeOffPos;
    bool takeOff;
    Vector3 velocityTakeOff = Vector3.zero;
    public Transform rewardBoxRT;
    Vector3 velocityPos = Vector3.zero;
    float timer;
    public float flyTime = 0.45f;

    private void Start()
    {
        takeOffPos = transform.position + new Vector3(Random.Range(-300f, 300f), Random.Range(-250f, 150f), 0);
        transform.DOMove(takeOffPos, flyTime*2).OnComplete(ToRewardBox);
    }

    void ToRewardBox()
    {
        transform.DOMove(rewardBoxRT.position, flyTime*3).OnComplete(Reached);
        transform.DOScale(0.2f, flyTime * 3);
    }
    void Reached()
    {
        Destroy(gameObject);
    }
}
