using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHummanMiddlePoint : MonoBehaviour {
    public Transform Neck;
    public Vector3 GetPoint() {
        return new Vector3(Neck.position.x, 0, Neck.position.z);
    }
}
