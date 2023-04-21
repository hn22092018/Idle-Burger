using DG.Tweening;
using DG.Tweening.Plugins.Options;
using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverCarCustomer : MonoBehaviour {
    [SerializeField] Transform[] meshs;
    [SerializeField] DOTweenPath currentPath;
     public Research foodOrder;
    [SerializeField] Animator carAnimator;
    [SerializeField] UIFoodOrderIcon foodOrderIcon;
    [SerializeField] UIEmojiIcon emojiIcon;
    bool IsMove;
    bool IsCheckOverlapOtherCar;
    public DeliverPosition deliverPosTarget;
    public void OnInit() {
        for (int i = 0; i < meshs.Length; i++) {
            meshs[i].gameObject.SetActive(false);
        }
        if (meshs.Length > 0) meshs[Random.Range(0, meshs.Length)].gameObject.SetActive(true);
        currentPath = DeliverRoomManager.instance.BeginPath;
        var path = transform.DOPath(currentPath.path.wps, currentPath.duration).SetSpeedBased(currentPath.isSpeedBased).SetEase(Ease.Linear).OnComplete(() => {
            CheckQueue();
        }).plugOptions.orientType = OrientType.ToPath;
        IsMove = true;
        IsPauseTween = false;
        IsCheckOverlapOtherCar = true;
        foodOrderIcon.gameObject.SetActive(false);
        emojiIcon.gameObject.SetActive(false);
    }
    void CheckQueue() {
        deliverPosTarget = DeliverRoomManager.instance.GetDeliverPostionEmpty(this);
        if (deliverPosTarget == null) {
            // No Empty Queue
            transform.position = DeliverRoomManager.instance.OutPath.transform.position;
            currentPath = DeliverRoomManager.instance.OutPath;
            transform.DOPath(currentPath.path.wps, currentPath.duration).SetSpeedBased(currentPath.isSpeedBased).SetEase(Ease.Linear).SetLookAt(0.01f, true).OnComplete(() => {
                PoolManager.Pools["GameEntity"].Despawn(transform);
            });
        } else {
            IsCheckOverlapOtherCar = false;
            transform.position = DeliverRoomManager.instance.QueueInPaths[deliverPosTarget.IndexQueue].transform.position;
            currentPath = DeliverRoomManager.instance.QueueInPaths[deliverPosTarget.IndexQueue];
            transform.DOPath(currentPath.path.wps, currentPath.duration, PathType.CatmullRom).SetSpeedBased(currentPath.isSpeedBased).SetEase(Ease.Linear).SetLookAt(0.01f, true).OnComplete(() => {
                OnOrderFood();
            });
        }
    }
    void OnOrderFood() {
        IsMove = false;
        foodOrder = ProfileManager.PlayerData.researchManager.GetRandomFood();
        foodOrderIcon.gameObject.SetActive(true);
        foodOrderIcon.ShowIcon(foodOrder.icon);
        DeliverRoomManager.instance.OrderStaff(this);
    }
    public float GetOrderFoodValue() {
        int level = ProfileManager.PlayerData.researchManager.GetLevelByName(foodOrder.researchName);
        if (foodOrder != null && level > 0)
            return (float)foodOrder.CalculateProfit(level);
        return 0;
    }
    public void OnReceiveFoodFromStaff() {
        IsMove = true;
        IsCheckOverlapOtherCar = false;
        foodOrderIcon.gameObject.SetActive(false);
        emojiIcon.gameObject.SetActive(true);
        if (GetOrderFoodValue() > 0) {
            emojiIcon.ShowFunnyEmoji(3);
        } else {
            emojiIcon.ShowAngryEmoji(3);
        }
        DeliverRoomManager.instance.OutPosition(this);
        currentPath = DeliverRoomManager.instance.QueueOutPaths[deliverPosTarget.IndexQueue];
        transform.DOPath(currentPath.path.wps, currentPath.duration).SetSpeedBased(currentPath.isSpeedBased).SetEase(Ease.Linear).SetLookAt(0.01f, true).OnComplete(() => {
            PoolManager.Pools["GameEntity"].Despawn(transform);
        });
    }
    int rayLength = 4;
    bool IsPauseTween;
    private void Update() {
        carAnimator.SetBool("IsIdle", !IsMove);
        foodOrderIcon.UpdateAngle();
        //Debug.DrawRay(transform.position, transform.forward * rayLength, Color.red);
        if (IsCheckOverlapOtherCar) {
            RaycastHit raycastHit;
            if(Physics.Raycast(transform.position+new Vector3(0,1f,0), transform.forward,out raycastHit, rayLength)){
                if (raycastHit.collider.tag.Equals(compareTag)) {
                    transform.DOPause();
                    IsPauseTween = true;
                    IsMove = false;
                }
            }
            else {
                if (IsPauseTween) {
                    IsMove = true;
                    IsPauseTween = false;
                    transform.DOPlay();
                }
            }
        }
    }
    string compareTag = "DeliverCarCustomer";

}
