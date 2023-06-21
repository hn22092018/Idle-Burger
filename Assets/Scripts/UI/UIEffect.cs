using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIEffect : MonoBehaviour {
    protected void ScaleEffectButton(Button btn, UnityAction callback = null) {
        btn.interactable = false;
        btn.transform.localScale = Vector3.one;
        btn.transform.DOScale(new Vector3(1.1f, 1.1f, 1), 0.2f).OnComplete(() => {
            btn.transform.DOScale(new Vector3(1f, 1, 1), 0.2f);
            btn.interactable = true;
            if (callback != null) callback();
        });
    }
}
