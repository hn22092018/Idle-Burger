using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkinBox
{
    public string skinBoxName;
    public ItemType boxType;
    public List<SkinEarnable> skinEarnable;
}

[System.Serializable]
public class SkinEarnable
{
    public Rarity skinRarity;
    public int amount;
}

[CreateAssetMenu(fileName = "SkinBoxData", menuName = "ScriptableObjects/SkinBoxDataAsset", order = 1)]
public class SkinBoxDataAsset : ScriptableObject
{
    public List<SkinBox> skinBoxes;
    private void OnEnable()
    {
        
    }
}
