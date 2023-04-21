using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StaffDataAsset", menuName = "ScriptableObjects/StaffDataAsset", order = 1)]
public class BaseStaffData : ScriptableObject
{

    /// <summary>
    /// Model name
    /// </summary>
    public string nameStaff;
    public int NameLocalizeID;
    /// <summary>
    /// Model Prefab
    /// </summary>
    public GameObject modelStaff;
    /// <summary>
    ///  Sprite To Show in UI
    /// </summary>
    public Sprite sprUIStaff;
    /// <summary>
    ///  Sprite To Show in UI All Staff Manager
    /// </summary>
    public Sprite sprUIStaffBig;
    /// <summary>
    /// Total staff in room
    /// </summary>
    public int TotalStaff;
    /// <summary>
    ///  money need to unlock staff
    /// </summary>
    public int costPerStaff = 500;
    public string description;
    public int DesLocalizeID;
    public string GetName() {
        return ProfileManager.Instance.dataConfig.GameText.GetTextByID(NameLocalizeID);
    }
    public string GetDes() {
        return ProfileManager.Instance.dataConfig.GameText.GetTextByID(DesLocalizeID);
    }
}
