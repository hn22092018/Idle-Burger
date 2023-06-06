using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCustomerMesh : MonoBehaviour {
    public Animator animator;
    public SkinnedMeshRenderer headMesh, bodyMesh;
    public void RandomMesh() {
        var config = CustomerMeshManager.instance.GetRandom();
        headMesh.sharedMesh = config.Item1;
        bodyMesh.sharedMesh = config.Item2;
        animator.avatar = config.Item3;

    }
}
