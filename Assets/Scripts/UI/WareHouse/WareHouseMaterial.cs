using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WareHouseMaterialType {
    Cheese,
    Pepper,
    Sugar,
    Carot
}
[System.Serializable]
public class WareHouseMaterial
{
    public WareHouseMaterialType wareHouseMaterialType;
    public Sprite icon;
    public int priceSale;
}
