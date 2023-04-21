using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" || other.tag == "Staff") animator.SetTrigger("IsOpen");
    }
}
