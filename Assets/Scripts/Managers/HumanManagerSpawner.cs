using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManagerSpawner : MonoBehaviour
{
    public StaffID staffID;
    public int GroupID;
    public Transform Prefab;
    public Transform[] Nodes;
    private void Awake() {
        Transform t = Instantiate(Prefab);
        t.GetComponent<StaffManager>().InitStaff(Nodes);
    }
}
