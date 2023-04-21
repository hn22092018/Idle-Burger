using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRoomEffect : MonoBehaviour {
    public Transform[] DropObjects;
    public Transform StaffObject;
    public void OnDrop() {
        StartCoroutine(IOnDrop());
    }
    public IEnumerator IOnDrop() {
        if (StaffObject != null) StaffObject.gameObject.SetActive(false);
        for (int i = 0; i < DropObjects.Length; i++) {
            DropObjects[i].gameObject.SetActive(false);
            DropObjects[i].transform.localPosition = new Vector3(DropObjects[i].transform.localPosition.x, 3, DropObjects[i].transform.localPosition.z);
        }
        for (int i = 0; i < DropObjects.Length; i++) {
            DropObjects[i].gameObject.SetActive(true);
            if (i < DropObjects.Length - 1) {
                DropObjects[i].DOLocalMoveY(0, 0.8f).SetEase(Ease.OutBounce);
                yield return new WaitForSeconds(0.4f);
            } else {
                DropObjects[i].DOLocalMoveY(0, 0.8f).SetEase(Ease.OutBounce).OnComplete(() => {
                    if (StaffObject != null) StaffObject.gameObject.SetActive(true);
                });
            }
        }
    }
}
