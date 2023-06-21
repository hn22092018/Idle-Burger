using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[Serializable]
public class BaseModelData<Enum> {

    /// <summary>
    /// Model name
    /// </summary>
    public string name;
    /// <summary>
    /// type
    /// </summary>
    public Enum type;
    /// <summary>
    /// 3D model by level 
    /// </summary>
    public GameObject[] models3D;
    public List<BigNumber> upgradePrices;
    [ShowIf("IsShowFieldEnergy")]
    public List<int> energyEarns;
    [HideInInspector]
    public int levelMax;
    /// <summary>
    ///  Sprite Show In UI To Upgrade
    /// </summary>
    public Sprite sprUI;
    bool IsShowFieldEnergy() {
        return type is PowerModelType.Power_BigGenerator || type is PowerModelType.Power_SmallGenerator;
    }
}

