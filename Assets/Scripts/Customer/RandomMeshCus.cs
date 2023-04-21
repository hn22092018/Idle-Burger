using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMeshCus : MonoBehaviour
{
    public GameObject[] Heads, Bodys;
    public void Hide() {
        foreach(GameObject go in Heads) {
            go.SetActive(false);
        }
        foreach (GameObject go in Bodys) {
            go.SetActive(false);
        }
    }
    public void RandomMesh() {
        Hide();
        int rd1 = Random.Range(0, Heads.Length);
        int rd2 = Random.Range(0, Bodys.Length);
        Heads[rd1].SetActive(true);
        Bodys[rd2].SetActive(true);
    }
}
