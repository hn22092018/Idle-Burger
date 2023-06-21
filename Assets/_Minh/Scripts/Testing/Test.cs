using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Test : MonoBehaviour {
    public float basecose;
    public int level;
    [Button]
    void TestBig() {
        BigNumber price = 120;
        price = StatCostConfig.Instance.GetCost(basecose, level);
        Debug.Log(price);
    }
}
