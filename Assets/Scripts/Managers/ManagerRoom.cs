using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerRoom : MonoBehaviour
{
    public static ManagerRoom instance;
    public Transform[] movePositions;
    public Transform[] chairPosition;
    [HideInInspector] public ResManager currentManagerStaff;
    [HideInInspector] public bool IsAccpetNegotiation;
    private void Awake() {
        instance = this;
    }
    public Transform GetChairPositionForFamousPeople() {
        return chairPosition[2];
    }
    public Transform GetChairPositionForManager() {
        return chairPosition[1];
    }
}
