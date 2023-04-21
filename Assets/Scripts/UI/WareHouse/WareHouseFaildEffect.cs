using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WareHouseFaildEffect : MonoBehaviour
{
    [SerializeField] Text txtFail;
    bool onAnim;
    public void ChangeFail(string failWhat) {
        if (onAnim) return;
        onAnim = true;
        gameObject.SetActive(true);
        txtFail.text = failWhat;
        Vector3 vectorTarget = transform.position;
        vectorTarget.y -= 200f;
        transform.position = vectorTarget;
        vectorTarget.y += 200f;
        transform.DOMove(vectorTarget, .25f).OnComplete(()=> {
            StartCoroutine(IE_TurnOff());
        });
    }
    IEnumerator IE_TurnOff() {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        onAnim = false;
    }
}
