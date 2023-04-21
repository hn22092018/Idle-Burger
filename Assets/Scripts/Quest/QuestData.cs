using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDataAsset", menuName = "ScriptableObjects/QuestDataAsset", order = 1)]
public class QuestData : ScriptableObject
{
    public List<Quest> questList;
}
