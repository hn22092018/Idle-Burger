using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Test : MonoBehaviour {
    public float baseCost;
    public float costRate;
    public float powerRate;
    public BigNumber b;
    private void Start() {

    }
    [Button]
    void TestBig() {
        b += (b * 0.3f);
        Debug.Log(b);
    }
    [Button]
    public void Calculate() {
        string s = "";
        for (int i = 1; i <= 10; i++) {
            BigNumber num = GetCost2(i, baseCost, costRate, powerRate);
            s += "Level " + i + " ===== " + num + "\n";
        }
        Debug.Log(s);
    }
    public static BigNumber GetUnlockCost(int baseUnlockCost, int count) {
        return 500;
    }
    public static BigNumber GetCost(int level, float baseCost, float costRate, float powerRate = 1, float reduceRate = 0) {
        float pow = Mathf.Pow(level, powerRate);
        float num = Mathf.Pow(pow, costRate);
        float num1 = baseCost + num;
        int num2 = (((int)num1) / 10) * 10;

        BigNumber big = new BigNumber(num2);
        big = big * 100;
        big = big / (100 + reduceRate);
        return big;
    }
    public static int GetCost2(int level, float baseCost, float costRate, float powerRate = 1, float reduceRate = 0) {
        float num = 0;

        float num0 = costRate;
        float num3 = 0.8f;
        for (int i = 0; i < level - 1; i++) {

            if (i > 1) {
                num += num3 * num;
                num3 += 0.4f;
            } else {
                num += baseCost * num0;
                num0 *= powerRate;
            }
        }
        float num1 = baseCost + num;
        int num2 = 0;
        if (num1 < 500) {
            num2 = (((int)num1) / 10) * 10;
        } else {
            num2 = (((int)num1) / 100) * 100;
        }
        return num2;
    }
    [Button]
    public void TestHireCost() {
        string s = "";
        for (int i = 0; i <= 15; i++) {
            int num = GetSkinDefaultPrice(i);
            s += "ID_" + i + " = " + num + "\n";
        }
        Debug.Log(s);
    }
    public int GetSkinDefaultPrice(int index) {
        int price = 200 * (index + (int)(System.MathF.Pow(2, index)));
        return price;
    }
}
