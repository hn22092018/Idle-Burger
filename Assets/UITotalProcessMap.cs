using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITotalProcessMap : MonoBehaviour
{
    public Transform processTrans;
    private void Update() {
        processTrans.localScale = new Vector3(GameManager.instance.mapProcess, 1, 1); 
    }
}
