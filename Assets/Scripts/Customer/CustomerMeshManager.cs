using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CustomerMeshConfig {
    public Mesh[] heads, bodys;
    public Avatar avatar;
}
public class CustomerMeshManager : MonoBehaviour
{
    public static CustomerMeshManager instance;
    public CustomerMeshConfig[] configs;
    private void Awake() {
        instance = this;
    }
    public (Mesh, Mesh, Avatar) GetRandom() {
        int rd = Random.Range(0, configs.Length);
        Avatar avatar = configs[rd].avatar;
        Mesh head = configs[rd].heads[Random.Range(0, configs[rd].heads.Length)];
        Mesh body = configs[rd].bodys[Random.Range(0, configs[rd].bodys.Length)];
        return (head,body,avatar);
    }
}
