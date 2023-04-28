using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardDataAsset", order = 1)]
public class CardData : ScriptableObject
{
    public List<CardNormalConfig> CardInfoList;
}
