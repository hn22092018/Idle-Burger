using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCostConfig {
    public int id;
    public int unlockPrice;
    public int baseCost;
    public float costRate;
    public float powerRate;
    public int levelMax;
    public int powerEnergy;
  
}
public class RoomCostConfig {
    public float buildTime;
    public List<int> unlockCosts = new List<int>();
    public int powerEnergy;

    public int GetUnlockCost(int buildingCount) {
        return unlockCosts[buildingCount];
    }
}