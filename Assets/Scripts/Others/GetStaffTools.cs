using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStaffTools : MonoBehaviour
{
    public GameObject[] Tools;
    public void HideTool() {
        foreach(GameObject go in Tools) {
            go.SetActive(false);
        }
    }
    public void ShowTool() {
        foreach (GameObject go in Tools) {
            go.SetActive(true);
        }
    }
}
