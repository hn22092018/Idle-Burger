using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BuildData", menuName = "ScriptableObjects/BuildData", order = 1)]
public class BuildData : ScriptableObject {
    public List<BuildDataSetting> datas;
    public BuildDataSetting GetData(RoomID buildTarget) {
        for (int i = 0; i < datas.Count; i++) {
            BuildDataSetting buildData = datas[i];
            if (buildData.buildTarget == buildTarget) return buildData;
        }
        return null;
    }
    public int GetBuildCashPrice(RoomID buildTarget) {
        //if (ProfileManager.PlayerData.selectedWorld == 1) {
        //    switch (buildTarget) {
        //        case RoomID.BigTable1:
        //            if (GameManager.instance._RoomCostServer.Count > 0 && GameManager.instance._RoomCostServer[0] > 0)
        //                return GameManager.instance._RoomCostServer[0];
        //            break;
        //        case RoomID.BigTable2:
        //            if (GameManager.instance._RoomCostServer.Count > 1 && GameManager.instance._RoomCostServer[1] > 0)
        //                return GameManager.instance._RoomCostServer[1];
        //            break;
        //        case RoomID.BigTable3:
        //            if (GameManager.instance._RoomCostServer.Count > 2 && GameManager.instance._RoomCostServer[2] > 0)
        //                return GameManager.instance._RoomCostServer[2];
        //            break;
        //        case RoomID.BigTable4:
        //            if (GameManager.instance._RoomCostServer.Count > 3 && GameManager.instance._RoomCostServer[3] > 0)
        //                return GameManager.instance._RoomCostServer[3];
        //            break;
        //        case RoomID.DeliverRoom:
        //            if (GameManager.instance._RoomCostServer.Count > 5 && GameManager.instance._RoomCostServer[5] > 0)
        //                return GameManager.instance._RoomCostServer[5];
        //            break;
        //    }
        //} else if (ProfileManager.PlayerData.selectedWorld == 2) {
        //    switch (buildTarget) {
        //        case RoomID.BigTable1:
        //            if (GameManager.instance._RoomCostServer.Count > 0 && GameManager.instance._RoomCostServer[0] > 0)
        //                return GameManager.instance._RoomCostServer[0];
        //            break;
        //        case RoomID.BigTable2:
        //            if (GameManager.instance._RoomCostServer.Count > 1 && GameManager.instance._RoomCostServer[1] > 0)
        //                return GameManager.instance._RoomCostServer[1];
        //            break;
        //        case RoomID.BigTable3:
        //            if (GameManager.instance._RoomCostServer.Count > 2 && GameManager.instance._RoomCostServer[2] > 0)
        //                return GameManager.instance._RoomCostServer[2];
        //            break;
        //        case RoomID.BigTable4:
        //            if (GameManager.instance._RoomCostServer.Count > 3 && GameManager.instance._RoomCostServer[3] > 0)
        //                return GameManager.instance._RoomCostServer[3];
        //            break;
        //        case RoomID.BigTable5:
        //            if (GameManager.instance._RoomCostServer.Count > 4 && GameManager.instance._RoomCostServer[4] > 0)
        //                return GameManager.instance._RoomCostServer[4];
        //            break;
        //        case RoomID.BigTable6:
        //            if (GameManager.instance._RoomCostServer.Count > 5 && GameManager.instance._RoomCostServer[5] > 0)
        //                return GameManager.instance._RoomCostServer[5];
        //            break;
        //        case RoomID.DeliverRoom:
        //            if (GameManager.instance._RoomCostServer.Count > 7 && GameManager.instance._RoomCostServer[7] > 0)
        //                return GameManager.instance._RoomCostServer[7];
        //            break;
               
        //    }
        //}
        return GetData(buildTarget).cashRequire;
    }
    public int GetBuildEnergy(RoomID buildTarget) {
        return GetData(buildTarget).energyRequire;
    }

}
[System.Serializable]
public class BuildDataSetting {
    public RoomID buildTarget;
    public int DesLocalizeID;
    public string buildDes;
    public Sprite sprBuild;
    public int energyRequire;
    public int cashRequire;
    public int timeRequire;

    public string GetDes() {
        return ProfileManager.Instance.dataConfig.GameText.GetTextByID(DesLocalizeID);
    }
    public string GetBuildName() {
        return ProfileManager.Instance.dataConfig.GameText.RoomIDToString(buildTarget);
    }
    public override string ToString() {
        return buildTarget.ToString() + "_" + energyRequire + "_" + cashRequire + "_" + timeRequire;
    }
}
