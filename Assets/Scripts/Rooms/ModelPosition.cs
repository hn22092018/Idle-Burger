using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModelPosition<Enum> {

    /// <summary>
    /// 3D model type 
    /// </summary>
    public Enum type;
    /// <summary>
    ///  Model Level
    /// </summary>
    public int level;
    /// <summary>
    ///  Root Container Model In Room
    /// </summary>
    public Transform RootObject;
    [HideInInspector]
    public Transform currentModel;
    public bool IsInitItem;
}