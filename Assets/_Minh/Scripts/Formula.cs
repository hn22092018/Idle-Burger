using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formula {

    public BigNumber CalculateByPercentage(int level, BigNumber baseNum, float rate) {
        float num = (level - 1) * rate / 100f;
        BigNumber num1 = baseNum + baseNum * num;
        return num1;
    }
    public static BigNumber CalculateByPercentage2(int level, BigNumber baseNum, float rate, float minRate, float powerRate, float decreasePowerRate = 0) {
        BigNumber result = baseNum;
        minRate = minRate < 0.5f ? 0.5f : minRate;
        for (int i = 0; i < level; i++) {
            result = result + result * rate / 100f;
            {
                rate = rate - rate * powerRate / 100;
                rate = rate < minRate ? minRate : rate;
            }
            {
                powerRate = powerRate - powerRate * decreasePowerRate / 100;
                powerRate = powerRate < 0.2f ? 0.2f : powerRate;
            }
        }
        return result;
    }
    public BigNumber CalculateByPercentage3(int level, BigNumber baseNum, float rate, float rate2) {
        BigNumber result = baseNum;
        for (int i = 1; i < level; i++) {
            result += rate;
            rate += rate2;
        }
        return result;
    }

}