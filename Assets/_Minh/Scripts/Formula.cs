using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formula {

    public static int GetCost2(int level, float baseCost, float costRate, float powerRate = 1, float reduceRate = 0) {
        float num = 0;

        float num0 = costRate;
        float num3 = 0.8f;
        for (int i = 0; i < level - 1; i++) {

            if (i > 0) {
                num += num3 * num;
                num3 += 0.2f;
            } else {
                num += baseCost * num0;
                num0 *= powerRate;
            }
        }
        float num1 = baseCost + num;
        int num2 = 0;
        if (num1 < 100) {
            num2 = (((int)num1) / 10) * 10;
        } else {
            num2 = (((int)num1) / 100) * 100;
        }
        return num2;
    }
}