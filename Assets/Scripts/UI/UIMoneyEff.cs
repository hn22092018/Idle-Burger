using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoneyEff : MonoBehaviour {
    public TextMesh txtValue;
    public void Show(string value, Transform obj) {
        transform.position = obj.transform.position + new Vector3(0, 2, 0);
        transform.localEulerAngles = new Vector3(40, 45, 0);
        txtValue.text = value.ToString();
        gameObject.SetActive(true);
    }
    public void Off() {
        PoolManager.Pools["GameEntity"].Despawn(transform);
    }
}
