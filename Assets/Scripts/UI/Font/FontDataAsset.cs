using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TextType
{
    Text,
    Number,
    TextTitle
}

[CreateAssetMenu(fileName = "FontDataAsset", menuName = "ScriptableObjects/FontDataAsset", order = 1)]
public class FontDataAsset : ScriptableObject
{
    public Font textFont;
    public Font numberFont;
    public Font textTitleFont;
}
