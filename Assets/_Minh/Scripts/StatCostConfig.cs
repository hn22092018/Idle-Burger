using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatCostConfig", menuName = "ScriptableObjects/Formula/StatCostConfig", order = 1)]
[GlobalConfig("Assets/Resources/GlobalConfig")]
public class StatCostConfig : GlobalConfig<StatCostConfig> {

    public StatConfig costConfig;
    public StatConfig profitConfig;

    public BigNumber GetCost(float baseCost, int level) {
        return Formula.CalculateByPercentage2(level, baseCost, costConfig.rate, costConfig.minRate, costConfig.powerRate, costConfig.powerRate2);
    }
    public BigNumber GetProfit(float baseCost, int level) {
        return Formula.CalculateByPercentage2(level, baseCost, profitConfig.rate, profitConfig.minRate, profitConfig.powerRate, profitConfig.powerRate2);
    }
}

[System.Serializable]
public class StatConfig {
    public float baseStat;
    public float rate;
    public float minRate;
    public float powerRate;
    public float powerRate2;
}
