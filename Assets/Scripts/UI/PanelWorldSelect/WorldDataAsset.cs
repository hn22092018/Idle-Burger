using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WorldBaseData {
    public int NameLocalizeID;
    public string restaurantName;
    public int DesLocalizeID;
    public string restaurantDescription;
    public Sprite restaurantIcon;
    public int selectedWorld;
    public int burgerRequire;
    public string GetName() {
        return ProfileManager.Instance.dataConfig.GameText.GetTextByID(NameLocalizeID);
    }
    public string GetDes() {
        return ProfileManager.Instance.dataConfig.GameText.GetTextByID(DesLocalizeID);
    }
}

[CreateAssetMenu(fileName = "WorldBaseDataAsset", menuName = "ScriptableObjects/WorldBaseDataAsset", order = 1)]
public class WorldDataAsset : ScriptableObject
{
    public List<WorldBaseData> worldBaseDatas;
    public WorldBaseData GetDataByLevel(int level) {
        for(int i=0;i< worldBaseDatas.Count; i++) {
            if (worldBaseDatas[i].selectedWorld == level) return worldBaseDatas[i];
        }
        return null;
    }
}
